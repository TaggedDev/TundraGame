using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Creatures.Player.Inventory
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
