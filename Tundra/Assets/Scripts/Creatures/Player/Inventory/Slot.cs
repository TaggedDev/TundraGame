using System;
using System.Collections.Generic;
using UnityEngine;

namespace Creatures.Player.Inventory
{
    /// <summary>
    /// Класс, представляющий собой слот инвентаря для хранения предметов игрока.
    /// </summary>
    [Serializable]
    public class Slot
    {
        [SerializeField]
        private int itemsAmount;
        [SerializeField]
        private BasicItemConfiguration item;
        /// <summary>
        /// Создаёт новый слот с предметом внутри.
        /// </summary>
        /// <param name="itemsAmount">Количество предметов.</param>
        /// <param name="item">Сам по себе предмет.</param>
        public Slot(int itemsAmount, BasicItemConfiguration item)
        {
            this.itemsAmount=itemsAmount;
            this.item=item;
        }

        public Slot()
        {

        }

        /// <summary>
        /// Указывает, является ли слот пустым.
        /// </summary>
        public bool IsEmpty => ItemsAmount == 0;
        /// <summary>
        /// Указывает, является ли слот заполненным.
        /// </summary>
        public bool IsFull => ItemsAmount == (item == null ? -1 : item.MaxStackVolume);
        /// <summary>
        /// Предмет, лежащий внутри слота.
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
        /// Количество предметов в слоте.
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
        /// <exception cref="ArgumentNullException">Возникает в случае, если <see cref="item"/> имеет значение null.</exception>
        public bool PushItem(BasicItemConfiguration item, int amount)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            if (amount > item.MaxStackVolume || amount < 0) throw new ArgumentOutOfRangeException(nameof(amount), "Amount of items to push inside the slot is more than maximum or less than zero.");
            if (ItemsAmount > 0 && Item.Title != item.Title) return false;
            Item = item;
            ItemsAmount = amount;
            return true;
        }

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
        /// Выбрасывает предметы из слота в окружающий мир.
        /// </summary>
        /// <param name="amount">Количество предметов, которое нужно выбросить.</param>
        /// <param name="position">Местоположение объекта, который будет выбрасывать предмет.</param>
        /// <param name="force">Сила и направление, с которыми будет выброшен предмет (при наличии у объекта предмета компонента <see cref="Rigidbody"/>).</param>
        /// <exception cref="ArgumentOutOfRangeException">Возникает в случае, если нет такого количества предметов.</exception>
        public List<GameObject> DropItems(int amount, Vector3 position, Vector3 force)
        {
            if (amount > ItemsAmount) throw new ArgumentOutOfRangeException(nameof(amount), "Amount of items to throw is more than amount of items inside the slot.");
            ItemsAmount -= amount;
            return Item.MassDrop(amount, position, force);
        }

        public GameObject DropItem(Vector3 position, Vector3 force)
        {
            if (ItemsAmount == 0) throw new InvalidOperationException("Недостаточно предметов, чтобы выбросить.");
            var res = Item.Drop(position, force);
            ItemsAmount--;
            return res;
        }
        /// <summary>
        /// Бросает предмет из слота в противника.
        /// </summary>
        /// <param name="position">Местоположение игрока (чей инвентарь).</param>
        /// <param name="target">Направление, по которому нужно сделать бросок.</param>
        /// <returns>Брошенный объект.</returns>
        /// <exception cref="NotImplementedException">Пока не реализовано, т.к. нужно написать логику для брошенного предмета, а это вряд ли получится сделать сейчас.</exception>
        public GameObject ThrowItem(Vector3 position, Vector3 target)//TODO: возможно придётся несколько переделать после нормальной реализации броска предмета. 
        {
            if (ItemsAmount == 0) throw new InvalidOperationException("Недостаточно предметов, чтобы кинуть.");
            var res = Item.Drop(position, target * 20);
            ItemsAmount--;
            return res;
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

        /// <summary>
        /// Заполняет слот указаннными предметами.
        /// </summary>
        /// <param name="item">Предмет, которым производится заполнение.</param>
        /// <returns>True в случае, если получилось заполнить слот.</returns>
        /// <exception cref="ArgumentNullException"></exception>
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