using Animals.Core;

namespace EventsHandling.Events
{
    public readonly struct AnimalDiedEvent
    {
        public readonly Animal Animal;

        public AnimalDiedEvent(Animal animal)
        {
            Animal = animal;
        }
    }
}