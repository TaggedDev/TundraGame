using Creatures.Player.Behaviour;
using Creatures.Player.Inventory;
using UnityEngine;
using UnityEngine.UI;

namespace GUI.PlayerGameplayGUI
{
    /// <summary>
    /// A model to represent the panel to interact with items in world
    /// </summary>
    public class InteractionPanel : MonoBehaviour
    {
        [SerializeField] private PlayerInventory inventory;
        [SerializeField] private Image pickupProgressBar;
        [SerializeField] private string pickupText;
        [SerializeField] private string openText;
        [SerializeField] private Text pickupLabel;
        private RectTransform _rectTransform;
        private Camera _mainCamera;

        private void Start()
        {
            _rectTransform = GetComponent<RectTransform>();
            _mainCamera = Camera.main;
        }
        
        /// <summary>
        /// Enables the panel
        /// <param name="isCraftableItem">Bool variable: is item a crafting table or just an item</param>
        /// </summary>
        public void EnablePanel(bool isCraftableItem)
        {
            gameObject.SetActive(true);
            _rectTransform.position = RectTransformUtility.WorldToScreenPoint(_mainCamera, 
                inventory.NearestInteractableItem.transform.position) + new Vector2(0, 40);
            pickupProgressBar.fillAmount = inventory.ItemPickingProgress / PlayerInventory.ItemPickingUpTime;
            
            if (isCraftableItem)
                pickupLabel.text = openText;
            else 
                pickupLabel.text = pickupText;
        }

        /// <summary>
        /// Disables the interaction panel
        /// </summary>
        public void DisablePanel() => gameObject.SetActive(false);
    }
}