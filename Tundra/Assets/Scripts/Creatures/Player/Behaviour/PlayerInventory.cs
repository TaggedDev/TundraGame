using Creatures.Player.Inventory;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace Creatures.Player.Behaviour
{
    /// <summary>
    /// Class that handles player's inventory logic.
    /// </summary
    [RequireComponent(typeof(SphereCollider))]
    [RequireComponent(typeof(PlayerBehaviour))]
    public class PlayerInventory : MonoBehaviour
    {
        private InventoryContainer inventory;

        public static float ItemPickingUpTime => 3f;

        private PlayerBehaviour _playerBehaviour;
        private int _lastSlotIndex;

        /// <summary>
        /// Player inventory contatiner instance.
        /// </summary>
        public InventoryContainer Inventory
        {
            get
            {
                if (inventory == null) Init();
                return inventory;
            }
            private set => inventory = value;
        }

        /// <summary>
        /// Item in selected slot.
        /// </summary>
        public BasicItemConfiguration SelectedItem
        {
            get
            {
                if (SelectedInventorySlot != -1) return Inventory[SelectedInventorySlot]?.Item;
                else return null;
            }
        }

        public GameObject NearestInteractableItem { get; private set; }

        public float NearestInteractableItemDistance => NearestInteractableItem == null ? -1 : Vector3.Distance(transform.position, NearestInteractableItem.transform.position);

        public float ItemPickingProgress { get; private set; } = 0f;

        public int SelectedInventorySlot { get; set; }

        private void Start()
        {
            if (inventory == null) Init();
        }

        private void Init()
        {
            Inventory = new InventoryContainer();
            _playerBehaviour = GetComponent<PlayerBehaviour>();
        }

        private void Update()
        {
            if (Input.GetKey(KeyCode.E) && !Input.GetKey(KeyCode.LeftControl) && NearestInteractableItem != null)
            {
                ItemPickingProgress += Time.deltaTime;
                if (ItemPickingProgress > ItemPickingUpTime)
                {
                    PickItemUp();
                }
            }
            else ItemPickingProgress = 0f;
            if (Input.GetKeyDown(KeyCode.Q) && !Input.GetKey(KeyCode.LeftControl))
            {
                ThrowItemAway();
            }
        }

        /// <summary>
        /// Checks item if it's interactable.
        /// </summary>
        /// <param name="itemBehaviour">Item to test.</param>
        private void CheckNearestInteractableItem(DroppedItemBehaviour itemBehaviour)
        {
            if (itemBehaviour == null || itemBehaviour.IsThrown) return;
            float oldDistance = NearestInteractableItemDistance;
            float currentDistance = Vector3.Distance(transform.position, transform.position);
            if (currentDistance < oldDistance || oldDistance == -1)
            {
                ResetNearestItem(itemBehaviour.gameObject);
                Debug.Log($"Updated object to {itemBehaviour}");
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            CheckNearestInteractableItem(other.gameObject.GetComponent<DroppedItemBehaviour>());
        }

        private void OnTriggerExit(Collider other)
        {
            if (NearestInteractableItem == other)
            {
                ResetNearestItem(null);
                Debug.Log("Removed item object from this");
            }
        }

        private void ThrowItemAway()
        {
            Inventory.Slots[SelectedInventorySlot].DropItem(transform.position, transform.forward * 3 + Vector3.up);
        }

        private void PickItemUp()
        {
            if (_playerBehaviour.OverweightCoefficient < 2)
            {
                var drop = NearestInteractableItem.GetComponent<DroppedItemBehaviour>();
                if (Inventory.AddItem(drop.AssociatedItem,
                    drop.DroppedItemsAmount, out int rem))
                {
                    if (rem == 0)
                    {
                        drop.OnPickupHandler();
                    }
                }
            }
            NearestInteractableItem = null;
            ItemPickingProgress = 0f;
        }

        public void UnselectItem()
        {
            _lastSlotIndex = SelectedInventorySlot;
            SelectedInventorySlot = -1;
        }

        public void ReselectItem()
        {
            SelectedInventorySlot = _lastSlotIndex;
        }

        public void ResetNearestItem(GameObject item)
        {
            NearestInteractableItem = item;
            print($"New item: {item}");
        }
    }
}

