using System;
using UnityEngine;

namespace Player.Inventory
{
    /// <summary>
    /// Класс, представляющий собой слот инвентаря для хранения предметов игрока.
    /// </summary>
    public class Slot
    {
        /// <summary>
        /// Максимальный объём предметов, которые могут поместиться в слоте.
        /// </summary>
        public const int MaxStackVolume = 5;

        private int _itemsAmount;
        private ItemConfiguration _item;
        /// <summary>
        /// Создаёт новый пустой слот.
        /// </summary>
        public Slot() 
        {

        }
        /// <summary>
        /// Создаёт новый слот с предметом внутри.
        /// </summary>
        /// <param name="itemsAmount">Количество предметов.</param>
        /// <param name="item">Сам по себе предмет.</param>
        public Slot(int itemsAmount, ItemConfiguration item)
        {
            _itemsAmount=itemsAmount;
            _item=item;
        }
        /// <summary>
        /// Указывает, является ли слот пустым.
        /// </summary>
        public bool IsEmpty => ItemsAmount == 0;
        /// <summary>
        /// Указывает, является ли слот заполненным.
        /// </summary>
        public bool IsFull => ItemsAmount == MaxStackVolume;
        /// <summary>
        /// Предмет, лежащий внутри слота.
        /// </summary>
        public ItemConfiguration Item 
        { 
            get => _item; 
            private set => _item=value; 
        }
        /// <summary>
        /// Количество предметов в слоте.
        /// </summary>
        public int ItemsAmount 
        { 
            get => _itemsAmount;
            private set
            {
                _itemsAmount=value;
                if (value == 0)
                {
                    _item = null;
                }
            }
        }

        /// <summary>
        /// Очищает слот от предметов.
        /// </summary>
        public void Clear()
        {
            ItemsAmount = 0;
            Item = null;
        }
        /// <summary>
        /// Помещает в слот указанное число предметов.
        /// </summary>
        /// <param name="item">Предмет, который нужно поместить в слот.</param>
        /// <param name="amount">Количество предметов.</param>
        /// <returns>Возвращет True в случае, если предмет удалось поместить в слот (слот был пустой или в нём был предмет того же типа).</returns>
        /// <exception cref="ArgumentOutOfRangeException">Возникает в случае превышения лимита вместимости слота.</exception>
        public bool PushItem(ItemConfiguration item, int amount)
        {
            if (amount > MaxStackVolume || amount < 0) throw new ArgumentOutOfRangeException(nameof(amount), "Amount of items to push inside the slot is more than maximum or less than zero.");
            if (ItemsAmount > 0 && Item.Title != item.Title) return false;
            Item = item;
            ItemsAmount = amount;
            return true;
        }
        /// <summary>
        /// Выбрасывает предметы из слота в окружающий мир.
        /// </summary>
        /// <param name="amount">Количество предметов, которое нужно выбросить.</param>
        /// <param name="position">Местоположение объекта, который будет выбрасывать предмет.</param>
        /// <param name="force">Сила и направление, с которыми будет выброшен предмет (при наличии у объекта предмета компонента <see cref="Rigidbody"/>).</param>
        /// <exception cref="ArgumentOutOfRangeException">Возникает в случае, если нет такого количества предметов.</exception>
        public void ThrowItems(int amount, Vector3 position, Vector3 force)
        {
            if (amount > ItemsAmount) throw new ArgumentOutOfRangeException(nameof(amount), "Amount of items to throw is more than amount of items inside the slot.");
            Item.MassThrowAway(amount, position, force);
            ItemsAmount -= amount;
        }
        /// <summary>
        /// Убирает предметы из слота в указанном количестве.
        /// </summary>
        /// <param name="amount">Количество предметов.</param>
        /// <returns>True в случае, если убрать получилось.</returns>
        public bool RemoveItems(int amount)
        {
            if (amount > ItemsAmount) return false;
            ItemsAmount -= amount;
            return true;
        }

        public bool Fill(ItemConfiguration item)
        {
            if (Item != item) return false;
            ItemsAmount = MaxStackVolume;
            if (Item == null) Item = item;
            return true;
        }
    }
}