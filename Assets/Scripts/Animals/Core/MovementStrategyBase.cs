using System;
using UnityEngine;

namespace Animals.Core
{
    [Serializable]
    public abstract class MovementStrategyBase
    {
        public abstract void OnInitialize(Animal animal);

        public abstract void Tick(Animal animal);

        public abstract void Reset();

        public virtual void OnRedirected(Vector3 newDirection) { }

        public MovementStrategyBase Clone() => (MovementStrategyBase)MemberwiseClone();
    }
}