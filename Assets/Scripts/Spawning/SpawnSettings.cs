using Animals.Core;
using UnityEngine;

namespace Spawning
{
    [CreateAssetMenu(menuName = "ZooWorld/Spawn Settings", fileName = "SpawnSettings")]
    public class SpawnSettings : ScriptableObject
    {
        public float minSpawnInterval = 1f;
        public float maxSpawnInterval = 2f;
        public AnimalConfig[] animalConfigs;
        public Vector2 spawnAreaHalfExtents = new(8f, 5f);
    }
}