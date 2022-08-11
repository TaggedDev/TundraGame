using Player.Behaviour;
using Player.Inventory;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipment : MonoBehaviour
{
    [SerializeField]
    private EquipmentConfiguration head;
    [SerializeField]
    private EquipmentConfiguration body;
    [SerializeField]
    private EquipmentConfiguration legs;
    [SerializeField]
    private EquipmentConfiguration feet;
    [SerializeField]
    private EquipmentConfiguration neck;
    [SerializeField]
    private EquipmentConfiguration backpack;

    private PlayerInventoryController _inventoryController;

    public EquipmentConfiguration Helmet
    {
        get => head;
        set
        {
            if (value.EquipmentSlot == EquipmentSlotPosition.Head)
            {
                var temp = head;
                head = value;
                EquipmentChanged?.Invoke(this, temp);
                OnEquipmentChanged();
            }
            else throw new ArgumentException("This item cannot be equipped to this slot.", nameof(value));
        }
    }

    public EquipmentConfiguration Body
    {
        get => body;
        set
        {
            if (body.EquipmentSlot == EquipmentSlotPosition.Body)
            {
                var temp = body;
                body = value;
                EquipmentChanged?.Invoke(this, temp);
                OnEquipmentChanged();
            }
            else throw new ArgumentException("This item cannot be equipped to this slot.", nameof(value));
        }
    }

    public EquipmentConfiguration Legs
    {
        get => legs;
        set
        {
            if (legs.EquipmentSlot == EquipmentSlotPosition.Legs)
            {
                var temp = legs;
                legs = value;
                EquipmentChanged?.Invoke(this, temp);
                OnEquipmentChanged();
            }
            else throw new ArgumentException("This item cannot be equipped to this slot.", nameof(value));
        }
    }

    public EquipmentConfiguration Feet
    {
        get => feet;
        set
        {
            if (feet.EquipmentSlot == EquipmentSlotPosition.Feet)
            {
                var temp = feet;
                feet = value;
                EquipmentChanged?.Invoke(this, temp);
                OnEquipmentChanged();
            }
            else throw new ArgumentException("This item cannot be equipped to this slot.", nameof(value));
        }
    }

    public EquipmentConfiguration Neck
    {
        get => neck;
        set
        {
            if (neck.EquipmentSlot == EquipmentSlotPosition.Neck)
            {
                var temp = neck;
                neck = value;
                EquipmentChanged?.Invoke(this, temp);
                OnEquipmentChanged();
            }
            else throw new ArgumentException("This item cannot be equipped to this slot.", nameof(value));
        }
    }

    public EquipmentConfiguration Backpack
    {
        get => backpack;
        set
        {
            if (backpack.EquipmentSlot == EquipmentSlotPosition.Backpack)
            {
                var temp = backpack;
                backpack = value;
                EquipmentChanged?.Invoke(this, temp);
                OnEquipmentChanged();
            }
            else throw new ArgumentException("This item cannot be equipped to this slot.", nameof(value));
        }
    }

    public int TotalAdditionalSlots
    {
        get 
        {
            int total = 0;
            if (head != null) total += head.AdditionalSlots;
            if (body != null) total += body.AdditionalSlots;
            if (legs != null) total += legs.AdditionalSlots;
            if (feet != null) total += feet.AdditionalSlots;
            if (neck != null) total += neck.AdditionalSlots;
            if (backpack != null) total += backpack.AdditionalSlots;
            return total;
        }
    }

    public event EventHandler<EquipmentConfiguration> EquipmentChanged;

    // Start is called before the first frame update
    void Start()
    {
        _inventoryController = GetComponent<PlayerInventoryController>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnEquipmentChanged()
    {
        _inventoryController.Inventory.ResizeInventory(TotalAdditionalSlots);
    }
}
