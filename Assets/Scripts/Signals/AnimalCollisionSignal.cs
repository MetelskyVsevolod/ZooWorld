using Animals.Core;

namespace Signals
{
    public readonly struct AnimalCollisionSignal
    {
        public readonly Animal Initiator;
        public readonly Animal Other;
        
        public AnimalCollisionSignal(Animal initiator, Animal other)
        {
            Initiator = initiator;
            Other = other;
        }
    }
}