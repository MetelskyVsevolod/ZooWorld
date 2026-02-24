using System;
using Animals.Core;
using UnityEngine;

namespace Animals.Movement
{
    [Serializable]
    public class JumpMovement : MovementStrategyBase
    {
        [SerializeField] private float jumpForce = 5f;
        [SerializeField] private float jumpInterval = 1.5f;

        [NonSerialized] private float _timer;

        public override void OnInitialize(Animal animal) => _timer = jumpInterval;

        public override void Tick(Animal animal)
        {
            _timer += Time.deltaTime;
            if (_timer < jumpInterval)
            {
                return;
            }

            _timer = 0f;
            var dir = UnityEngine.Random.insideUnitCircle.normalized;
            animal.RigidbodyComponent.AddForce(new Vector3(dir.x, 1f, dir.y) * jumpForce, ForceMode.Impulse);
        }

        public override void Reset()
        {
            _timer = jumpInterval;
        }
    }
}