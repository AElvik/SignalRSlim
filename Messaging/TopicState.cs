﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace PO.SignalR.Slim.Messaging
{
    internal class TopicState
    {
        public const int NoSubscriptions = 0;
        public const int HasSubscriptions = 1;
        public const int Dying = 2;
        public const int Dead = 3;
    }
}
