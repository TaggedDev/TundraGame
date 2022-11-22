using UnityEngine;

namespace Creatures.Player.Inventory.ItemConfiguration
{
    /// <summary>
    /// Базовый класс для конфигураций оружий ближнего и дальнего боя.
    /// </summary>
    public abstract class WeaponConfiguration : BasicItemConfiguration
    {
        [SerializeField]
        private int throwDamage;

        public int ThrowDamage { get => throwDamage; private set => throwDamage=value; }
    }
}
