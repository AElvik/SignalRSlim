// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using PO.SignalR.Slim.Hubs.Lookup.Descriptors;
using PO.SignalR.Slim.Json;

namespace PO.SignalR.Slim.Hubs.Extensions
{
    public static class MethodExtensions
    {
        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "1", Justification = "The condition checks for null parameters")]
        public static bool Matches(this MethodDescriptor methodDescriptor, IList<IJsonValue> parameters)
        {
            if (methodDescriptor == null)
            {
                throw new ArgumentNullException("methodDescriptor");
            }

            if ((methodDescriptor.Parameters.Count > 0 && parameters == null)
                || methodDescriptor.Parameters.Count != parameters.Count)
            {
                return false;
            }

            return true;
        }
    }
}
