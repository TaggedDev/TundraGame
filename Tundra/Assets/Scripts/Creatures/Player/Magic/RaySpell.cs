using Creatures.Player.Magic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Creatures.Player.Behaviour;
using UnityEngine;

namespace Creatures.Player.Magic
{
    /// <summary>
    /// The ray spell class.
    /// </summary>
    [Spell("Ray", "A magic ray which appears right in front of player. Maximal lifetime: 3 secs.", new MagicElement[] { MagicElement.Light, MagicElement.Explosion })]
    [ElementRestrictions(MagicElement.Light | MagicElement.Explosion)]
    public class RaySpell : Spell
    {
        private const int PrefabID = 0;

        /// <summary>
        /// Damage inflicted to all enemies in the hit area every second.
        /// </summary>
        [IncreasableProperty(4, MagicElement.Light)]
        public double Damage { get; set; } = 8;
        /// <summary>
        /// Width coefficient of the ray hit area.
        /// </summary>
        [IncreasableProperty(1.25, MagicElement.Explosion, IncreasablePropertyAttribute.IncreaseMode.Multiplication)]
        public double WidthCoefficient { get; set; } = 1;

        /// <inheritdoc cref="Spell.Cast(GameObject, PlayerMagic)"/>
        public override void Cast(GameObject player, PlayerMagic magic)
        {
            Caster = player;
            var variableForPrefab = magic.GetSpellPrefabByID(PrefabID);
            var spellObject = UnityEngine.Object.Instantiate(variableForPrefab);
            spellObject.GetComponent<RayScript>().Configuration = this;
        }
    }
}
