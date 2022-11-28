using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace GUI.PlayerInventoryUI
{
    /// <summary>
    /// A controller to manage the view of inventory slots
    /// </summary>
    public class InventoryUISlotsController : MonoBehaviour
    {
        [SerializeField] private UIInventorySlot inventorySlotPrefab;
        private UIInventorySlot[] _uiInventorySlots;
        private GridLayoutGroup _uiSlotsParent;
        private int _currentVisibleSlotsAmount;

        private void Start()
        {
            _uiSlotsParent = GetComponentInChildren<GridLayoutGroup>();
            InitialiseUISlots();
            _uiInventorySlots = GetComponentsInChildren<UIInventorySlot>();
            SelectChosenSlot(0, 0);
        }
        
        /// <summary>
        /// Sets (removes or adds) visibility of the slots in the inventory 
        /// </summary>
        /// <param name="count">The amount of slots that has to be drawn</param>
        public void SetInventorySlotsVisibility(int count)
        {
            // Count active inventory slots
            int currentSlotAmount = _uiSlotsParent.transform
                .GetComponentsInChildren<UIInventorySlot>().Count(x => x.gameObject.activeSelf);
            
            // If we need to add slots
            if (currentSlotAmount < count)
                for (int i = currentSlotAmount - 1; i < count; i++)
                    _uiInventorySlots[i].gameObject.SetActive(true);
            // If we need to remove slots
            else if (currentSlotAmount > count)
                for (int i = count; i < currentSlotAmount; i++)
                    _uiInventorySlots[i].gameObject.SetActive(false);
        }

        /// <summary>
        /// Initialises the slots when game starts
        /// </summary>
        private void InitialiseUISlots()
        {
            for (int i = 0; i < 9; i++)
                Instantiate(inventorySlotPrefab, _uiSlotsParent.transform);
        }

        /// <summary>
        /// Adds visual effects on slot with given index (starts with 0)
        /// </summary>
        /// <param name="slotToActivate">The index of a slot to highlight</param>
        /// <param name="slotToDeactivate">The index of a slot to remove the highlight</param>
        public void SelectChosenSlot(int slotToActivate, int slotToDeactivate)
        {
            _uiInventorySlots[slotToDeactivate].RemoveSlotSelectionEffects();
            _uiInventorySlots[slotToActivate].ApplySlotSelectionEffects();

            /*// Hande user mousewheel scroll
            float wheel = Input.GetAxis("Mouse ScrollWheel") /* * mouseScrollCoefficient#1#;
            
            if (_inventoryController.SelectedInventorySlot + wheel > MaxSlotsNumber - 1) 
                tempVal = 0;
            else if (_inventoryController.SelectedInventorySlot + wheel < 0) 
                tempVal = MaxSlotsNumber - 1;
            else 
                tempVal += (int)Mathf.Round(wheel);*/

            /*// Prevent new 
            if (tempVal != _inventoryController.SelectedInventorySlot)
                _inventoryController.SelectedInventorySlot = tempVal;
            
            // Add new visual states
            int i = 0;
            foreach (var slot in _inventoryController.Inventory.Slots)
            {
                GameObject UIslot = _visualSlots[i];
                SetSlotActive(UIslot, _inventoryController.SelectedInventorySlot == i);
                _icons[i].sprite = slot.Item != null ? slot.Item.Icon : transparent;
                _texts[i].text = slot.ItemsAmount.ToString();
                i++;
            }#2#

            // Show the nearest interactable item tile.
            if (_inventoryController.NearestInteractableItem == null)
            {
                _pickupPanel.SetActive(false);
            }
            else
            {
                _pickupPanel.SetActive(true);
                (_pickupPanel.transform as RectTransform).position = RectTransformUtility.WorldToScreenPoint(_mainCamera, _inventoryController.NearestInteractableItem.transform.position) + new Vector2(0, 40);
                _progressBar.fillAmount = _inventoryController.ItemPickingProgress / PlayerInventory.ItemPickingUpTime;
                if (_inventoryController.NearestInteractableItem.GetComponent<DroppedItemBehaviour>()) _pickupLabel.text = pickupText;
                else _pickupLabel.text = openText;
            }*/
        }
    }
}