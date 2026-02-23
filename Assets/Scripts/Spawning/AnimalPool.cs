using System.Collections.Generic;
using Animals.Core;
using Events;
using UnityEngine;
using UnityEngine.Pool;
using Zenject;

namespace Spawning
{
    public class AnimalPool
    {
        private readonly DiContainer _container;
        private readonly GameEventBus _eventBus;
        private readonly Transform _poolRoot;

        private readonly Dictionary<AnimalConfig, ObjectPool<Animal>> _pools = new();

        [Inject]
        public AnimalPool(
            DiContainer container,
            GameEventBus eventBus,
            [Inject(Id = "PoolRoot")] Transform poolRoot)
        {
            _container = container;
            _eventBus = eventBus;
            _poolRoot = poolRoot;

            _eventBus.Subscribe<AnimalDiedEvent>(OnAnimalDied);
        }
        
        public Animal Get(AnimalConfig config)
        {
            return GetOrCreatePool(config).Get();
        }
        
        public void Release(Animal animal)
        {
            if (!_pools.TryGetValue(animal.Config, out var pool)) 
            {
                return;
            }
            pool.Release(animal);
        }
        
        private ObjectPool<Animal> GetOrCreatePool(AnimalConfig config)
        {
            if (_pools.TryGetValue(config, out var existing))
            {
                return existing;
            }

            var pool = new ObjectPool<Animal>(
                createFunc: () => CreateAnimal(config),
                actionOnGet: animal => animal.Revive(),
                actionOnRelease: animal => { animal.gameObject.SetActive(false); animal.transform.SetParent(_poolRoot); },
                actionOnDestroy: animal => Object.Destroy(animal.gameObject),
                collectionCheck: false,
                defaultCapacity: 10,
                maxSize: 100
            );

            _pools[config] = pool;
            return pool;
        }

        private Animal CreateAnimal(AnimalConfig config)
        {
            var spawnedAnimal = _container.InstantiatePrefab(config.prefab, _poolRoot);
            var animal = spawnedAnimal.GetComponent<Animal>();

            if (animal == null)
            {
                animal = spawnedAnimal.AddComponent<Animal>();
            }

            return animal;
        }
        
        private void OnAnimalDied(AnimalDiedEvent evt)
        {
            Release(evt.Animal);
        }
    }
}