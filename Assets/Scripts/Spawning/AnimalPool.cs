using System;
using System.Collections.Generic;
using Animals.Core;
using EventsHandling;
using EventsHandling.Events;
using Extensions;
using UnityEngine;
using UnityEngine.Pool;
using Zenject;
using Object = UnityEngine.Object;

namespace Spawning
{
    public class AnimalPool : IDisposable
    {
        private const int DefaultCapacity = 10;
        private const int MaxSize = 100;
        
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
        
        public void Dispose()
        {
            _eventBus.Unsubscribe<AnimalDiedEvent>(OnAnimalDied);
        }
        
        private void Release(Animal animal)
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
                actionOnGet: OnGetAnimal,
                actionOnRelease: OnReleaseAnimal,
                actionOnDestroy: OnDestroyAnimal,
                collectionCheck: false,
                defaultCapacity: DefaultCapacity,
                maxSize: MaxSize
            );

            _pools[config] = pool;
            return pool;
        }

        private Animal CreateAnimal(AnimalConfig config)
        {
            var spawnedAnimal = _container.InstantiatePrefab(config.Prefab, _poolRoot);
            return spawnedAnimal.GetOrAddComponent<Animal>();
        }

        private void OnGetAnimal(Animal animal)
        {
            animal.Revive();
        }

        private void OnReleaseAnimal(Animal animal)
        {
            animal.gameObject.SetActive(false);
            animal.transform.SetParent(_poolRoot);
        }

        private void OnDestroyAnimal(Animal animal)
        {
            Object.Destroy(animal.gameObject);
        }

        private void OnAnimalDied(AnimalDiedEvent evt)
        {
            Release(evt.Animal);
        }
    }
}