using UnityEngine;
using UnityEngine.UI;

namespace GUI.PlayerInventoryUI
{
    public class InventorySlotsScrollView : MonoBehaviour
    {
        [SerializeField] private UISlot slotPrefab;
        private GridLayoutGroup uiSlotsParent;

        private void Start()
        {
            uiSlotsParent = GetComponentInChildren<GridLayoutGroup>();
            SetUISlots();
        }

        private void SetUISlots()
        {
            for (int i = 0; i < 8; i++)
            {
                Instantiate(slotPrefab, uiSlotsParent.transform);
            }
        }
    }
}