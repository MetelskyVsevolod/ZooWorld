using UnityEngine;

namespace Animals.Core
{
    [CreateAssetMenu(menuName = "ZooWorld/Animal Config", fileName = "NewAnimalConfig")]
    public class AnimalConfig : ScriptableObject
    {
        [SerializeField] private string animalName;
        [SerializeField] private AnimalRole role;
        [SerializeField] private GameObject prefab;
        [SerializeReference] private MovementStrategyBase movementStrategy;

        public string AnimalName => animalName;
        public AnimalRole Role => role;
        public GameObject Prefab => prefab;
        public MovementStrategyBase MovementStrategy => movementStrategy;
    }
}