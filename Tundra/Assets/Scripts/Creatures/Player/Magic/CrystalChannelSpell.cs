using Creatures.Player.Behaviour;
using UnityEngine;

namespace Creatures.Player.Magic
{
    /// <summary>
    /// A spell of crystal channel.
    /// </summary>
    [Spell("Crystal Channel",
        "Creates a waterfall around the player to protect him from attacks. It increases regeneration and attacks mobs in nearby area.",
        new[] { MagicElement.Water, MagicElement.Crystal })]
    [ElementRestrictions(MagicElement.Water | MagicElement.Crystal)]
    public class CrystalChannelSpell : Spell
    {
        private const int PrefabID = 1;
        /// <summary>
        /// A coefficient of player speed decreasing.
        /// </summary>
        [IncreasableProperty(0.25, MagicElement.Crystal)]
        public double MovementFineCoefficient { get; set; } = 0.5;
        /// <summary>
        /// A regeneration value.
        /// </summary>
        [IncreasableProperty(10, 1.1, MagicElement.Water)]
        public double Regeneration { get; set; } = 20;
        /// <summary>
        /// A coefficient of damage that enemies take back from the player.
        /// </summary>
        public double DamageCoefficient { get; set; } = 0.1;

        /// <inheritdoc cref="Spell.Cast(GameObject, PlayerMagic)"/>
        public override void Cast(GameObject player, PlayerMagic magic)
        {
            Caster = player;
            var variableForPrefab = magic.GetSpellPrefabByID(PrefabID);
            var spellObject = Object.Instantiate(variableForPrefab);
            spellObject.GetComponent<CrystalChannelScript>().Configuration = this;
        }
    }
}
