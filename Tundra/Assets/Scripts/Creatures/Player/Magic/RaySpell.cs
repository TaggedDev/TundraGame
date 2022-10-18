using Creatures.Player.Magic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Creatures.Player.Magic
{
    [Spell("Ray", "A magic ray which appears right in front of player. Maximal lifetime: 3 secs.", new MagicElement[] { MagicElement.Light, MagicElement.Explosion })]
    [ElementRestrictions(MagicElement.Light | MagicElement.Explosion)]
    public class RaySpell : Spell
    {
        [IncreasableProperty(4, MagicElement.Light)]
        public double Damage { get; set; } = 8;
        [IncreasableProperty(1.25, MagicElement.Explosion, IncreasablePropertyAttribute.IncreaseMode.Multiplication)]
        public double WidthCoefficient { get; set; } = 1;

        public override void Cast(GameObject player, PlayerMagic magic)
        {
            throw new NotImplementedException();
        }
    }
}
