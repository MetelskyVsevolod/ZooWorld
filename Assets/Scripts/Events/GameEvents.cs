using Animals.Core;

namespace Events
{
    public readonly struct AnimalSpawnedEvent
    {
        public readonly Animal Animal;
        public AnimalSpawnedEvent(Animal animal) => Animal = animal;
    }

    public readonly struct AnimalDiedEvent
    {
        public readonly Animal Animal;
        public AnimalDiedEvent(Animal animal) => Animal = animal;
    }

    public readonly struct AnimalCollisionEvent
    {
        public readonly Animal Initiator;
        public readonly Animal Other;
        public AnimalCollisionEvent(Animal initiator, Animal other)
        {
            Initiator = initiator;
            Other = other;
        }
    }

    public readonly struct AnimalAteEvent
    {
        public readonly Animal Predator;
        public AnimalAteEvent(Animal predator) => Predator = predator;
    }
}