using System;
using Common;
using Signals;
using Zenject;

namespace UI
{
    public class TastyLabelHandler : IDisposable
    {
        private readonly TastyLabelPool _pool;
        private readonly SignalBus _signalBus;

        [Inject]
        public TastyLabelHandler(SignalBus signalBus, TastyLabelPool pool)
        {
            _pool = pool;
            _signalBus = signalBus;
            _signalBus.Subscribe<AnimalDiedSignal>(OnAnimalDied);
            _signalBus.Subscribe<AnimalAteSignal>(OnAnimalAte);
        }
        
        public void Dispose()
        {
            _signalBus.Unsubscribe<AnimalDiedSignal>(OnAnimalDied);
            _signalBus.Unsubscribe<AnimalAteSignal>(OnAnimalAte);
        }

        private void OnAnimalAte(AnimalAteSignal signal)
        {
            _pool.ShowOn(signal.Predator.CanvasTransformSlot, Constants.OnAnimalAteLabelText);
        }

        private void OnAnimalDied(AnimalDiedSignal signal)
        {
            var tastyLabelView = signal.Animal.GetComponentInChildren<TastyLabelView>();
            
            if (tastyLabelView)
            {
                tastyLabelView.Hide();
            }
        }
    }
}