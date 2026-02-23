using UnityEngine;

namespace Animals.Core
{
    [CreateAssetMenu(menuName = "ZooWorld/Animal Config", fileName = "NewAnimalConfig")]
    public class AnimalConfig : ScriptableObject
    {
        [Header("Identity")]
        public string animalName;
        public AnimalRole role;

        [Header("Visuals")]
        public GameObject prefab;
        
        [Header("Movement")]
        [Tooltip("Pick a movement type from the dropdown. Its parameters appear inline below.")]
        [SerializeReference]
        public MovementStrategyBase movementStrategy;
    }
}