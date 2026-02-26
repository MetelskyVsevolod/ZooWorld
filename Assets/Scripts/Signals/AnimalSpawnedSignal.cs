using Animals.Core;

namespace Signals
{
    public readonly struct AnimalSpawnedSignal
    {
        public readonly Animal Animal;

        public AnimalSpawnedSignal(Animal animal)
        {
            Animal = animal;
        }
    }
}