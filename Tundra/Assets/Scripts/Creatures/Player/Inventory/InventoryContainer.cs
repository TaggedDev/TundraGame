using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Creatures.Player.Inventory.ItemConfiguration;
using UnityEngine;

namespace Creatures.Player.Inventory
{
    /// <summary>
    /// A model to store inventory items
    /// </summary>
    public class InventoryContainer
    {
        private int maxInventoryCapacity = 4;

        /// <summary>
        /// Maximum inventory capacity
        /// </summary>
        public int MaxInventoryCapacity
        {
            get => maxInventoryCapacity;
            private set
            {
                MaxInventoryCapacityChanging?.Invoke(this, value);
                maxInventoryCapacity=value;
                Slot[] newInventory = new Slot[maxInventoryCapacity];
                Slots.CopyTo(newInventory, 0);
                Slots = newInventory;
                for (int i = 0; i < Slots.Length; i++)
                    if (Slots[i] == null) Slots[i] = new Slot();
            }
        }
        
        /// <summary>
        /// Массив слотов инвентаря.
        /// </summary>
        public Slot[] Slots { get; private set; }
        
        /// <summary>
        /// Общий суммарный вес всех предметов в инвентаре.
        /// </summary>
        public float TotalWeight => Slots.Sum(x => x.Item == null ? 0 : x.Item.Weight * x.ItemsAmount);

        /// <summary>
        /// Returns the slot by index
        /// </summary>
        /// <param name="index">Index of requested slot</param>
        public Slot this[int index] => Slots[index];

        /// <summary>
        /// Событие, происходящее перед изменением количества слотов в инвентаре.
        /// </summary>
        public event EventHandler<int> MaxInventoryCapacityChanging;

        //TODO: организовать сохранение предметов и т.д
        /// <summary>
        /// Добавляет предмет в инвентарь.
        /// </summary>
        /// <param name="item">Добавляемый предмет.</param>
        /// <param name="amount">Количество предметов.</param>
        /// <param name="rem">Остаток после добавления.</param>
        /// <returns>True if all items have been added and false if only part of them</returns>
        public bool AddItem(BasicItemConfiguration item, int amount, out int rem)
        {
            while (amount > 0)
            {
                Slot slot = FindNearestSlot(item, amount, out int remainder);
                if (slot != null)
                {
                    if (remainder > 0)
                    {
                        slot.Fill(item);
                    }
                    else if (slot.IsEmpty) slot.PushItem(item, amount);
                    else slot.AddItems(amount);
                    amount = remainder;
                }
                else
                {
                    rem = remainder;
                    return false;
                }
            }
            rem = 0;
            return true;
        }

        /// <summary>
        /// Finds the first available slot to store this item 
        /// </summary>
        /// <param name="item">Item to store</param>
        /// <param name="amount">Amount of items to store</param>
        /// <param name="remainder">Amount of items can't be stored in this slot</param>
        /// <returns>First available slot</returns>
        private Slot FindNearestSlot(BasicItemConfiguration item, int amount, out int remainder)
        {
            remainder = 0;
            Slot found = Slots.FirstOrDefault(x => x.IsEmpty || x.Item == item && !x.IsFull);
            if (found != null)
            {
                remainder = Math.Max(0, found.ItemsAmount + amount - item.MaxStackVolume);
                return found;
            }
            remainder = amount;
            return null;
        }

        public InventoryContainer()
        {
            Slots = new Slot[MaxInventoryCapacity];
            for (int i = 0; i<Slots.Length; i++)
            {
                Slots[i] = new Slot();
            }
        }

        /// <summary>
        /// Resizes inventory by the amount provided
        /// </summary>
        /// <param name="additionalSlotsAmount">Amount of additional slots to add</param>
        internal void ResizeInventory(int additionalSlotsAmount)
        {
            MaxInventoryCapacity = 4 + additionalSlotsAmount;
        }
    }
}