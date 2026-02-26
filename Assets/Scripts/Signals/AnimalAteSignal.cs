using Animals.Core;

namespace Signals
{
    public readonly struct AnimalAteSignal
    {
        public readonly Animal Predator;

        public AnimalAteSignal(Animal predator)
        {
            Predator = predator;
        }
    }
}