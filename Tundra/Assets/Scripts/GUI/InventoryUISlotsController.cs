using Creatures.Player.Behaviour;
using Creatures.Player.States;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GUI.HeadUpDisplay
{
    public class InventoryUISlotsController : MonoBehaviour
    {
        private GameObject _player;
        [SerializeField]
        private Sprite _transparent;
        private PlayerInventory _inventoryController;
        private GameObject _pickupPanel;
        public float mouseScrollCoefficient = 10f;
        private GameObject[] _visualSlots;
        private Image _progressBar;
        private Image[] _icons;
        private Text[] _texts;

        public int MaxSlotsNumber { get; private set; }

        private void SetSlotActive(GameObject slot, bool state)
        {
            slot.transform.Find("SelectedSlot").gameObject.SetActive(state);
        }

        // Start is called before the first frame update
        void Start()
        {
            _player = UIController.RootCanvas.GetComponent<UIController>().Player;
            _player.GetComponent<PlayerBehaviour>().StateChanged += (sender, args) =>
            {
                bool visibility = !((sender as PlayerBehaviour).CurrentState is BusyPlayerState || (sender as PlayerBehaviour).CurrentState is MagicCastingPlayerState);
                gameObject.SetActive(visibility);
            };
            _visualSlots = new GameObject[9];
            _icons = new Image[9];
            _texts = new Text[9];
            for (int i = 0; i < 9; i++)
            {
                _visualSlots[i] = GameObject.Find("InventorySlot" + (i + 1));
                _icons[i] = _visualSlots[i].transform.Find("ItemIcon").gameObject.GetComponent<Image>();
                _texts[i] = _visualSlots[i].transform.Find("AmountIndicator").gameObject.GetComponent<Text>();
            }
            _inventoryController = _player.GetComponent<PlayerInventory>();
            _pickupPanel = GameObject.Find("ItemPickupPanel");
            _inventoryController.Inventory.MaxInventoryCapacityChanging += ResetSlots;
            _progressBar = _pickupPanel.transform.Find("Progress").gameObject.GetComponent<Image>();
            ResetSlots(this, _inventoryController.Inventory.MaxInventoryCapacity);
        }

        private void ResetSlots(object sender, int e)
        {
            MaxSlotsNumber = e;
            float offset = 150 * (UIController.RootCanvas.GetComponent<Canvas>()).scaleFactor;
            var rect = (transform as RectTransform).rect;
            float posX = rect.center.x + rect.width / 2 - 75 - e * offset;
            for (int i = 1; i < 10; i++)
            {
                GameObject slot = _visualSlots[i - 1];
                if (i <= e)
                {
                    slot.SetActive(true);
                    slot.transform.position = new Vector3(posX, slot.transform.position.y, slot.transform.position.z);
                    posX += offset;
                }
                else
                {
                    slot.SetActive(false);
                }
            }
        }

        // Update is called once per frame
        void Update()
        {
            int tempVal = _inventoryController.SelectedInventorySlot;
            if (Input.GetKeyDown(KeyCode.Alpha1)) tempVal = 0;
            if (Input.GetKeyDown(KeyCode.Alpha2)) tempVal = 1;
            if (Input.GetKeyDown(KeyCode.Alpha3)) tempVal = 2;
            if (Input.GetKeyDown(KeyCode.Alpha4)) tempVal = 3;
            if (Input.GetKeyDown(KeyCode.Alpha5)) tempVal = 4;
            if (Input.GetKeyDown(KeyCode.Alpha6)) tempVal = 5;
            if (Input.GetKeyDown(KeyCode.Alpha7)) tempVal = 6;
            if (Input.GetKeyDown(KeyCode.Alpha8)) tempVal = 7;
            if (Input.GetKeyDown(KeyCode.Alpha9)) tempVal = 8;
            float wheel = Input.GetAxis("Mouse ScrollWheel") * mouseScrollCoefficient;
            if (_inventoryController.SelectedInventorySlot + wheel > MaxSlotsNumber - 1) tempVal = 0;
            else if (_inventoryController.SelectedInventorySlot + wheel < 0) tempVal = MaxSlotsNumber - 1;
            else tempVal += (int)Mathf.Round(wheel);
            if (tempVal != _inventoryController.SelectedInventorySlot) _inventoryController.SelectedInventorySlot = tempVal;
            int i = 0;
            foreach (var slot in _inventoryController.Inventory.Slots)
            {
                GameObject uislot = _visualSlots[i];
                SetSlotActive(uislot, _inventoryController.SelectedInventorySlot == i);
                _icons[i].sprite = slot.Item != null ? slot.Item.Icon : _transparent;
                _texts[i].text = slot.ItemsAmount.ToString();
                i++;
            }
            if (_inventoryController.NearestInteractableItem == null)
            {
                _pickupPanel.SetActive(false);
            }
            else
            {
                _pickupPanel.SetActive(true);
                (_pickupPanel.transform as RectTransform).position = RectTransformUtility.WorldToScreenPoint(Camera.main, _inventoryController.NearestInteractableItem.transform.position) + new Vector2(0, 40);
                _progressBar.fillAmount = _inventoryController.ItemPickingProgress / PlayerInventory.ItemPickingUpTime;
            }
        }
    }
}