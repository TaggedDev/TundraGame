using Creatures.Player.Inventory;
using Creatures.Player.States;
using GUI.HeadUpDisplay;
using System;
using Creatures.Player.Crafts;
using Creatures.Player.Crafts.Placeables;
using Creatures.Player.Inventory.ItemConfiguration;
using GUI.PlayerInventoryUI;
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
        [SerializeField] private InventoryContainer inventoryContainer;
        [SerializeField] private RecipesListConfig recipesList;
        [SerializeField] private ItemHolder itemHolder;
        [SerializeField] private InventoryUISlotsController inventoryUIController;

        public ItemHolder ItemHolder => itemHolder;
        private static float ItemPickingUpTime => 3f;
        
        private PlayerBehaviour _playerBehaviour;
        private int _currentSlotIndex;

        /// <summary>
        /// Player inventory container instance.
        /// </summary>
        public InventoryContainer InventoryContainer
        {
            get
            {
                if (inventoryContainer == null) 
                    Init();
                return inventoryContainer;
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
                if (SelectedInventorySlot != -1 && InventoryContainer[SelectedInventorySlot].Item != null)
                    return InventoryContainer[SelectedInventorySlot].Item;
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
        private float ItemPickingProgress { get; set; }
        
        /// <summary>
        /// Currently selected slot
        /// </summary>
        public int SelectedInventorySlot
        {
            get => _currentSlotIndex;
            set
            {
                if (value >= inventoryContainer.MaxInventoryCapacity)
                    value %= inventoryContainer.MaxInventoryCapacity;
                else if (value < 0)
                    value = inventoryContainer.MaxInventoryCapacity - 1;
                
                inventoryUIController.SelectChosenSlot(value, _currentSlotIndex);
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
            inventoryContainer = new InventoryContainer(inventoryUIController);
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
            
            // Select slots if buttons 1-9 are pressed 
            if (!string.IsNullOrEmpty(Input.inputString))
            {
                // If two buttons are pressed -> the first one will be shown
                char input = Input.inputString[0];
                if ('1' <= input && '9' >= input)
                {
                    int slotIndex = Convert.ToInt32(input) - 48 - 1;
                    inventoryUIController.SelectChosenSlot(slotIndex,
                        SelectedInventorySlot);
                    SelectedInventorySlot = slotIndex;
                }
            }

            // Handle wheel scroll (back & forward)
            float wheelAxis = Input.GetAxis("Mouse ScrollWheel");
            if (wheelAxis > 0)
                SelectedInventorySlot = _currentSlotIndex + 1;
            else if (wheelAxis < 0)
                SelectedInventorySlot = _currentSlotIndex - 1;
            
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
            InventoryContainer.Slots[SlotID].Clear();
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
            InventoryContainer.Slots[SelectedInventorySlot].DropItem(transform.position, transform.forward * 3 + Vector3.up);
            if (InventoryContainer.Slots[SelectedInventorySlot].ItemsAmount == 0)
            {
                itemHolder.ResetMesh();
                inventoryUIController.UIInventorySlots[SelectedInventorySlot].RemoveSlotIcon();
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
                if (InventoryContainer.AddItem(drop.AssociatedItem, drop.DroppedItemsAmount, out int rem, out int slotIndex))
                {
                    if (rem == 0)
                    {
                        drop.OnPickupHandler(itemHolder);
                    }
                    inventoryUIController.SetSlotIcon(slotIndex, drop.AssociatedItem.Icon);
                    inventoryUIController.SelectChosenSlot(slotIndex, _currentSlotIndex);
                }
            }
            NearestInteractableItem = null;
            ItemPickingProgress = 0f;
            SelectedItemChanged?.Invoke(this, SelectedInventorySlot);
        }
        
        /// <summary>
        /// Removes an item selection from the inventory.
        /// </summary>
        public void SetMagicAsSelectedItem()
        {
            SelectedInventorySlot = -1;
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
