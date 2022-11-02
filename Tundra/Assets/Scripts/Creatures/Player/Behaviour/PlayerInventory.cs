using Creatures.Player.Inventory;
using System;
using Creatures.Player.Inventory.ItemConfiguration;
using UnityEngine;

namespace Creatures.Player.Behaviour
{
    public class PlayerInventory : MonoBehaviour
    {
        public static float ItemPickingUpTime => 3f;

        [SerializeField] private ItemHolder itemHolder;
        
        private InventoryContainer _inventory;
        private PlayerBehaviour _playerBehaviour;
        private int _lastSlotIndex;
        private int _currentSlotIndex;

        public InventoryContainer Inventory
        {
            get
            {
                if (_inventory == null) Init();
                return _inventory;
            }
            private set => _inventory = value;
        }

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

        public float ItemPickingProgress { get; private set; }

        public int SelectedInventorySlot 
        { 
            get
            {
                return _currentSlotIndex;
            } 
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
                ThrowItemAway();
                SelectedItemChanged?.Invoke(this, null);
            }
        }

        private void ThrowItemAway()
        {
            Inventory.Slots[SelectedInventorySlot].DropItem(transform.position, transform.forward * 3 + Vector3.up);
            itemHolder.ResetMesh();
        }

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

