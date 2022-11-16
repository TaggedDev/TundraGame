using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Creatures.Player.Inventory
{
    [CreateAssetMenu(fileName = "New Melee Configuration", menuName = "Items/Melee Configuration")]
    public class MeleeWeaponConfiguration : WeaponConfiguration
    {
        [SerializeField]
        private float fullAttackLoadingTime;
        [SerializeField]
        private float _damage;
        /// <summary>
        /// Время для 100% зарядки оружия.
        /// </summary>
        public float FullWindupTime { get => fullAttackLoadingTime; private set => fullAttackLoadingTime=value; }
        /// <summary>
        /// Урон при максимальном (100%) заряде оружия.
        /// </summary>
        public float Damage { get => _damage; private set => _damage=value; }
    }
}
