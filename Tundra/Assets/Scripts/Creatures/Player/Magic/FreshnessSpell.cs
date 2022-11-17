using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Creatures.Player.Magic
{
    /// <summary>
    /// A freshness spell class.
    /// </summary>
    [Spell("Freshness", "Warms up the player, provides regeneration boost and increases player's speed.", 
        new MagicElement[] {MagicElement.Explosion, MagicElement.Water, MagicElement.Magma})]
    [ElementRestrictions(MagicElement.Explosion | MagicElement.Magma | MagicElement.Water)]
    public class FreshnessSpell : Spell
    {
        private const int PrefabID = 2;

        /// <summary>
        /// Coefficient of player speed increasion.
        /// </summary>
        [IncreasableProperty(0.2, MagicElement.Explosion)]
        [IncreasableProperty(-0.1, MagicElement.Magma | MagicElement.Water)]
        public double PlayerSpeedCoefficient { get; set; } = 2;

        /// <summary>
        /// A coefficient of player warming.
        /// </summary>
        [IncreasableProperty(2, MagicElement.Magma)]
        [IncreasableProperty(-1, MagicElement.Explosion | MagicElement.Water)]
        public double PlayerWarmCoefficient { get; set; } = 10;

        /// <summary>
        /// A regeneration player coefficient.
        /// </summary>
        [IncreasableProperty(0.1, MagicElement.Water)]
        [IncreasableProperty(-0.05, MagicElement.Magma | MagicElement.Explosion)]
        public double RegenerationCoefficient { get; set; } = 1.1;

        /// <inheritdoc cref="Spell.Cast(GameObject, PlayerMagic)"/>
        public override void Cast(GameObject player, PlayerMagic magic)
        {
            Caster = player;
            var variableForPrefab = magic.GetSpellPrefabByID(PrefabID);
            var spellObject = UnityEngine.Object.Instantiate(variableForPrefab);
            spellObject.GetComponent<FreshnessScript>().Configuration = this;
        }
    }
}
