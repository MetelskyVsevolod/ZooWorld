using System;
using System.Threading;
using Animals.Core;
using Cysharp.Threading.Tasks;
using Extensions;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Spawning
{
    public class AnimalSpawner : IInitializable, IDisposable
    {
        private readonly AnimalFactory _factory;
        private readonly SpawnSettings _settings;
        private CancellationTokenSource _cts;

        [Inject]
        public AnimalSpawner(AnimalFactory factory, SpawnSettings settings)
        {
            _factory = factory;
            _settings = settings;
        }

        public void Initialize()
        {
            _cts = new CancellationTokenSource();
            SpawnLoop(_cts.Token).Forget();
        }

        public void Dispose()
        {
            _cts?.Cancel();
            _cts?.Dispose();
        }

        private async UniTaskVoid SpawnLoop(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                var delay = Random.Range(_settings.MinSpawnInterval, _settings.MaxSpawnInterval);
                await UniTask.Delay(TimeSpan.FromSeconds(delay), cancellationToken: token);
                SpawnOne();
            }
        }

        private void SpawnOne()
        {
            var config = PickConfig();
            var position = GetRandomSpawnPosition();
            _factory.Create(config, position);
        }
        
        private AnimalConfig PickConfig()
        {
            return _settings.AnimalConfigs.GetRandom();
        }

        private Vector3 GetRandomSpawnPosition()
        {
            var h = _settings.SpawnAreaHalfExtents;
            return new Vector3(
                Random.Range(-h.x, h.x),
                0f,
                Random.Range(-h.y, h.y)
            );
        }
    }
}