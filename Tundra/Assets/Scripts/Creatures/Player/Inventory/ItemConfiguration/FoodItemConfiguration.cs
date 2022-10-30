using UnityEngine;

namespace Creatures.Player.Inventory.ItemConfiguration
{
    [CreateAssetMenu(fileName = "New Placeable Configuration", menuName = "Items/Food Configuration")]
    public class FoodItemConfiguration : BasicItemConfiguration
    {
        /// <summary>
        /// Spawns
        /// </summary>
        /// <param name="startPos"></param>
        public void SpawnItem(Vector3 startPos)
        {
            var item = Instantiate(ItemInWorldPrefab, startPos, Quaternion.identity);
        }
    }
}