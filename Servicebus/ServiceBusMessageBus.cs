// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using PO.SignalR.Slim.Messaging;
using PO.SignalR.Slim.Tracing;

namespace PO.SignalR.Slim.Servicebus
{
    /// <summary>
    /// Uses Windows Azure Service Bus topics to scale-out SignalR applications in web farms.
    /// </summary>
    public class ServiceBusMessageBus : ScaleoutMessageBus
    {
        private const string SignalRTopicPrefix = "SIGNALR_TOPIC";

        private ServiceBusConnectionContext _connectionContext;

        private TraceSource _trace;

        private readonly ServiceBusConnection _connection;
        private readonly string[] _topics;

        public ServiceBusMessageBus(IDependencyResolver resolver, ServiceBusScaleoutConfiguration configuration)
            : base(resolver, configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException("configuration");
            }

            // Retrieve the trace manager
            var traceManager = resolver.Resolve<ITraceManager>();
            _trace = traceManager["SignalR." + nameof(ServiceBusMessageBus)];

            _connection = new ServiceBusConnection(configuration, _trace);

            _topics = Enumerable.Range(0, configuration.TopicCount)
                                .Select(topicIndex => SignalRTopicPrefix + "_" + configuration.TopicPrefix + "_" + topicIndex)
                                .ToArray();

            _connectionContext = new ServiceBusConnectionContext(configuration, _topics, _trace, OnMessage, OnError, Open);

            Subscribe().GetAwaiter().GetResult();
        }

        protected override int StreamCount
        {
            get
            {
                return _topics.Length;
            }
        }

        protected override Task Send(int streamIndex, IList<Message> messages)
        {
            var scaleoutMessage = new ScaleoutMessage(messages);
            var bytes = scaleoutMessage.ToBytes();

            //var stream = ServiceBusMessage.ToStream(messages);

            TraceMessages(messages, "Sending");

            return _connectionContext.Publish(streamIndex, bytes);
        }

        private void OnMessage(int topicIndex, IEnumerable<Microsoft.Azure.ServiceBus.Message> messages)
        {
            //if (!messages.Any())
            //{
            //    // Force the topic to re-open if it was ever closed even if we didn't get any messages
            //    Open(topicIndex);
            //}

            //foreach (var message in messages)
            //{
            //    using (message)
            //    {
            //        ScaleoutMessage scaleoutMessage = ServiceBusMessage.FromBrokeredMessage(message);

            //        TraceMessages(scaleoutMessage.Messages, "Receiving");

            //        OnReceived(topicIndex, (ulong)message.EnqueuedSequenceNumber, scaleoutMessage);
            //    }
            //}
        }

        private async Task Subscribe()
        {
            await _connection.Subscribe(_connectionContext);
        }

        private void TraceMessages(IList<Message> messages, string messageType)
        {
            if (!_trace.Switch.ShouldTrace(TraceEventType.Verbose))
            {
                return;
            }

            foreach (Message message in messages)
            {
                _trace.TraceVerbose("{0} {1} bytes over Service Bus: {2}", messageType, message.Value.Array.Length, message.GetString());
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
            {
                if (_connectionContext != null)
                {
                    _connectionContext.Dispose();
                }

                if (_connection != null)
                {
                    _connection.Dispose();
                }
            }
        }
    }
}
