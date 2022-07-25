using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Player.Inventory
{
    [CreateAssetMenu(fileName = "New Equipment Configuration", menuName = "Items")]
    public class EquipmentConfiguration : BasicItemConfiguration
    {
        private EquipmentSlotPosition equipmentSlot;
        private int additionalSlots;
        private float warmEconomyCoefficient;

        /// <summary>
        /// Слот снаряжения, которому соответствует данный элемент экипировки.
        /// </summary>
        public EquipmentSlotPosition EquipmentSlot { get => equipmentSlot; set => equipmentSlot=value; }
        /// <summary>
        /// Число дополнительных слотов, которое даёт этот элемент экипировки.
        /// </summary>
        public int AdditionalSlots { get => additionalSlots; set => additionalSlots=value; }
        /// <summary>
        /// Коэффициент теплосбережения экипировки.
        /// </summary>
        public float WarmEconomyCoefficient { get => warmEconomyCoefficient; set => warmEconomyCoefficient=value; }

        public override GameObject ThrowAway(Vector3 originPosition, Vector3 throwForce)
        {
            //Пусть пока что будет делать ничего.
            //throw new InvalidOperationException("Throwing items is prohibited!");
            return null;
        }

        public override List<GameObject> MassThrowAway(int amount, Vector3 originPosition, Vector3 throwForce)
        {
            //И тут тоже ничего.
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
        Neck
    }
}
