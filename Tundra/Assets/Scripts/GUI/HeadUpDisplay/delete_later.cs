using Creatures.Player.Behaviour;
using GameObject = UnityEngine.GameObject;
using Creatures.Player.States;
using UnityEngine;
using UnityEngine.UI;

namespace GUI.HeadUpDisplay
{
    /// <summary>
    /// A script to control inventory slots visualization.
    /// </summary>
    public class delete_later : MonoBehaviour
    {
        
        [SerializeField] private Sprite transparent;
        private Camera _mainCamera;
        private PlayerInventory _inventoryController;
        private GameObject _player; // Replace GameObject with smth else
        private int _maxSlotsNumber;
        public float mouseScrollCoefficient = 10f;
        private Image[] _icons;
        private Text[] _texts;
        
        private void Start()
        {
            // Initialize all fields.
            _mainCamera = Camera.main;
            // Bind to player StateChanged event
            _player = UIController.RootCanvas.GetComponent<UIController>().Player;
            _player.GetComponent<PlayerBehaviour>().StateChanged += (sender, args) =>
            {
                bool visibility = !((sender as PlayerBehaviour).CurrentState is BusyPlayerState ||
                                    (sender as PlayerBehaviour).CurrentState is MagicCastingPlayerState);
                gameObject.SetActive(visibility);
            };
            
            /*// Cache another fields
            _inventoryController = _player.GetComponent<PlayerInventory>();
            _pickupPanel = GameObject.Find("ItemPickupPanel");
            _inventoryController.Inventory.MaxInventoryCapacityChanging += ResetSlots;
            // Rework! No find method is allowed in runtime
            _progressBar = _pickupPanel.transform.Find("Progress").gameObject.GetComponent<Image>();
            _pickupLabel = _pickupPanel.transform.Find("Text").gameObject.GetComponent<Text>();
            // Reset slots values.
            ResetSlots(this, _inventoryController.Inventory.MaxInventoryCapacity);*/
        }

        /*private void Update()
        {
            // TODO: Extract a function for user input
            
            // Handle user digits input
            int tempVal = _inventoryController.SelectedInventorySlot;
            if (Input.GetKeyDown(KeyCode.Alpha1))
                tempVal = 0;
            if (Input.GetKeyDown(KeyCode.Alpha2))
                tempVal = 1;
            if (Input.GetKeyDown(KeyCode.Alpha3))
                tempVal = 2;
            if (Input.GetKeyDown(KeyCode.Alpha4))
                tempVal = 3;
            if (Input.GetKeyDown(KeyCode.Alpha5))
                tempVal = 4;
            if (Input.GetKeyDown(KeyCode.Alpha6))
                tempVal = 5;
            if (Input.GetKeyDown(KeyCode.Alpha7))
                tempVal = 6;
            if (Input.GetKeyDown(KeyCode.Alpha8))
                tempVal = 7;
            if (Input.GetKeyDown(KeyCode.Alpha9))
                tempVal = 8;
            
            // Hande user mousewheel scroll
            float wheel = Input.GetAxis("Mouse ScrollWheel") * mouseScrollCoefficient;
            
            if (_inventoryController.SelectedInventorySlot + wheel > MaxSlotsNumber - 1) 
                tempVal = 0;
            else if (_inventoryController.SelectedInventorySlot + wheel < 0) 
                tempVal = MaxSlotsNumber - 1;
            else 
                tempVal += (int)Mathf.Round(wheel);
            
            // Prevent new 
            if (tempVal != _inventoryController.SelectedInventorySlot)
                _inventoryController.SelectedInventorySlot = tempVal;
            
            // Add new visual states
            /*int i = 0;
            foreach (var slot in _inventoryController.Inventory.Slots)
            {
                GameObject UIslot = _visualSlots[i];
                SetSlotActive(UIslot, _inventoryController.SelectedInventorySlot == i);
                _icons[i].sprite = slot.Item != null ? slot.Item.Icon : transparent;
                _texts[i].text = slot.ItemsAmount.ToString();
                i++;
            }#1#

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
            }
        }*/
    }
}