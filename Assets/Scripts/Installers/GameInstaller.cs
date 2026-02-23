using Events;
using Spawning;
using Systems;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private SpawnSettings spawnSettings;
        [SerializeField] private Transform poolRoot;

        public override void InstallBindings()
        {
            Container.Bind<GameEventBus>()
                .AsSingle();

            Container.Bind<Transform>()
                .WithId("PoolRoot")
                .FromInstance(poolRoot)
                .AsCached();

            Container.Bind<AnimalPool>()
                .AsSingle();

            Container.Bind<AnimalFactory>()
                .AsSingle();

            Container.Bind<SpawnSettings>()
                .FromInstance(spawnSettings)
                .AsSingle();

            Container.Bind<AnimalSpawner>()
                .FromComponentInHierarchy()
                .AsSingle()
                .NonLazy();
            
            Container.Bind<AnimalRegistry>()
                .AsSingle();

            Container.Bind<BoundaryChecker>()
                .FromComponentInHierarchy()
                .AsSingle()
                .NonLazy();
        }
    }
}