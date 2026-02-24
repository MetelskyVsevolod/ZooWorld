using System.Collections;
using Animals.Core;
using Extensions;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Spawning
{
    public class AnimalSpawner : MonoBehaviour
    {
        private AnimalFactory _factory;
        private SpawnSettings _settings;
        private Coroutine _spawnLoopCoroutine;
        
        [Inject]
        public void Construct(AnimalFactory factory, SpawnSettings settings)
        {
            _factory = factory;
            _settings = settings;
        }
        
        private void Start()
        {
            _spawnLoopCoroutine = StartCoroutine(SpawnLoop());
        }

        private IEnumerator SpawnLoop()
        {
            while (true)
            {
                var delay = Random.Range(_settings.MinSpawnInterval, _settings.MaxSpawnInterval);
                yield return new WaitForSeconds(delay);
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
        
        private void OnDestroy()
        {
            if (_spawnLoopCoroutine != null)
            {
                StopCoroutine(_spawnLoopCoroutine);
            }
        }
    }
}