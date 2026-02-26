using System;
using System.Collections.Generic;
using Signals;
using Systems.Collision;
using Zenject;

namespace Systems
{
    public class CollisionResolver : IDisposable
    {
        private readonly SignalBus _signalBus;
        private readonly List<CollisionStrategyBase> _strategies;

        [Inject]
        public CollisionResolver(SignalBus signalBus, List<CollisionStrategyBase> strategies)
        {
            _signalBus = signalBus;
            _strategies = strategies;
            _signalBus.Subscribe<AnimalCollisionSignal>(OnCollision);
        }

        public void Dispose()
        {
            _signalBus.Unsubscribe<AnimalCollisionSignal>(OnCollision);
        }
        
        private void OnCollision(AnimalCollisionSignal signal)
        {
            var initiator = signal.Initiator;
            var other = signal.Other;

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
                if (strategy.TryResolve(initiator, other, _signalBus))
                {
                    break;
                }
            }
        }
    }
}