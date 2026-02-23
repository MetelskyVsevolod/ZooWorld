using System.Collections.Generic;
using UnityEngine;

namespace Extensions
{
    public static class CollectionExtensions
    {
        public static T GetRandom<T>(this IReadOnlyList<T> source)
        {
            if (source == null || source.Count == 0)
            {
                Debug.LogError($"The {nameof(source)} collection is null or empty!");
                return default;
            }

            return source[Random.Range(0, source.Count)];
        }
    }
}