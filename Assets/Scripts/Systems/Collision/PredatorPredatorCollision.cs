using Animals.Core;
using Signals;
using Zenject;
using Random = UnityEngine.Random;

namespace Systems.Collision
{
    public class PredatorPredatorCollision : CollisionStrategyBase
    {
        public override bool TryResolve(Animal animalA, Animal animalB, SignalBus signalBus)
        {
            if (animalA.Config.Role != AnimalRole.Predator || animalB.Config.Role != AnimalRole.Predator)
            {
                return false;
            }

            var loser = Random.value < 0.5f ? animalA : animalB;
            var winner = loser == animalA ? animalB : animalA;
            loser.Die();
            signalBus.Fire(new AnimalAteSignal(winner));
            return true;
        }
    }
}