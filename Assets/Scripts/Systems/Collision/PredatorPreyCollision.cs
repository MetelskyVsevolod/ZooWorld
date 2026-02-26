using Animals.Core;
using EventsHandling;
using EventsHandling.Events;

namespace Systems.Collision
{
    public class PredatorPreyCollision : CollisionStrategyBase
    {
        public override bool TryResolve(Animal animalA, Animal animalB, GameEventBus eventBus)
        {
            Animal predator;
            Animal prey;

            if (animalA.Config.Role == AnimalRole.Predator && animalB.Config.Role == AnimalRole.Prey)
            {
                predator = animalA;
                prey = animalB;
            }
            else if (animalA.Config.Role == AnimalRole.Prey && animalB.Config.Role == AnimalRole.Predator)
            {
                predator = animalB;
                prey = animalA;
            }
            else
            {
                return false;
            }

            prey.Die();
            eventBus.Publish(new AnimalAteEvent(predator));
            return true;
        }
    }
}