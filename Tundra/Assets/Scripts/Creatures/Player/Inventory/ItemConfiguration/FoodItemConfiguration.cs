using UnityEngine;

namespace Creatures.Player.Inventory.ItemConfiguration
{
    [CreateAssetMenu(fileName = "New Placeable Configuration", menuName = "Items/Food Configuration")]
    public class FoodItemConfiguration : BasicItemConfiguration
    {
        [SerializeField] private float calories;

        public float Calories => calories;

        /// <summary>
        /// Spawns item on position
        /// </summary>
        /// <param name="startPos">Position</param>
        public void SpawnItem(Vector3 startPos)
        {
            var item = Instantiate(ItemInWorldPrefab, startPos, Quaternion.identity);
        }
    }
}