using UnityEngine;

namespace Creatures.Player.Inventory.ItemConfiguration
{
    [CreateAssetMenu(fileName = "New Equipment Configuration", menuName = "Items/Equipment/Equipment Configuration")]
    public class EquipmentConfiguration : BasicItemConfiguration
    {
        [SerializeField]
        private EquipmentSlotPosition _equipmentSlot;
        [SerializeField]
        private int _additionalSlots;
        [SerializeField]
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
        Backpack,
        /// <summary>
        /// Слот для книги/гримуара. Используется для книг заклинаний.
        /// </summary>
        Book
    }
}
