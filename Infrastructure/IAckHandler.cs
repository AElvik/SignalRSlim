﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading.Tasks;

namespace PO.SignalR.Slim.Infrastructure
{
    public interface IAckHandler
    {
        Task CreateAck(string id);

        bool TriggerAck(string id);
    }
}
