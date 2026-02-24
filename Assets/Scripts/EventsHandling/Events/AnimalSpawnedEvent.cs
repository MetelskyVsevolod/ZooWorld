using Animals.Core;

namespace EventsHandling.Events
{
    public readonly struct AnimalSpawnedEvent
    {
        public readonly Animal Animal;

        public AnimalSpawnedEvent(Animal animal)
        {
            Animal = animal;
        }
    }
}