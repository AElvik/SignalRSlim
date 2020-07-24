// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace PO.SignalR.Slim.Infrastructure
{
    internal class BinaryMemoryPoolTextWriter : MemoryPoolTextWriter, IBinaryWriter
    {
        public BinaryMemoryPoolTextWriter(IMemoryPool memory)
            : base(memory)
        {
        }
    }
}
