using Creatures.Player.Behaviour;
using Creatures.Player.States;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryUISlotsController : MonoBehaviour
{
    private int _slotId = 1;

    private int _maxSlotsNumber;

    private GameObject _player;
    [SerializeField]
    private Sprite _transparent;
    private PlayerInventory _inventoryController;
    private GameObject _pickupPanel;
    public float mouseScrollCoefficient = 10f;
    private GameObject[] _visualSlots;

    public int SelectedInventorySlot
    {
        get
        {
            return _slotId;
        }
        set
        {
            if (_slotId != value)
            {
                GameObject slot = _visualSlots[_slotId - 1];
                SetSlotActive(slot, false);
                if (value < 1) value = 1;
                if (value > MaxSlotsNumber) value = MaxSlotsNumber;
                _slotId = value;
                slot = _visualSlots[_slotId - 1];
                SetSlotActive(slot, true);
                _inventoryController.SelectedInventorySlot = _slotId - 1;
            }
        }
    }

    public int MaxSlotsNumber 
    { 
        get => _maxSlotsNumber; 
        private set => _maxSlotsNumber=value; 
    }

    private void SetSlotActive(GameObject slot, bool state)
    {
        slot.transform.Find("SelectedSlot").gameObject.SetActive(state);
    }

    // Start is called before the first frame update
    void Start()
    {
        _player = UIController._rootCanvas.GetComponent<UIController>()._player;
        _player.GetComponent<PlayerBehaviour>().StateChanged += (sender, args) =>
        {
            bool visibility = !((sender as PlayerBehaviour).CurrentState is BusyPlayerState || (sender as PlayerBehaviour).CurrentState is MagicCastingPlayerState);
            gameObject.SetActive(visibility);
        };
        _visualSlots = new GameObject[9];
        for (int i = 1; i < 10; i++)
        {
            _visualSlots[i - 1] = GameObject.Find("InventorySlot" + i);
        }
        _inventoryController = _player.GetComponent<PlayerInventory>();
        _pickupPanel = GameObject.Find("ItemPickupPanel");
        _inventoryController.Inventory.MaxInventoryCapacityChanging += ResetSlots;
        ResetSlots(this, _inventoryController.Inventory.MaxInventoryCapacity);
    }

    private void ResetSlots(object sender, int e)
    {
        MaxSlotsNumber = e;
        float offset = 150 * (UIController._rootCanvas.GetComponent<Canvas>()).scaleFactor;
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
        if (Input.GetKeyDown(KeyCode.Alpha1)) SelectedInventorySlot = 1;
        if (Input.GetKeyDown(KeyCode.Alpha2)) SelectedInventorySlot = 2;
        if (Input.GetKeyDown(KeyCode.Alpha3)) SelectedInventorySlot = 3;
        if (Input.GetKeyDown(KeyCode.Alpha4)) SelectedInventorySlot = 4;
        if (Input.GetKeyDown(KeyCode.Alpha5)) SelectedInventorySlot = 5;
        if (Input.GetKeyDown(KeyCode.Alpha6)) SelectedInventorySlot = 6;
        if (Input.GetKeyDown(KeyCode.Alpha7)) SelectedInventorySlot = 7;
        if (Input.GetKeyDown(KeyCode.Alpha8)) SelectedInventorySlot = 8;
        if (Input.GetKeyDown(KeyCode.Alpha9)) SelectedInventorySlot = 9;
        float wheel = Input.GetAxis("Mouse ScrollWheel") * mouseScrollCoefficient;
        if (SelectedInventorySlot + wheel > MaxSlotsNumber) SelectedInventorySlot = 1;
        else if (SelectedInventorySlot + wheel < 1) SelectedInventorySlot = MaxSlotsNumber;
        else SelectedInventorySlot += (int)Mathf.Round(wheel);
        int i = 0;
        foreach (var slot in _inventoryController.Inventory.Slots)
        {
            GameObject uislot = _visualSlots[i++];
            uislot.transform.Find("ItemIcon").gameObject.GetComponent<Image>().sprite = slot.Item != null ? slot.Item.Icon : _transparent;
            uislot.transform.Find("AmountIndicator").gameObject.GetComponent<Text>().text = slot.ItemsAmount.ToString();
        }
        if (_inventoryController.NearestInteractableItem == null)
        {
            _pickupPanel.SetActive(false);
        }
        else
        {
            _pickupPanel.SetActive(true);
            (_pickupPanel.transform as RectTransform).position = RectTransformUtility.WorldToScreenPoint(Camera.main, _inventoryController.NearestInteractableItem.transform.position) + new Vector2(0, 40);
            _pickupPanel.transform.Find("Progress").gameObject.GetComponent<Image>().fillAmount = _inventoryController.ItemPickingProgress / PlayerInventory.ItemPickingUpTime;
        }
    }
}
