// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Management;
using PO.SignalR.Slim.Tracing;

namespace PO.SignalR.Slim.Servicebus
{
    public class ServiceBusConnectionContext : IDisposable
    {
        private readonly ServiceBusScaleoutConfiguration _configuration;

        private readonly SubscriptionContext[] _subscriptions;
        private readonly TopicClient[] _topicClients;

        private readonly TraceSource _trace;

        public object SubscriptionsLock { get; private set; }
        public static readonly SemaphoreSlim TopicClientsLock = new SemaphoreSlim(1);

        public IList<string> TopicNames { get; private set; }
        public Action<int, IEnumerable<Microsoft.Azure.ServiceBus.Message>> Handler { get; private set; }
        public Action<int, Exception> ErrorHandler { get; private set; }
        public Action<int> OpenStream { get; private set; }

        public bool IsDisposed { get; private set; }

        public ManagementClient NamespaceManager { get; set; }

        public ServiceBusConnectionContext(ServiceBusScaleoutConfiguration configuration,
                                           IList<string> topicNames,
                                           TraceSource traceSource,
                                           Action<int, IEnumerable<Microsoft.Azure.ServiceBus.Message>> handler,
                                           Action<int, Exception> errorHandler,
                                           Action<int> openStream)
        {
            if (topicNames == null)
            {
                throw new ArgumentNullException("topicNames");
            }

            _configuration = configuration;

            _subscriptions = new SubscriptionContext[topicNames.Count];
            _topicClients = new TopicClient[topicNames.Count];

            _trace = traceSource;

            TopicNames = topicNames;
            Handler = handler;
            ErrorHandler = errorHandler;
            OpenStream = openStream;
            
            SubscriptionsLock = new object();
        }

        public Task Publish(int topicIndex, byte[] bytes)
        {
            if (IsDisposed)
            {
                return TaskAsyncHelper.Empty;
            }

            var message = new Microsoft.Azure.ServiceBus.Message(bytes)
            {
                TimeToLive = _configuration.TimeToLive
            };

            if (message.Size > _configuration.MaximumMessageSize)
            {
                _trace.TraceWarning("Message size {0}KB exceeds the maximum size limit of {1}KB : {2}", message.Size / 1024, _configuration.MaximumMessageSize / 1024, message);
            }

            return _topicClients[topicIndex].SendAsync(message);
        }

        internal void SetSubscriptionContext(SubscriptionContext subscriptionContext, int topicIndex)
        {
            lock (SubscriptionsLock)
            {
                if (!IsDisposed)
                {
                    _subscriptions[topicIndex] = subscriptionContext;
                }
            }
        }

        internal void SetTopicClients(TopicClient topicClient, int topicIndex)
        {
            if (!IsDisposed)
            {
                _topicClients[topicIndex] = topicClient;
            }
            
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (!IsDisposed)
                {
                    //lock (TopicClientsLock)
                    //{
                    //    lock (SubscriptionsLock)
                    //    {
                            for (int i = 0; i < TopicNames.Count; i++)
                            {
                                // BUG #2937: We need to null check here because the given topic/subscription
                                // may never have actually been created due to the lock being released
                                // between each retry attempt
                                var topicClient = _topicClients[i];
                                if (topicClient != null)
                                {
                                    //topicClient.Close();
                                    topicClient.CloseAsync().GetAwaiter().GetResult();
                                }

                                //var subscription = _subscriptions[i];
                                //if (subscription != null)
                                //{
                                //    subscription.Receiver.Close();
                                //    //NamespaceManager.DeleteSubscription(subscription.TopicPath, subscription.Name);
                                //}
                            }

                            IsDisposed = true;
                    //    }
                    //}
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}
