using System;
using System.Collections.Generic;
using System.Linq;

namespace Creatures.Player.Inventory
{
    /// <summary>
    /// A class which represents a container to store player's items.
    /// </summary>
    public class InventoryContainer
    {
        private int maxInventoryCapacity = 4;

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
        ///Общий суммарный вес всех предметов в инвентаре.
        /// </summary>
        public float TotalWeight => Slots.Sum(x => x.Item == null ? 0 : x.Item.Weight * x.ItemsAmount);
        /// <summary>
        /// Returns a slot with given index.
        /// </summary>
        /// <param name="index">Provided index.</param>
        /// <returns></returns>
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
        /// <returns></returns>
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
        /// FInds all slots which contains item of this type.
        /// </summary>
        /// <param name="item">Item configuration to find slots.</param>
        /// <returns></returns>
        public List<Slot> FindSlotsWithItem(BasicItemConfiguration item)
        {
            return Slots.Where(x => x.Item == item).ToList();
        }

        /// <summary>
        /// Counts the number of items of this type in the inventory.
        /// </summary>
        /// <param name="item">Item configuration.</param>
        /// <returns></returns>
        public int CountItemOfTypeInTheInventory(BasicItemConfiguration item)
        {
            return Slots.Aggregate(0, (x, y) => x += y.Item == item ? y.ItemsAmount : 0);//Исхожу из ситуации, что ItemConfiguration существует в единственном экземпляре для каждого предмета
        }
        /// <summary>
        /// Finds the first slot to store item of provided configuration.
        /// </summary>
        /// <param name="item">Item configuration.</param>
        /// <param name="amount">Amount of items.</param>
        /// <param name="remainder">Number of items which can't be stored in this slot.</param>
        /// <returns></returns>
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

        internal void ResizeInventory(int additionalSlotsAmount)
        {
            MaxInventoryCapacity = 4 + additionalSlotsAmount;
        }
    }
}
