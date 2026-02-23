using System.Collections.Generic;
using Animals.Core;
using Events;
using Zenject;

namespace Systems
{
    public class AnimalRegistry
    {
        private readonly HashSet<Animal> _liveAnimals = new();

        public IReadOnlyCollection<Animal> LiveAnimals => _liveAnimals;

        [Inject]
        public AnimalRegistry(GameEventBus eventBus)
        {
            eventBus.Subscribe<AnimalSpawnedEvent>(OnSpawned);
            eventBus.Subscribe<AnimalDiedEvent>(OnDied);
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