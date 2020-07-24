// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using PO.SignalR.Slim.Hubs.Pipeline;
using PO.SignalR.Slim.Infrastructure;

namespace PO.SignalR.Slim.Hubs
{
    public class GroupProxy : SignalProxy
    {
        public GroupProxy(IConnection connection, IHubPipelineInvoker invoker, string signal, string hubName, IList<string> exclude) :
            base(connection, invoker, signal, hubName, PrefixHelper.HubGroupPrefix, exclude)
        {

        }
    }
}
