using UnityEngine;
using UnityEngine.UI;

namespace GUI.PlayerInventoryUI
{
    /// <summary>
    /// A model to represent the visual slot behaviour 
    /// </summary>
    public class UIInventorySlot : MonoBehaviour
    {
        [SerializeField] private Image slotBackground;
        [SerializeField] private Image slotOutline;

        public bool IsSelected { get; set; }

        public void ApplySlotSelectionEffects()
        {
            var tempColor = slotOutline.color;
            tempColor.a = 1f;
            slotOutline.color = tempColor;
            IsSelected = true;
        }

        public void RemoveSlotSelectionEffects()
        {
            var tempColor = slotOutline.color;
            tempColor.a = 150/255f;
            slotOutline.color = tempColor;
            IsSelected = false;
        }
    }
}