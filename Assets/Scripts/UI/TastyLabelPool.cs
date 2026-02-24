using Common;
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
        public TastyLabelPool(TastyLabelView prefab, [Inject(Id = Constants.PoolRootTransformId)] Transform poolRoot)
        {
            _prefab = prefab;
            _poolRoot = poolRoot;

            _pool = new ObjectPool<TastyLabelView>(
                createFunc: CreateView,
                actionOnGet: OnGetView,
                actionOnRelease: OnReleaseView,
                actionOnDestroy: OnDestroyView,
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

        private TastyLabelView CreateView()
        {
            var view = Object.Instantiate(_prefab, _poolRoot);
            view.gameObject.SetActive(false);
            view.name = nameof(TastyLabelView);
            return view;
        }

        private void OnGetView(TastyLabelView view)
        {
            view.OnHidden += OnViewHidden;
        }

        private void OnReleaseView(TastyLabelView view)
        {
            view.OnHidden -= OnViewHidden;
            view.transform.SetParent(_poolRoot);
        }

        private void OnDestroyView(TastyLabelView view)
        {
            Object.Destroy(view.gameObject);
        }
    }
}