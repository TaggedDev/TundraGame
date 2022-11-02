using Creatures.Player.Behaviour;
using Creatures.Player.Inventory.ItemConfiguration;
using UnityEngine;

namespace Creatures.Player
{
    public class TemporaryItemsController : MonoBehaviour
    {
        [SerializeField] private FoodItemConfiguration meat;
        [SerializeField] private PlayerBehaviour player;
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.O))
            {
                meat.SpawnItem(player.transform.position + Vector3.up * 1.5f);
            }
        }
    }
}