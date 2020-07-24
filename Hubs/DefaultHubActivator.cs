// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using PO.SignalR.Slim.Hubs.Lookup.Descriptors;

namespace PO.SignalR.Slim.Hubs
{
    public class DefaultHubActivator : IHubActivator
    {
        private readonly IDependencyResolver _resolver;

        public DefaultHubActivator(IDependencyResolver resolver)
        {
            _resolver = resolver;
        }

        public IHub Create(HubDescriptor descriptor)
        {
            if (descriptor == null)
            {
                throw new ArgumentNullException("descriptor");
            }

            if(descriptor.HubType == null)
            {
                return null;
            }

            object hub = _resolver.Resolve(descriptor.HubType) ?? Activator.CreateInstance(descriptor.HubType);
            return hub as IHub;
        }
    }
}
