﻿using FrogsTalks.Domain;
using System;
using System.Collections.Generic;

namespace FrogsTalks.Application.Ports
{
    /// <summary>
    /// An abstract projections reader.
    /// </summary>
    /// <remarks>One of the application layer ports needed to have adapter in outer layer.</remarks>
    public interface IProjectionsReader
    {
        /// <summary>
        /// Load the projection by identifier.
        /// </summary>
        IProjection Load(Guid id);
    }

    /// <summary>
    /// An abstract projections repository.
    /// </summary>
    public interface IProjectionsRepository : IProjectionsReader
    {
        /// <summary>
        /// Save the projection for further loading by identifier.
        /// </summary>
        void Save(IProjection projection);
    }

    /// <summary>
    /// Projections repository working in memory.
    /// </summary>
    /// <remarks>
    /// This is an embedded <see cref="IProjectionsReader">port</see> adapter implementation for tests and experiments.
    /// </remarks>
    public class InMemoryStateStore : IProjectionsRepository
    {
        /// <summary>
        /// Load the projection by identifier.
        /// </summary>
        public IProjection Load(Guid id)
        {
            return _db.ContainsKey(id) ? _db[id] : null;
        }

        /// <summary>
        /// Save the projection for further loading by identifier.
        /// </summary>
        public void Save(IProjection projection)
        {
            if (!_db.ContainsKey(projection.Id)) _db.Add(projection.Id, projection);
            else _db[projection.Id] = projection;
        }

        private readonly Dictionary<Guid, IProjection> _db = new Dictionary<Guid, IProjection>();
    }
}