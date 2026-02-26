using Animals.Core;
using Signals;
using Zenject;

namespace Systems.Collision
{
    public class PredatorPreyCollision : CollisionStrategyBase
    {
        public override bool TryResolve(Animal animalA, Animal animalB, SignalBus eventBus)
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
            eventBus.Fire(new AnimalAteSignal(predator));
            return true;
        }
    }
}