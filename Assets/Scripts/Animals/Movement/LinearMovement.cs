using System;
using Animals.Core;
using UnityEngine;

namespace Animals.Movement
{
    [Serializable]
    public class LinearMovement : MovementStrategyBase
    {
        [SerializeField] private float speed = 3f;
        [NonSerialized] private Vector3 _direction;

        public override void OnInitialize(Animal animal)
        {
            PickRandomDirection();
        }

        public override void Tick(Animal animal)
        {
            animal.RigidbodyComponent.linearVelocity = _direction * speed;
        }

        public override void Reset()
        {
            PickRandomDirection();
        }

        public override void OnRedirected(Vector3 newDirection)
        {
            _direction = newDirection;
        }

        private void PickRandomDirection()
        {
            var raw = UnityEngine.Random.insideUnitCircle.normalized;
            _direction = new Vector3(raw.x, 0f, raw.y);
        }
    }
}