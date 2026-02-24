using Animals.Core;
using Events;
using UnityEngine;
using Zenject;

namespace Systems
{
    public class CollisionResolver
    {
        private readonly GameEventBus _eventBus;

        [Inject]
        public CollisionResolver(GameEventBus eventBus)
        {
            _eventBus = eventBus;
            _eventBus.Subscribe<AnimalCollisionEvent>(OnCollision);
        }

        private void OnCollision(AnimalCollisionEvent evt)
        {
            var initiator = evt.Initiator;
            var other = evt.Other;

            if (!initiator.IsAlive || !other.IsAlive)
            {
                return;
            }

            if (initiator.Config.role == AnimalRole.Prey && other.Config.role == AnimalRole.Prey)
            {
                return;
            }

            if (initiator.Config.role == AnimalRole.Predator && other.Config.role == AnimalRole.Predator)
            {
                ResolvePredatorVsPredator(initiator, other);
                return;
            }

            var predator = initiator.Config.role == AnimalRole.Predator ? initiator : other;
            var prey = initiator.Config.role == AnimalRole.Prey ? initiator : other;
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