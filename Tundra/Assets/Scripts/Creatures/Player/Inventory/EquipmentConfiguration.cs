using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Creatures.Player.Inventory
{
    [CreateAssetMenu(fileName = "New Equipment Configuration", menuName = "Items/Equipment Configuration")]
    public class EquipmentConfiguration : BasicItemConfiguration
    {
        private EquipmentSlotPosition _equipmentSlot;
        private int _additionalSlots;
        private float _warmEconomyCoefficient;

        /// <summary>
        /// Слот снаряжения, которому соответствует данный элемент экипировки.
        /// </summary>
        public EquipmentSlotPosition EquipmentSlot { get => _equipmentSlot; set => _equipmentSlot=value; }
        /// <summary>
        /// Число дополнительных слотов, которое даёт этот элемент экипировки.
        /// </summary>
        public int AdditionalSlots { get => _additionalSlots; set => _additionalSlots=value; }
        /// <summary>
        /// Коэффициент теплосбережения экипировки.
        /// </summary>
        public float WarmEconomyCoefficient { get => _warmEconomyCoefficient; set => _warmEconomyCoefficient=value; }

        public override GameObject Throw(Vector3 originPosition, Vector3 force)
        {
            //Пусть пока что будет делать ничего.
            //throw new InvalidOperationException("Throwing items is prohibited!");
            return null;
        }
    }
    /// <summary>
    /// Слот снаряжения, которому соответствует элемент экипировки.
    /// </summary>
    public enum EquipmentSlotPosition
    {
        /// <summary>
        /// Голова.
        /// </summary>
        /// <example>Шлемы, шапки и т.п.</example>
        Head,
        /// <summary>
        /// Тело.
        /// </summary>
        /// <example>Куртки и т.д.</example>
        Body,
        /// <summary>
        /// Ноги.
        /// </summary>
        /// <example>Штаны и пр.</example>
        Legs,
        /// <summary>
        /// Ступни.
        /// </summary>
        /// <example>Ботинки.</example>
        Feet,
        /// <summary>
        /// Шея.
        /// </summary>
        /// <example>Шарфы.</example>
        Neck,
        /// <summary>
        /// Слот для рюкзака. Он же тоже по сути должен считаться снаряжением.
        /// </summary>
        Backpack
    }
}
