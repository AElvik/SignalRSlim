// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Diagnostics.CodeAnalysis;
using System.Dynamic;
using PO.SignalR.Slim.Hubs.Pipeline;
using PO.SignalR.Slim.Infrastructure;

namespace PO.SignalR.Slim.Hubs
{
    public class StatefulSignalProxy : SignalProxy
    {
        private readonly StateChangeTracker _tracker;

        public StatefulSignalProxy(IConnection connection, IHubPipelineInvoker invoker, string signal, string hubName, string prefix, StateChangeTracker tracker)
            : base(connection, invoker, signal, prefix, hubName, ListHelper<string>.Empty)
        {
            _tracker = tracker;
        }

        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "The compiler generates calls to invoke this")]
        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            _tracker[binder.Name] = value;
            return true;
        }

        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "The compiler generates calls to invoke this")]
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            result = _tracker[binder.Name];
            return true;
        }

        protected override ClientHubInvocation GetInvocationData(string method, object[] args)
        {
            return new ClientHubInvocation
            {
                Hub = HubName,
                Method = method,
                Args = args,
                State = _tracker.GetChanges()
            };
        }
    }
}
