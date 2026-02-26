using Animals.Core;
using Zenject;

namespace Systems.Collision
{
    public abstract class CollisionStrategyBase
    {
        public abstract bool TryResolve(Animal animalA, Animal animalB, SignalBus signalBus);
    }
}