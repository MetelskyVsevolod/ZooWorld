using UnityEngine;

namespace Extensions
{
    public static class ComponentExtensions
    {
        public static T GetOrAddComponent<T>(this GameObject go) where T : Component
        {
            return go.TryGetComponent(out T component) ? component : go.AddComponent<T>();
        }
    }
}