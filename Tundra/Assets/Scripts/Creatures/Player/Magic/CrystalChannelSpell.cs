using Creatures.Player.Magic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Creatures.Player.Magic
{
    [Spell("Crystal Channel", 
        "Creates a waterflow around the player to proptect him from attacks. It increases regeneration and attacks mobs in nearby area.", 
        new MagicElement[] {MagicElement.Water, MagicElement.Crystal } )]
    [ElementRestrictions(MagicElement.Water | MagicElement.Crystal)]
    public class CrystalChannelSpell : Spell
    {
        [IncreasableProperty(0.25, MagicElement.Crystal)]
        public double MovementFineCoefficient { get; set; } = 0.5;
        [IncreasableProperty(10, 1.1, MagicElement.Water)]
        public double Regeneration { get; set; } = 20;

        public double DamageCoefficient { get; set; } = 0.1;

        public override void Cast(GameObject player, PlayerMagic magic)
        {
            throw new NotImplementedException();
        }
    }
}
