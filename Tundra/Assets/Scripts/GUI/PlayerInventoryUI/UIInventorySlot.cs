using UnityEngine;
using UnityEngine.UI;

namespace GUI.PlayerInventoryUI
{
    /// <summary>
    /// A model to represent the visual slot behaviour 
    /// </summary>
    public class UIInventorySlot : MonoBehaviour
    {
        [SerializeField] private Image slotIcon;
        [SerializeField] private Image slotOutline;

        public bool IsSelected { get; set; }

        /// <summary>
        /// Applies slot selection effects on this slot in inventory UI
        /// </summary>
        public void ApplySlotSelectionEffects()
        {
            var tempColor = slotOutline.color;
            tempColor.a = 1f;
            slotOutline.color = tempColor;
            IsSelected = true;
        }
        
        /// <summary>
        /// Removes slot selection effects on this slot in inventory UI
        /// </summary>
        public void RemoveSlotSelectionEffects()
        {
            var tempColor = slotOutline.color;
            tempColor.a = 150/255f;
            slotOutline.color = tempColor;
            IsSelected = false;
        }

        /// <summary>
        /// Sets the sprite image to this slot
        /// </summary>
        /// <param name="sprite">Sprite to set as an icon</param>
        public void SetSlotIcon(Sprite sprite)
        {
            slotIcon.sprite = sprite;
            slotIcon.enabled = true;
        }

        /// <summary>
        /// Removes the icon image from this slot
        /// </summary>
        public void RemoveSlotIcon()
        {
            slotIcon.sprite = null;
            slotIcon.enabled = false;
        }
        
    }
}