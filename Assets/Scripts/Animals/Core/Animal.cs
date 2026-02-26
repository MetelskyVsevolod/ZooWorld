using Signals;
using UnityEngine;
using Zenject;

namespace Animals.Core
{
    public class Animal : MonoBehaviour
    {
        [SerializeField] private Rigidbody rigidbodyComponent;
        [SerializeField] private Transform canvasTransformSlot;

        public AnimalConfig Config { get; private set; }
        public Transform CanvasTransformSlot => canvasTransformSlot;
        public Rigidbody RigidbodyComponent => rigidbodyComponent;
        public bool IsAlive { get; private set; }

        private MovementStrategyBase _movement;
        private SignalBus _signalBus;
        
        [Inject]
        private void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }
        
        private void Update()
        {
            if (!IsAlive)
            {
                return;
            }
            _movement?.Tick(this);
        }
        
        public void Initialize(AnimalConfig config)
        {
            if (config.MovementStrategy == null)
            {
                Debug.LogError($"[Animal] {nameof(config.MovementStrategy)} is null on config '{config.AnimalName}'!");
                return;
            }
            
            Config = config;
            IsAlive = true;
            
            _movement = config.MovementStrategy.Clone();
            _movement.OnInitialize(this);

            gameObject.name = config.AnimalName;
            _signalBus.Fire(new AnimalSpawnedSignal(this));
        }
        
        public void Die()
        {
            if (!IsAlive)
            {
                return;
            }

            IsAlive = false;
            _signalBus.Fire(new AnimalDiedSignal(this));
            gameObject.SetActive(false);
        }
        
        public void Revive()
        {
            IsAlive = true;
            rigidbodyComponent.linearVelocity = Vector3.zero;
            rigidbodyComponent.angularVelocity = Vector3.zero;
            _movement?.Reset();
            gameObject.SetActive(true);
        }
        
        public void NotifyRedirected(Vector3 newDirection)
        {
            _movement?.OnRedirected(newDirection);
        }
        
        private void OnCollisionEnter(Collision collision)
        {
            if (!IsAlive)
            {
                return;
            }

            var other = collision.gameObject.GetComponent<Animal>();
            
            if (other == null || !other.IsAlive)
            {
                return;
            }

            _signalBus.Fire(new AnimalCollisionSignal(this, other));
        }
    }
}