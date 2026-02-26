using Animals.Core;

namespace Signals
{
    public readonly struct AnimalDiedSignal
    {
        public readonly Animal Animal;

        public AnimalDiedSignal(Animal animal)
        {
            Animal = animal;
        }
    }
}