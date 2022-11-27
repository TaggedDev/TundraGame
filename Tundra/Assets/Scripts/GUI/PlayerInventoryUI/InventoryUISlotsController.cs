using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace GUI.PlayerInventoryUI
{
    public class InventoryUISlotsController : MonoBehaviour
    {
        [SerializeField] private UIInventorySlot inventorySlotPrefab;
        private UIInventorySlot[] _uiInventorySlots;
        private GridLayoutGroup _uiSlotsParent;
        
        private void Start()
        {
            _uiSlotsParent = GetComponentInChildren<GridLayoutGroup>();
            SetUISlots();
            _uiInventorySlots = GetComponentsInChildren<UIInventorySlot>();
        }
        
        /// <summary>
        /// Sets (removes or adds) visibility of the slots in the inventory 
        /// </summary>
        /// <param name="count">The amount of slots that has to be drawn</param>
        public void SetInventorySlots(int count)
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

        private void SetUISlots()
        {
            for (int i = 0; i < 9; i++)
            {
                Instantiate(inventorySlotPrefab, _uiSlotsParent.transform);
            }
        }
    }
}