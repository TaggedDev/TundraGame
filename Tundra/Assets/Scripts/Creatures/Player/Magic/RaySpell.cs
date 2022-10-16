using Creatures.Player.Magic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Creatures.Player.Magic
{
    [Spell("Ray", "A magic ray which appears right in front of player. Maximal lifetime: 3 secs.", new MagicElement[] { MagicElement.Light, MagicElement.Explosion })]
    [ElementRestrictions(MagicElement.Light | MagicElement.Explosion)]
    public class RaySpell : Spell
    {

    }
}
