using System;
using System.Collections.Generic;

namespace Events
{
    public class GameEventBus
    {
        private readonly Dictionary<Type, Delegate> _handlers = new();

        public void Subscribe<T>(Action<T> handler)
        {
            var type = typeof(T);
            if (_handlers.TryGetValue(type, out var existing))
            {
                _handlers[type] = Delegate.Combine(existing, handler);
            }
            else
            {
                _handlers[type] = handler;
            }
        }

        public void Unsubscribe<T>(Action<T> handler)
        {
            var type = typeof(T);
            if (_handlers.TryGetValue(type, out var existing))
            {
                var updated = Delegate.Remove(existing, handler);
                if (updated == null)
                {
                    _handlers.Remove(type);
                }
                else
                {
                    _handlers[type] = updated;
                }
            }
        }

        public void Publish<T>(T evt)
        {
            if (_handlers.TryGetValue(typeof(T), out var handler))
            {
                ((Action<T>)handler)?.Invoke(evt);
            }
        }
    }
}