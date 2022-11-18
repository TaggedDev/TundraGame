using UnityEngine;

namespace Creatures.Player.Inventory.ItemConfiguration
{
    [CreateAssetMenu(fileName = "New Melee Configuration", menuName = "Items/Melee Configuration")]
    public class MeleeWeaponConfiguration : WeaponConfiguration
    {
        [SerializeField]
        private float fullAttackLoadingTime;
        [SerializeField]
        private float minimalDamage;
        [SerializeField]
        private float maximalDamage;
        /// <summary>
        /// Время для 100% зарядки оружия.
        /// </summary>
        public float FullAttackLoadingTime { get => fullAttackLoadingTime; private set => fullAttackLoadingTime=value; }
        /// <summary>
        /// Урон при минимальном (1%) заряде оружия.
        /// </summary>
        public float MinimalDamage { get => minimalDamage; private set => minimalDamage=value; }
        /// <summary>
        /// Урон при максимальном (100%) заряде оружия.
        /// </summary>
        public float MaximalDamage { get => maximalDamage; private set => maximalDamage=value; }
    }
}
