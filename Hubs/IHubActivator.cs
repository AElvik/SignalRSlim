// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using PO.SignalR.Slim.Hubs.Lookup.Descriptors;

namespace PO.SignalR.Slim.Hubs
{
    public interface IHubActivator
    {
        IHub Create(HubDescriptor descriptor);
    }
}
