﻿namespace Cleanic.Core
{
    /// <summary>
    /// Represents an intent to change something in the domain.
    /// </summary>
    public abstract class Command : Message { }

    /// <summary>
    /// Command that not supposed to be used by external actors (exists only for this domain sagas).
    /// </summary>
    public abstract class InternalCommand : Command { }
}