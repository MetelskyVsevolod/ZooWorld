using System;
using System.Collections.Generic;
using Animals.Core;
using Signals;
using Zenject;

namespace Systems
{
    public class AnimalRegistry : IDisposable
    {
        private readonly HashSet<Animal> _liveAnimals = new();
        private readonly SignalBus _signalBus;

        public IReadOnlyCollection<Animal> LiveAnimals => _liveAnimals;

        [Inject]
        public AnimalRegistry(SignalBus signalBus)
        {
            _signalBus = signalBus;
            _signalBus.Subscribe<AnimalSpawnedSignal>(OnSpawned);
            _signalBus.Subscribe<AnimalDiedSignal>(OnDied);
        }
        
        public void Dispose()
        {
            _signalBus.Unsubscribe<AnimalSpawnedSignal>(OnSpawned);
            _signalBus.Unsubscribe<AnimalDiedSignal>(OnDied);
        }

        private void OnSpawned(AnimalSpawnedSignal signal)
        {
            _liveAnimals.Add(signal.Animal);
        }

        private void OnDied(AnimalDiedSignal signal)
        {
            _liveAnimals.Remove(signal.Animal);
        }
    }
}