using Common;
using EventsHandling;
using Spawning;
using Systems;
using UI;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private SpawnSettings spawnSettings;
        [SerializeField] private Transform poolRoot;
        [SerializeField] private AnimalSpawner animalSpawner;
        [SerializeField] private BoundaryChecker boundaryChecker;
        [SerializeField] private TastyLabelView tastyLabelPrefab;

        public override void InstallBindings()
        {
            BindCore();
            BindPooling();
            BindSpawning();
            BindSystems();
            BindUI();
        }

        private void BindCore()
        {
            Container.Bind<GameEventBus>().AsSingle();
        }

        private void BindPooling()
        {
            Container.Bind<Transform>().WithId(Constants.PoolRootTransformId).FromInstance(poolRoot).AsCached();
            Container.Bind<AnimalPool>().AsSingle();
        }

        private void BindSpawning()
        {
            Container.Bind<SpawnSettings>().FromInstance(spawnSettings).AsSingle();
            Container.Bind<AnimalFactory>().AsSingle();
            Container.Bind<AnimalSpawner>().FromInstance(animalSpawner).AsSingle();
        }

        private void BindSystems()
        {
            Container.Bind<AnimalRegistry>().AsSingle().NonLazy();
            Container.Bind<BoundaryChecker>().FromInstance(boundaryChecker).AsSingle();
            Container.Bind<CollisionResolver>().AsSingle().NonLazy();
        }

        private void BindUI()
        {
            Container.Bind<TastyLabelView>().FromInstance(tastyLabelPrefab).AsSingle();
            Container.Bind<TastyLabelPool>().AsSingle();
            Container.Bind<TastyLabelHandler>().AsSingle().NonLazy();
        }
    }
}