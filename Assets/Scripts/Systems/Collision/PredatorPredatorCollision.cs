using Animals.Core;
using EventsHandling;
using EventsHandling.Events;
using Random = UnityEngine.Random;

namespace Systems.Collision
{
    public class PredatorPredatorCollision : CollisionStrategyBase
    {
        public override bool TryResolve(Animal animalA, Animal animalB, GameEventBus eventBus)
        {
            if (animalA.Config.Role != AnimalRole.Predator || animalB.Config.Role != AnimalRole.Predator)
            {
                return false;
            }

            var loser = Random.value < 0.5f ? animalA : animalB;
            var winner = loser == animalA ? animalB : animalA;
            loser.Die();
            eventBus.Publish(new AnimalAteEvent(winner));
            return true;
        }
    }
}