// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using PO.SignalR.Slim.Hubs.Pipeline;
using PO.SignalR.Slim.Infrastructure;

namespace PO.SignalR.Slim.Hubs
{
    public class ConnectionIdProxy : SignalProxy
    {
        public ConnectionIdProxy(IConnection connection, IHubPipelineInvoker invoker, string signal, string hubName, params string[] exclude) :
            base(connection, invoker, signal, hubName, PrefixHelper.HubConnectionIdPrefix, exclude)
        {

        }
    }
}
