﻿namespace Cleanic.Framework
{
    using Cleanic.Application;
    using Cleanic.Core;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class InMemoryViewStore : IViewStore
    {
        public InMemoryViewStore(ILogger<InMemoryViewStore> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public Task<AggregateView> Load(AggregateViewInfo aggregateViewInfo, String aggregateId)
        {
            if (!_db.ContainsKey(aggregateViewInfo.Type)) return Task.FromResult<AggregateView>(null);

            return Task.FromResult(_db[aggregateViewInfo.Type].ContainsKey(aggregateId) ? _db[aggregateViewInfo.Type][aggregateId] : null);
        }

        public Task Save(AggregateView aggregateView)
        {
            if (!_db.TryGetValue(aggregateView.GetType(), out var entities))
            {
                _db.Add(aggregateView.GetType(), entities = new Dictionary<String, AggregateView>());
            }

            if (!entities.ContainsKey(aggregateView.AggregateId))
            {
                entities.Add(aggregateView.AggregateId, aggregateView);
            }
            else
            {
                entities[aggregateView.AggregateId] = aggregateView;
            }

            return Task.CompletedTask;
        }

        public Task Clear()
        {
            _db.Clear();
            return Task.CompletedTask;
        }

        private readonly Dictionary<Type, Dictionary<String, AggregateView>> _db = new Dictionary<Type, Dictionary<String, AggregateView>>();
        private readonly ILogger _logger;
    }
}