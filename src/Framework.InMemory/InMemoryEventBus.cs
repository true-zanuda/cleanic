﻿namespace Cleanic.Framework
{
    using Cleanic.Core;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class InMemoryEventBus
    {
        public InMemoryEventBus(ILogger logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _busy = false;
            _queue = new Queue<AggregateEvent>();
            _eventSubscribers = new Dictionary<Type, List<Func<AggregateEvent, Task>>>();
        }

        public async Task Publish(AggregateEvent @event)
        {
            if (@event == null) throw new ArgumentNullException(nameof(@event));

            _queue.Enqueue(@event);
            _logger.LogInformation("{event} published (agg: {aggId})", @event.GetType(), @event.AggregateId);
            if (_busy) return;

            try
            {
                _busy = true;
                await HandleQueue();
            }
            finally
            {
                _busy = false;
            }
        }

        public void ListenEvents(Type eventType, Func<AggregateEvent, Task> listener)
        {
            if (!_eventSubscribers.ContainsKey(eventType)) _eventSubscribers.Add(eventType, new List<Func<AggregateEvent, Task>>());
            var current = _eventSubscribers[eventType];
            if (!current.Contains(listener)) current.Add(listener);
        }

        private async Task HandleQueue()
        {
            while (true)
            {
                if (_queue.Count == 0) return;
                var @event = _queue.Dequeue();
                var type = @event.GetType();

                if (_eventSubscribers.ContainsKey(type))
                {
                    var handlers = new List<Func<AggregateEvent, Task>>(_eventSubscribers[type]);
                    foreach (var handler in handlers) await handler(@event);
                }
            }
        }

        private readonly ILogger _logger;
        private Boolean _busy;
        private readonly Queue<AggregateEvent> _queue;
        private readonly Dictionary<Type, List<Func<AggregateEvent, Task>>> _eventSubscribers;
    }
}