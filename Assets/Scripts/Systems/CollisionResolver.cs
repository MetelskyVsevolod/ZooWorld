using System;
using Animals.Core;
using EventsHandling;
using EventsHandling.Events;
using Zenject;
using Random = UnityEngine.Random;

namespace Systems
{
    public class CollisionResolver : IDisposable
    {
        private readonly GameEventBus _eventBus;

        [Inject]
        public CollisionResolver(GameEventBus eventBus)
        {
            _eventBus = eventBus;
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

            if (initiator.Config.Role == AnimalRole.Prey && other.Config.Role == AnimalRole.Prey)
            {
                return;
            }

            if (initiator.Config.Role == AnimalRole.Predator && other.Config.Role == AnimalRole.Predator)
            {
                ResolvePredatorVsPredator(initiator, other);
                return;
            }

            var predator = initiator.Config.Role == AnimalRole.Predator ? initiator : other;
            var prey = initiator.Config.Role == AnimalRole.Prey ? initiator : other;
            ResolvePreyVsPredator(predator, prey);
        }

        private void ResolvePreyVsPredator(Animal predator, Animal prey)
        {
            prey.Die();
            _eventBus.Publish(new AnimalAteEvent(predator));
        }

        private void ResolvePredatorVsPredator(Animal predatorA, Animal predatorB)
        {
            var loser = Random.value < 0.5f ? predatorA : predatorB;
            var winner = loser == predatorA ? predatorB : predatorA;
            loser.Die();
            _eventBus.Publish(new AnimalAteEvent(winner));
        }
    }
}