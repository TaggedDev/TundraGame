using Creatures.Player.Crafts;
using Creatures.Player.Inventory;
using System;
using System.Data;
using UnityEngine;

namespace Creatures.Player.Behaviour
{
    public class PlayerInventory : MonoBehaviour
    {
        [SerializeField]
        private InventoryContainer inventory;
        [SerializeField]
        private RecipesListConfig recipesList;

        public static float ItemPickingUpTime => 3f;

        private PlayerBehaviour _playerBehaviour;
        private int _lastSlotIndex = 0;
        private int _currentSlotIndex;

        public InventoryContainer Inventory
        {
            get
            {
                if (inventory == null) Init();
                return inventory;
            }
            private set => inventory = value;
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

        public float ItemPickingProgress { get; private set; } = 0f;

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

        internal RecipesListConfig RecipesList => recipesList;

        public event EventHandler SelectedItemChanged;

        // Start is called before the first frame update
        void Start()
        {
            Init();
        }

        private void Init()
        {
            Inventory = new InventoryContainer();
            _playerBehaviour = GetComponent<PlayerBehaviour>();
        }

        // Update is called once per frame
        void Update()
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
            else ItemPickingProgress = 0f;
            if (Input.GetKeyDown(KeyCode.Q) && !Input.GetKey(KeyCode.LeftControl))
            {
                ThrowItemAway();
                SelectedItemChanged?.Invoke(this, null);
            }
        }

        private void ThrowItemAway()
        {
            Inventory.Slots[SelectedInventorySlot].DropItem(transform.position, transform.forward * 3 + Vector3.up);
        }

        private void PickItemUp()
        {
            Debug.Log(_playerBehaviour);
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

