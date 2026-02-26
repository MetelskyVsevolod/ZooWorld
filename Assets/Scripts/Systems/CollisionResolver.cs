using System;
using System.Collections.Generic;
using EventsHandling;
using EventsHandling.Events;
using Systems.Collision;
using Zenject;

namespace Systems
{
    public class CollisionResolver : IDisposable
    {
        private readonly GameEventBus _eventBus;
        private readonly List<CollisionStrategyBase> _strategies;

        [Inject]
        public CollisionResolver(GameEventBus eventBus, List<CollisionStrategyBase> strategies)
        {
            _eventBus = eventBus;
            _strategies = strategies;
            _eventBus.Subscribe<AnimalCollisionEvent>(OnCollision);
        }

        public void Dispose()
        {
            _eventBus.Unsubscribe<AnimalCollisionEvent>(OnCollision);
        }
        
        private void OnCollision(AnimalCollisionEvent evt)
        {
            var initiator = evt.Initiator;
            var other = evt.Other;

            if (!initiator.IsAlive || !other.IsAlive)
            {
                return;
            }

            if (initiator.GetInstanceID() > other.GetInstanceID())
            {
                return;
            }

            foreach (var strategy in _strategies)
            {
                if (strategy.TryResolve(initiator, other, _eventBus))
                {
                    break;
                }
            }
        }
    }
}