// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace PO.SignalR.Slim.Infrastructure
{
    internal class ListHelper<T>
    {
        public static readonly IList<T> Empty = new ReadOnlyCollection<T>(new List<T>());
    }
}
