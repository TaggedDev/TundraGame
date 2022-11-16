using Creatures.Player.Crafts;
using Creatures.Player.Inventory;
using Creatures.Player.States;
using GUI.HeadUpDisplay;
using System;
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
        [SerializeField]
        private InventoryContainer inventory;
        [SerializeField]
        private RecipesListConfig recipesList;
 
        public static float ItemPickingUpTime => 3f;

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
                SelectedItemChanged?.Invoke(this, value);
            }
        }

        public RecipesListConfig RecipesList => recipesList;

        public event EventHandler<int> SelectedItemChanged;

        private void Start()
        {
            Init();
        }

        private void Init()
        {
            if (inventory == null) inventory = new InventoryContainer();
            _playerBehaviour = GetComponent<PlayerBehaviour>();
        }

        private void Update()
        {
            if (Input.GetKey(KeyCode.E) && !Input.GetKey(KeyCode.LeftControl) && NearestInteractableItem != null)
            {
                ItemPickingProgress += Time.deltaTime;
                if (ItemPickingProgress > ItemPickingUpTime)
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

        private void OnTriggerEnter(Collider other)
        {
            var drop = other.gameObject.GetComponent<DroppedItemBehaviour>();
            if (drop != null)
            {
                if (drop.IsThrown) return;
                CheckNearestInteractableItem(drop.gameObject);
            }
            else
            {
                var placeable = other.GetComponent<PlaceableObjectBehaviour>();
                if (placeable != null) CheckNearestInteractableItem(placeable.gameObject);
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
