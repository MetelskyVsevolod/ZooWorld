using Animals.Core;
using UnityEngine;
using Zenject;

namespace Spawning
{
    public class AnimalFactory
    {
        private readonly AnimalPool _pool;

        [Inject]
        public AnimalFactory(AnimalPool pool)
        {
            _pool = pool;
        }

        public Animal Create(AnimalConfig config, Vector3 worldPosition)
        {
            var animal = _pool.Get(config);
            animal.transform.position = worldPosition;
            animal.Initialize(config);
            return animal;
        }
    }
}