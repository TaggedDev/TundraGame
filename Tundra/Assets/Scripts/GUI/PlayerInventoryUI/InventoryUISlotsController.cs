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
        public UIInventorySlot[] UIInventorySlots => _uiInventorySlots;
        
        [SerializeField] private UIInventorySlot inventorySlotPrefab;
        private UIInventorySlot[] _uiInventorySlots;
        private GridLayoutGroup _uiSlotsParent;
        private int _currentVisibleSlotsAmount;

        private void Awake()
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
        public void SetVisibleSlotAmount(int count)
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
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="slotIndex"></param>
        /// <param name="sprite"></param>
        public void SetSlotIcon(int slotIndex, Sprite sprite)
        {
            _uiInventorySlots[slotIndex].SetSlotIcon(sprite);
        }
    }
}