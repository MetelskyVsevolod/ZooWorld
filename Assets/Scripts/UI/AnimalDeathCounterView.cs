using Animals.Core;
using Events;
using TMPro;
using UnityEngine;
using Zenject;

namespace UI
{
    public class AnimalDeathCounterView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI label;
        [SerializeField] private AnimalRole trackedRole;

        private GameEventBus _eventBus;
        private int _count;

        [Inject]
        public void Construct(GameEventBus eventBus)
        {
            _eventBus = eventBus;
            _eventBus.Subscribe<AnimalDiedEvent>(OnAnimalDied);
        }

        private void Start()
        {
            Refresh();
        }

        private void OnDestroy()
        {
            _eventBus.Unsubscribe<AnimalDiedEvent>(OnAnimalDied);
        }
        
        private void OnAnimalDied(AnimalDiedEvent evt)
        {
            if (evt.Animal.Config.role != trackedRole)
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