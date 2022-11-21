using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Creatures.Player.Inventory.ItemConfiguration;
using UnityEngine;

namespace Creatures.Player.Inventory
{
    /// <summary>
    /// A model to store inventory items
    /// </summary>
    [Serializable]
    public class InventoryContainer : IEnumerable<Slot>
    {
        [SerializeField] private int maxInventoryCapacity = 4;
        [SerializeField] private Slot[] slots;

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
                {
                    if (Slots[i] == null)
                    {
                        Slots[i] = new Slot();
                        Slots[i].ItemChanged += (s, e) => ContentChanged?.Invoke(s, new ItemChangeArgs(i, e));
                    }
                }
            }
        }

        /// <summary>
        /// An array of slots in the inventory.
        /// </summary>
        public Slot[] Slots { get => slots; private set => slots=value; }

        /// <summary>
        /// Total summary weight of all items in the inventory.
        /// </summary>
        public float TotalWeight => Slots.Sum(x => x.Item == null ? 0 : x.Item.Weight * x.ItemsAmount);

        /// <summary>
        /// Returns a slot with the provided index.
        /// </summary>
        /// <param name="index">Index of a slot.</param>
        public Slot this[int index] => Slots[index];

        /// <summary>
        /// An event raises when the maximal inventory capacity has been changed.
        /// </summary>
        public event EventHandler<int> MaxInventoryCapacityChanging;
        
        /// <summary>
        /// An event raises when the content of the slot has been changed.
        /// </summary>
        public event EventHandler<ItemChangeArgs> ContentChanged;

        /// <summary>
        /// Adds an item into the inventory.
        /// </summary>
        /// <param name="item">An item configuration.</param>
        /// <param name="amount">Amount of items.</param>
        /// <param name="rem">Remainder of items (how much weren't added).</param>
        /// <returns><see langword="true"/> if the item has been added into the inventory, <see langword="false"/> otherwise.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="item"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the <paramref name="item"/> has no assigned storage limit.</exception>
        public bool AddItem(BasicItemConfiguration item, int amount, out int rem)
        {
            if (item == null) 
                throw new ArgumentNullException(nameof(item));
            if (item.MaxStackVolume == 0) 
                throw new ArgumentOutOfRangeException(nameof(item), "Item must have an amount limit to add it to the inventory.");
            while (amount > 0)
            {
                Slot slot = FindNearestSlot(item, amount, out int remainder);
                if (slot != null)
                {
                    if (remainder > 0)
                        slot.Fill(item);
                    else if (slot.IsEmpty) 
                        slot.PushItem(item, amount);
                    else 
                        slot.AddItems(amount);
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
        /// Finds slots containing this item configuration.
        /// </summary>
        /// <param name="item">An item configuration.</param>
        /// <returns>List of slots containing this item.</returns>
        public List<Slot> FindSlotsWithItem(BasicItemConfiguration item) => Slots.Where(x => x.Item == item).ToList();

        /// <summary>
        /// Counts the items of this configuration are in inventory.
        /// </summary>
        /// <param name="item">An item configuration.</param>
        /// <returns>Amount of items of this configuration.</returns>
        public int CountItemOfTypeInTheInventory(BasicItemConfiguration item)
        {
            return Slots.Aggregate(0, (x, y) => x += y.Item == item ? y.ItemsAmount : 0);//Исхожу из ситуации, что ItemConfiguration существует в единственном экземпляре для каждого предмета
        }

        /// <summary>
        /// A helper method to find the first available slot to keep this item.
        /// </summary>
        /// <param name="item">An item configuration.</param>
        /// <param name="amount">Amount of items to keep.</param>
        /// <param name="remainder">How much items will be in remainder if add them to provided slot.</param>
        /// <returns>The first slot which is empty or its item has given configuration.</returns>
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

        /// <summary>
        /// Creates new inventory container.
        /// </summary>
        public InventoryContainer()
        {
            Slots = new Slot[MaxInventoryCapacity];
            for (int i = 0; i<Slots.Length; i++)
            {
                Slots[i] = new Slot();
                Slots[i].ItemChanged += (s, e) => ContentChanged?.Invoke(s, new ItemChangeArgs(i, e));
            }
        }

        /// <summary>
        /// Resizes inventory with amount of additional slots.
        /// </summary>
        /// <param name="additionalSlotsAmount">Number of slots in addition to 4 basic slots.</param>
        public void ResizeInventory(int additionalSlotsAmount)
        {
            MaxInventoryCapacity = 4 + additionalSlotsAmount;
        }

        public override string ToString()
        {
            string res = $"Container: [max capacity {maxInventoryCapacity}; slots: {Slots.Length}]";
            int i = 0;
            foreach (var slot in Slots)
            {
                res += $"Slot {i++}: " + slot + "\n";
            }
            return res;
        }

        public IEnumerator<Slot> GetEnumerator()
        {
            return ((IEnumerable<Slot>)Slots).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Slots.GetEnumerator();
        }
    }
}