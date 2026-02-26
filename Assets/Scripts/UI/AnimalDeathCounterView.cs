using Animals.Core;
using Signals;
using TMPro;
using UnityEngine;
using Zenject;

namespace UI
{
    public class AnimalDeathCounterView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI label;
        [SerializeField] private AnimalRole trackedRole;

        private SignalBus _signalBus;
        private int _count;

        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
            _signalBus.Subscribe<AnimalDiedSignal>(OnAnimalDied);
        }

        private void Start()
        {
            Refresh();
        }

        private void OnDestroy()
        {
            _signalBus.Unsubscribe<AnimalDiedSignal>(OnAnimalDied);
        }
        
        private void OnAnimalDied(AnimalDiedSignal evt)
        {
            if (evt.Animal.Config.Role != trackedRole)
            {
                return;
            }

            _count++;
            Refresh();
        }

        private void Refresh()
        {
            label.text = $"{trackedRole} deaths: {_count}";
        }
    }
}