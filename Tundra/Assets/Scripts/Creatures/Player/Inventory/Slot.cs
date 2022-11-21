using System;
using System.Collections.Generic;
using Creatures.Player.Inventory.ItemConfiguration;
using UnityEngine;

namespace Creatures.Player.Inventory
{
    /// <summary>
    /// Класс, представляющий собой слот инвентаря для хранения предметов игрока.
    /// </summary>
    [Serializable]
    public class Slot
    {
        [SerializeField] private int itemsAmount;
        [SerializeField] private BasicItemConfiguration item;
        
        /// <summary>
        /// Creates a new slot with provided item configuration.
        /// </summary>
        /// <param name="itemsAmount">Amount of items.</param>
        /// <param name="item">An item configuration.</param>
        public Slot(int itemsAmount, BasicItemConfiguration item)
        {
            this.itemsAmount=itemsAmount;
            this.item=item;
        }

        public Slot()
        {

        }

        /// <summary>
        /// Indicates if the slot is empty.
        /// </summary>
        public bool IsEmpty => ItemsAmount == 0;

        /// <summary>
        /// Indicates if the slot is full.
        /// </summary>
        public bool IsFull => ItemsAmount == (item == null ? -1 : item.MaxStackVolume);

        /// <summary>
        /// An item inside the slot.
        /// </summary>
        public BasicItemConfiguration Item
        {
            get => item;
            private set
            {
                var cache = item;
                item=value;
                ItemChanged?.Invoke(this, cache);
            }
        }

        /// <summary>
        /// Amount of items inside the slot.
        /// </summary>
        public int ItemsAmount
        {
            get => itemsAmount;
            private set
            {
                itemsAmount=value;
                if (value == 0)
                {
                    item = null;
                }
            }
        }

        public event EventHandler<BasicItemConfiguration> ItemChanged;

        /// <summary>
        /// Clears the slot.
        /// </summary>
        public void Clear()
        {
            ItemsAmount = 0;
            Item = null;
        }
        
        /// <summary>
        /// Pushes items into the slot.
        /// </summary>
        /// <param name="item">An item to push.</param>
        /// <param name="amount">Amount of items.</param>
        /// <returns><see langword="true"/> if the item was successfully placed into the slot.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the slot capacity limit has been broken.</exception>
        /// <exception cref="ArgumentNullException">Throw if the <see cref="item"/> is <see langword="null"/>.</exception>
        public bool PushItem(BasicItemConfiguration item, int amount)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            if (amount > item.MaxStackVolume || amount < 0) throw new ArgumentOutOfRangeException(nameof(amount), "Amount of items to push inside the slot is more than maximum or less than zero.");
            if (ItemsAmount > 0 && Item.Title != item.Title) return false;
            Item = item;
            ItemsAmount = amount;
            return true;
        }

        /// <summary>
        /// Adds items into the non-empty slot.
        /// </summary>
        /// <param name="amount">Amount of items to add.</param>
        /// <returns>Remainder after items addition</returns>
        public int AddItems(int amount)
        {
            if (amount + ItemsAmount > Item.MaxStackVolume)
            {
                int rem = amount + ItemsAmount - Item.MaxStackVolume;
                ItemsAmount = Item.MaxStackVolume;
                return rem;
            }
            else ItemsAmount += amount;
            return 0;
        }

        /// <summary>
        /// Drops items into a world space.
        /// </summary>
        /// <param name="amount">Amounnt of items to drop.</param>
        /// <param name="position">Position in the world to drop into.</param>
        /// <param name="force">Force to throw an item (if its <see cref="GameObject"/> contains <see cref="Rigidbody"/>).</param>
        /// <exception cref="ArgumentOutOfRangeException">Throws if items amount is more than available.</exception>
        public List<GameObject> DropItems(int amount, Vector3 position, Vector3 force)
        {
            if (amount > ItemsAmount) throw new ArgumentOutOfRangeException(nameof(amount), "Amount of items to throw is more than amount of items inside the slot.");
            ItemsAmount -= amount;
            return Item.MassDrop(amount, position, force);
        }

        /// <summary>
        /// Drops one item
        /// </summary>
        /// <param name="position">Position in the world to drop into.</param>
        /// <param name="force">Force to throw an item (if its <see cref="GameObject"/> contains <see cref="Rigidbody"/>).</param>
        /// <exception cref="InvalidOperationException">Thrown if the slot is empty.</exception>
        public GameObject DropItem(Vector3 position, Vector3 force)
        {
            if (ItemsAmount == 0) throw new InvalidOperationException("Slot is empty.");
            var res = Item.Drop(position, force);
            ItemsAmount--;
            return res;
        }

        /// <summary>
        /// Removes items from the slot.
        /// </summary>
        /// <param name="amount">Amount of items.</param>
        /// <returns><see langword="true"/> if removing was successful.</returns>
        public bool RemoveItems(int amount)
        {
            if (amount > ItemsAmount) return false;
            ItemsAmount -= amount;
            return true;
        }

        /// <summary>
        /// Fills the slot with the given item.
        /// </summary>
        /// <param name="item">An item configuration.</param>
        /// <returns><see langword="true"/> if filling was successful.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the item configuration is <see langword="null"/>.</exception>
        public bool Fill(BasicItemConfiguration item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            if (Item != item && Item != null) return false;
            ItemsAmount = item.MaxStackVolume;
            if (Item == null) Item = item;
            return true;
        }

        public override string ToString()
        {
            return $"[{item.name}: {itemsAmount} / {item.MaxStackVolume}]";
        }
    }
}