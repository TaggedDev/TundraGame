using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Creatures.Player.Magic
{
    [Spell("Freshness", "Warms up the player, provides regeneration boost and increases player's speed.", 
        new MagicElement[] {MagicElement.Explosion, MagicElement.Magma, MagicElement.Water})]
    [ElementRestrictions(MagicElement.Explosion | MagicElement.Magma | MagicElement.Water)]
    public class FreshnessSpell : Spell
    {
        [IncreasableProperty(0.2, MagicElement.Explosion)]
        [IncreasableProperty(-0.1, MagicElement.Magma | MagicElement.Water)]
        public double PlayerSpeedCoefficient { get; set; } = 2;

        [IncreasableProperty(2, MagicElement.Magma)]
        [IncreasableProperty(-1, MagicElement.Explosion | MagicElement.Water)]
        public double PlayerWarmCoefficient { get; set; } = 10;

        [IncreasableProperty(0.1, MagicElement.Water)]
        [IncreasableProperty(-0.05, MagicElement.Magma | MagicElement.Explosion)]
        public double RegenerationCoefficient { get; set; } = 1.1;

        public override void Cast(GameObject player, PlayerMagic magic)
        {
            throw new NotImplementedException();
        }
    }
}
