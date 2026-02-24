using Animals.Core;

namespace EventsHandling.Events
{
    public readonly struct AnimalAteEvent
    {
        public readonly Animal Predator;

        public AnimalAteEvent(Animal predator)
        {
            Predator = predator;
        }
    }
}