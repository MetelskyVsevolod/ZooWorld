using UnityEditor;

namespace Animals.Core.Editor
{
    [CustomPropertyDrawer(typeof(MovementStrategyBase), true)]
    public class MovementStrategyPickerDrawer : PolymorphicPickerDrawer<MovementStrategyBase> { }
}