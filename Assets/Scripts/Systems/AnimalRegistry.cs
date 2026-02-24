using System;
using System.Collections.Generic;
using Animals.Core;
using Events;
using Zenject;

namespace Systems
{
    public class AnimalRegistry : IDisposable
    {
        private readonly HashSet<Animal> _liveAnimals = new();
        private readonly GameEventBus _eventBus;

        public IReadOnlyCollection<Animal> LiveAnimals => _liveAnimals;

        [Inject]
        public AnimalRegistry(GameEventBus eventBus)
        {
            _eventBus = eventBus;
            _eventBus.Subscribe<AnimalSpawnedEvent>(OnSpawned);
            _eventBus.Subscribe<AnimalDiedEvent>(OnDied);
        }
        
        public void Dispose()
        {
            _eventBus.Unsubscribe<AnimalSpawnedEvent>(OnSpawned);
            _eventBus.Unsubscribe<AnimalDiedEvent>(OnDied);
        }

        private void OnSpawned(AnimalSpawnedEvent evt)
        {
            _liveAnimals.Add(evt.Animal);
        }

        private void OnDied(AnimalDiedEvent evt)
        {
            _liveAnimals.Remove(evt.Animal);
        }
    }
}