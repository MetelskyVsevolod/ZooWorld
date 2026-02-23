using Animals.Core;
using UnityEngine;
using Zenject;

namespace Systems
{
    public class BoundaryChecker : MonoBehaviour
    {
        [Inject] private AnimalRegistry _registry;
        [SerializeField] private float margin = 0.5f;

        private Bounds _worldBounds;

        private void Start()
        {
            var cam = Camera.main;
            var bottomLeft = ViewportToXZ(cam, Vector3.zero);
            var topRight = ViewportToXZ(cam, Vector3.one);

            _worldBounds = new Bounds(
                (bottomLeft + topRight) * 0.5f,
                new Vector3(Mathf.Abs(topRight.x - bottomLeft.x) + margin * 2f, 10f,
                    Mathf.Abs(topRight.z - bottomLeft.z) + margin * 2f)
            );
        }

        private void Update()
        {
            foreach (var animal in _registry.LiveAnimals)
            {
                if (!_worldBounds.Contains(animal.transform.position))
                {
                    Redirect(animal);
                }
            }
        }

        private void Redirect(Animal animal)
        {
            var toCenter = _worldBounds.center - animal.transform.position;
            toCenter.y = 0f;
            var newDirection = toCenter.normalized;

            animal.RigidbodyComponent.linearVelocity = newDirection * animal.RigidbodyComponent.linearVelocity.magnitude;
            animal.NotifyRedirected(newDirection);
        }

        private static Vector3 ViewportToXZ(Camera cam, Vector3 viewportPoint)
        {
            var ray = cam.ViewportPointToRay(viewportPoint);
            var t = -ray.origin.y / ray.direction.y;
            return ray.origin + ray.direction * t;
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (!Application.isPlaying)
            {
                return;
            }
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(_worldBounds.center, _worldBounds.size);
        }
#endif
    }
}