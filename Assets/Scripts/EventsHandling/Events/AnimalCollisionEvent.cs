using Animals.Core;

namespace EventsHandling.Events
{
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
}