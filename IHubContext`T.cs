// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using PO.SignalR.Slim.Hubs;

namespace PO.SignalR.Slim
{
    /// <summary>
    /// Provides access to information about a <see cref="IHub"/>.
    /// </summary>
    public interface IHubContext<T>
    {
        /// <summary>
        /// Encapsulates all information about a SignalR connection for an <see cref="IHub"/>.
        /// </summary>
        IHubConnectionContext<T> Clients { get; }

        /// <summary>
        /// Gets the <see cref="IGroupManager"/> the hub.
        /// </summary>
        IGroupManager Groups { get; }
    }
}
