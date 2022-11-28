using Creatures.Player.Inventory;
using Creatures.Player.States;
using GUI.HeadUpDisplay;
using System;
using Creatures.Player.Crafts;
using Creatures.Player.Crafts.Placeables;
using Creatures.Player.Inventory.ItemConfiguration;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace Creatures.Player.Behaviour
{
    /// <summary>
    /// Class that handles player's inventory logic.
    /// </summary>
    [RequireComponent(typeof(SphereCollider))]
    [RequireComponent(typeof(PlayerBehaviour))]
    public class PlayerInventory : MonoBehaviour
    {

        [SerializeField] private MeleeWeaponConfiguration fist;
        [SerializeField] private InventoryContainer inventory;
        [SerializeField] private RecipesListConfig recipesList;
        [SerializeField] private ItemHolder itemHolder;
 
        public ItemHolder ItemHolder => itemHolder;
        public static float ItemPickingUpTime => 3f;
        
        private PlayerBehaviour _playerBehaviour;
        private int _lastSlotIndex;
        private int _currentSlotIndex;

        /// <summary>
        /// Player inventory container instance.
        /// </summary>
        public InventoryContainer Inventory
        {
            get
            {
                if (inventory == null) 
                    Init();
                return inventory;
            }
        }
        /// <summary>
        /// Item that is currently in hands <br/>
        /// Or hands itself
        /// </summary>
        public BasicItemConfiguration SelectedItem
        {
            get
            {
                if (SelectedInventorySlot != -1 && Inventory[SelectedInventorySlot].Item != null)
                    return Inventory[SelectedInventorySlot].Item;
                else if(SelectedInventorySlot != -1)
                    return fist;
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
        /// Currently selected slot
        /// </summary>
        public int SelectedInventorySlot
        {
            get
            {
                return _currentSlotIndex;
            }
            set
            {
                _currentSlotIndex = value;
                SelectedItemChanged?.Invoke(this, value);
                
            }
        }

        public RecipesListConfig RecipesList => recipesList;

        /// <summary>
        /// Invokes on changing selected item
        /// </summary>
        public event EventHandler<int> SelectedItemChanged;

        private void Start()
        {
            Init();
        }

        private void Init()
        {
            inventory = new InventoryContainer();
            _playerBehaviour = GetComponent<PlayerBehaviour>();
        }

        private void Update()
        {
            if (Input.GetKey(KeyCode.E) && !Input.GetKey(KeyCode.LeftControl) && NearestInteractableItem != null)
            {
                ItemPickingProgress += Time.deltaTime;
                if (ItemPickingProgress > ItemPickingUpTime)
                {
                    InteractWithItem();
                }
            }
            else
            {
                ItemPickingProgress = 0f;
            }
            if (Input.GetKeyDown(KeyCode.Q) && !Input.GetKey(KeyCode.LeftControl))
            {
                DropEquippedItem();
                SelectedItemChanged?.Invoke(this, 0);
            }
        }

        /// <summary>
        /// Do an interaction with an item.
        /// </summary>
        private void InteractWithItem()
        {
            var craft = NearestInteractableItem.GetComponent<PlaceableObjectBehaviour>();
            if (craft != null && craft.CanBeOpened)
            {
                _playerBehaviour.SwitchState<BusyPlayerState>();
                UIController.CraftPanel.ShowPanel(craft.Configuration);
                NearestInteractableItem = null;
                ItemPickingProgress = 0f;
            }
            else
            {
                PickItemUp();
            }
        }

        /// <summary>
        /// Checks item if it's interactable.
        /// </summary>
        /// <param name="item">Item to test.</param>
        private void CheckNearestInteractableItem(GameObject item)
        {
            float oldDistance = NearestInteractableItemDistance;
            float currentDistance = Vector3.Distance(transform.position, transform.position);
            if (currentDistance < oldDistance || oldDistance == -1)
            {
                ResetNearestItem(item);
            }
        }

        /// <summary>
        /// Clears Selected Slot, raising an event
        /// </summary>
        /// <param name="SlotID">Slot Id to clear</param>
        public void ClearSlot(int SlotID)
        {
            this.Inventory.Slots[SlotID].Clear();
            if(SlotID == SelectedInventorySlot)
                SelectedItemChanged?.Invoke(this, SelectedInventorySlot);
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent<DroppedItemBehaviour>(out var drop))
            {
                if (drop.IsThrown) return;
                CheckNearestInteractableItem(drop.gameObject);
            }
            else
            {
                if (other.TryGetComponent<PlaceableObjectBehaviour>(out var placeable))
                    CheckNearestInteractableItem(placeable.gameObject);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (NearestInteractableItem == other.gameObject)
            {
                ResetNearestItem(null);
                Debug.Log("Removed item object from this");
            }
        }

        /// <summary>
        /// Handles the drop item event
        /// </summary>
        private void DropEquippedItem()
        {
            Inventory.Slots[SelectedInventorySlot].DropItem(transform.position, transform.forward * 3 + Vector3.up);
            if (Inventory.Slots[SelectedInventorySlot].ItemsAmount == 0)
            {
                itemHolder.ResetMesh();
                SelectedItemChanged?.Invoke(this, SelectedInventorySlot);
            }

        }

        /// <summary>
        /// Picks an item from the ground.
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
            SelectedItemChanged?.Invoke(this, SelectedInventorySlot);
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
        /// Re-selects last selected item.
        /// </summary>
        public void ReSelectItem()
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
        }
    }
}
