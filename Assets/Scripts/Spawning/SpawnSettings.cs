using System.Collections.Generic;
using Animals.Core;
using UnityEngine;

namespace Spawning
{
    [CreateAssetMenu(menuName = "ZooWorld/Spawn Settings", fileName = "SpawnSettings")]
    public class SpawnSettings : ScriptableObject
    {
        [SerializeField] private float minSpawnInterval = 1f;
        [SerializeField] private float maxSpawnInterval = 2f;
        [SerializeField] private Vector2 spawnAreaHalfExtents = new(8f, 5f);
        [SerializeField] private AnimalConfig[] animalConfigs;

        public float MinSpawnInterval => minSpawnInterval;
        public float MaxSpawnInterval => maxSpawnInterval;
        public Vector2 SpawnAreaHalfExtents => spawnAreaHalfExtents;
        public IReadOnlyList<AnimalConfig> AnimalConfigs => animalConfigs;
    }
}