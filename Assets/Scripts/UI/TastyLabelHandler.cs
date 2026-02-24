using System;
using Common;
using Events;
using Zenject;

namespace UI
{
    public class TastyLabelHandler : IDisposable
    {
        private readonly TastyLabelPool _pool;
        private readonly GameEventBus _eventBus;

        [Inject]
        public TastyLabelHandler(GameEventBus eventBus, TastyLabelPool pool)
        {
            _pool = pool;
            _eventBus = eventBus;
            eventBus.Subscribe<AnimalDiedEvent>(OnAnimalDied);
            eventBus.Subscribe<AnimalAteEvent>(OnAnimalAte);
        }
        
        public void Dispose()
        {
            _eventBus.Unsubscribe<AnimalDiedEvent>(OnAnimalDied);
            _eventBus.Unsubscribe<AnimalAteEvent>(OnAnimalAte);
        }

        private void OnAnimalAte(AnimalAteEvent evt)
        {
            _pool.ShowOn(evt.Predator.CanvasTransformSlot, Constants.OnAnimalAteLabelText);
        }

        private void OnAnimalDied(AnimalDiedEvent evt)
        {
            var tastyLabelView = evt.Animal.GetComponentInChildren<TastyLabelView>();
            
            if (tastyLabelView)
            {
                tastyLabelView.Hide();
            }
        }
    }
}