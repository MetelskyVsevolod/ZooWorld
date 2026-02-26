using Signals;
using Spawning;
using Systems;
using Systems.Collision;
using UI;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private SpawnSettings spawnSettings;
        [SerializeField] private Transform poolRoot;
        [SerializeField] private BoundaryChecker boundaryChecker;
        [SerializeField] private TastyLabelView tastyLabelPrefab;

        public override void InstallBindings()
        {
            BindSignals();
            BindPooling();
            BindSpawning();
            BindSystems();
            BindCollisionStrategies();
            BindUI();
        }

        private void BindSignals()
        {
            SignalBusInstaller.Install(Container);
    
            Container.DeclareSignal<AnimalSpawnedSignal>();
            Container.DeclareSignal<AnimalDiedSignal>();
            Container.DeclareSignal<AnimalCollisionSignal>();
            Container.DeclareSignal<AnimalAteSignal>();
        }

        private void BindPooling()
        {
            Container.Bind<AnimalPool>().AsSingle().WithArguments(poolRoot);
        }

        private void BindSpawning()
        {
            Container.Bind<SpawnSettings>().FromInstance(spawnSettings).AsSingle();
            Container.Bind<AnimalFactory>().AsSingle();
            Container.BindInterfacesTo<AnimalSpawner>().AsSingle();
        }

        private void BindSystems()
        {
            Container.Bind<AnimalRegistry>().AsSingle().NonLazy();
            Container.Bind<BoundaryChecker>().FromInstance(boundaryChecker).AsSingle().NonLazy();
            Container.Bind<CollisionResolver>().AsSingle().NonLazy();
        }

        private void BindCollisionStrategies()
        {
            Container.Bind<CollisionStrategyBase>().To<PredatorPreyCollision>().AsSingle();
            Container.Bind<CollisionStrategyBase>().To<PredatorPredatorCollision>().AsSingle();
        }

        private void BindUI()
        {
            Container.Bind<TastyLabelView>().FromInstance(tastyLabelPrefab).AsSingle();
            Container.Bind<TastyLabelPool>().AsSingle().WithArguments(poolRoot);
            Container.Bind<TastyLabelHandler>().AsSingle().NonLazy();
        }
    }
}