using Creatures.Player.Inventory;
using System;
using Creatures.Player.Inventory.ItemConfiguration;
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
        public static float ItemPickingUpTime => 3f;

        [SerializeField] private ItemHolder itemHolder;

        public ItemHolder ItemHolder
        {
            get => itemHolder;
            set => itemHolder = value;
        }

        private InventoryContainer _inventory;
        private PlayerBehaviour _playerBehaviour;
        private int _lastSlotIndex;
        private int _currentSlotIndex;

        /// <summary>
        /// Player inventory contatiner instance.
        /// </summary>
        public InventoryContainer Inventory
        {
            get
            {
                if (_inventory == null) Init();
                return _inventory;
            }
            private set => _inventory = value;
        }

        /// <summary>
        /// Item in selected slot.
        /// </summary>
        public BasicItemConfiguration SelectedItem
        {
            get
            {
                if (SelectedInventorySlot != -1) 
                    return Inventory[SelectedInventorySlot]?.Item;
                else
                    return null;
            }
        }
        /// <summary>
        /// An interactable item which is the nearest to the player.
        /// </summary>
        public GameObject NearestInteractableItem { get; private set; }
        /// <summary>
        /// The distance to the <see cref="NearestInteractableItem"/>.
        /// </summary>
        public float NearestInteractableItemDistance => NearestInteractableItem == null ? -1 : Vector3.Distance(transform.position, NearestInteractableItem.transform.position);
        
        /// <summary>
        /// The progress of the item picking (or the interaction delay).
        /// </summary>
        public float ItemPickingProgress { get; private set; } = 0f;
        /// <summary>
        /// The index of a selected slot.
        /// </summary>
        public int SelectedInventorySlot 
        { 
            get => _currentSlotIndex;
            set
            {
                _currentSlotIndex = value;
                SelectedItemChanged?.Invoke(this, null);    
            } 
        }

        public event EventHandler SelectedItemChanged;

        private void Start()
        {
            if (_inventory == null) Init();
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
                    SelectedItemChanged?.Invoke(this, null);
                }
            }
            else
            {
                ItemPickingProgress = 0f;
            }
            
            if (Input.GetKeyDown(KeyCode.Q) && !Input.GetKey(KeyCode.LeftControl))
            {
                DropEquippedItem();
                SelectedItemChanged?.Invoke(this, null);
            }
        }

        private void DropEquippedItem()
        {
            Inventory.Slots[SelectedInventorySlot].DropItem(transform.position, transform.forward * 3 + Vector3.up);
            if (Inventory.Slots[SelectedInventorySlot].ItemsAmount == 0)
                itemHolder.ResetMesh();
        }

        /// <summary>
        /// Picks an item frokm the ground.
        /// </summary>
        private void PickItemUp()
        {
            // If there is no overweight, we pick up the item
            if (_playerBehaviour.OverweightCoefficient < 2)
            {
                var drop = NearestInteractableItem.GetComponent<DroppedItemBehaviour>();
                if (Inventory.AddItem(drop.AssociatedItem, drop.DroppedItemsAmount, out int rem))
                {
                    if (rem == 0)
                    {
                        drop.OnPickupHandler(itemHolder);
                    }
                }
            }
            NearestInteractableItem = null;
            ItemPickingProgress = 0f;
        }
        /// <summary>
        /// Removes an item selection from the inventory.
        /// </summary>
        public void UnselectItem()
        {
            _lastSlotIndex = SelectedInventorySlot;
            SelectedInventorySlot = -1;
        }
        /// <summary>
        /// Reselects last selected item.
        /// </summary>
        public void ReselectItem()
        {
            SelectedInventorySlot = _lastSlotIndex;
        }
        /// <summary>
        /// Resets the nearest item to the player.
        /// </summary>
        /// <param name="item">New nearest interactable item.</param>
        public void ResetNearestItem(GameObject item)
        {
            NearestInteractableItem = item;
            print($"New item: {item}");
        }
    }
}

