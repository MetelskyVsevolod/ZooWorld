using System.Threading;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace UI
{
    public class TastyLabelView : MonoBehaviour
    {
        public event System.Action<TastyLabelView> OnHidden;

        [SerializeField] private TextMeshProUGUI labelText;
        [SerializeField] private float displayDuration = 1.5f;

        private CancellationTokenSource _cts;

        public void SetText(string text)
        {
            labelText.text = text;
        }

        public void Show(Transform parent)
        {
            CancelCurrentTimer();

            transform.SetParent(parent, worldPositionStays: false);
            transform.localPosition = Vector3.zero;
            gameObject.SetActive(true);

            _cts = new CancellationTokenSource();
            HideAfterDelay(_cts.Token).Forget();
        }

        public void Hide()
        {
            CancelCurrentTimer();
            ReturnToPool();
        }

        private async UniTaskVoid HideAfterDelay(CancellationToken token)
        {
            await UniTask.Delay(System.TimeSpan.FromSeconds(displayDuration), cancellationToken: token);
            ReturnToPool();
        }

        private void ReturnToPool()
        {
            transform.SetParent(null);
            gameObject.SetActive(false);
            OnHidden?.Invoke(this);
        }

        private void CancelCurrentTimer()
        {
            _cts?.Cancel();
            _cts?.Dispose();
            _cts = null;
        }
    }
}