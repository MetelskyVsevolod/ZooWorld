using Animals.Core;
using EventsHandling;

namespace Systems.Collision
{
    public abstract class CollisionStrategyBase
    {
        public abstract bool TryResolve(Animal animalA, Animal animalB, GameEventBus eventBus);
    }
}