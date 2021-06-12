﻿// Copyright (c) Damir Dobric. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DotNetActors.Net
{
    /// <summary>
    /// Represents Interface for persistence. Below methods will be implemented as per the need
    /// </summary>
    public interface IPersistenceProvider
    {
        Task InitializeAsync(string name, Dictionary<string, object> setrtings, bool purgeOnStart = false, ILogger logger = null);

        Task PersistActor(ActorBase actorInstance);

        Task<ActorBase> LoadActor(ActorId actorId);

        Task Purge();
    }
}
