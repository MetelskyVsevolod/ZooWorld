using Common;
using Events;
using Zenject;

namespace UI
{
    public class TastyLabelHandler
    {
        private readonly TastyLabelPool _pool;

        [Inject]
        public TastyLabelHandler(GameEventBus eventBus, TastyLabelPool pool)
        {
            _pool = pool;
            eventBus.Subscribe<AnimalDiedEvent>(OnAnimalDied);
            eventBus.Subscribe<AnimalAteEvent>(OnAnimalAte);
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