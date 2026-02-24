using UnityEngine;
using UnityEngine.Pool;
using Zenject;

namespace UI
{
    public class TastyLabelPool
    {
        private readonly ObjectPool<TastyLabelView> _pool;
        private readonly TastyLabelView _prefab;
        private readonly Transform _poolRoot;

        [Inject]
        public TastyLabelPool(TastyLabelView prefab, [Inject(Id = "PoolRoot")] Transform poolRoot)
        {
            _prefab = prefab;
            _poolRoot = poolRoot;

            _pool = new ObjectPool<TastyLabelView>(
                createFunc: Create,
                actionOnGet: view => view.OnHidden += OnViewHidden,
                actionOnRelease: view =>
                {
                    view.OnHidden -= OnViewHidden;
                    view.transform.SetParent(_poolRoot);
                },
                actionOnDestroy: view => Object.Destroy(view.gameObject),
                collectionCheck: false,
                defaultCapacity: 4
            );
        }

        public void ShowOn(Transform parent, string text)
        {
            var view = _pool.Get();
            view.Show(parent);
            view.SetText(text);
        }

        private void OnViewHidden(TastyLabelView view)
        {
            _pool.Release(view);
        }

        private TastyLabelView Create()
        {
            var spawnedTastyLabelView = Object.Instantiate(_prefab, _poolRoot);
            spawnedTastyLabelView.gameObject.SetActive(false);
            return spawnedTastyLabelView;
        }
    }
}