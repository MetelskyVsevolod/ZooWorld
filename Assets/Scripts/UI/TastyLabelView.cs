using System.Collections;
using TMPro;
using UnityEngine;

namespace UI
{
    public class TastyLabelView : MonoBehaviour
    {
        public event System.Action<TastyLabelView> OnHidden;
        
        [SerializeField] private TextMeshProUGUI labelText;
        [SerializeField] private float displayDuration = 1.5f;

        private Coroutine _hideCoroutine;

        public void SetText(string text)
        {
            labelText.text = text;
        }
        
        public void Show(Transform parent)
        {
            if (_hideCoroutine != null)
            {
                StopCoroutine(_hideCoroutine);
            }

            transform.SetParent(parent, worldPositionStays: false);
            transform.localPosition = Vector3.zero;
            gameObject.SetActive(true);
            _hideCoroutine = StartCoroutine(HideAfterDelay());
        }

        public void Hide()
        {
            if (_hideCoroutine != null)
            {
                StopCoroutine(_hideCoroutine);
                _hideCoroutine = null;
            }

            ReturnToPool();
        }

        private IEnumerator HideAfterDelay()
        {
            yield return new WaitForSeconds(displayDuration);
            ReturnToPool();
        }

        private void ReturnToPool()
        {
            transform.SetParent(null);
            gameObject.SetActive(false);
            _hideCoroutine = null;
            OnHidden?.Invoke(this);
        }
    }
}