using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Creatures.Player.Magic
{
    [Flags]
    public enum MagicElement
    {
        Empty = 0,
        Water = 1,
        Fire = 2,
        Air = 4,
        Ice = 8,
        Electro = 0x10,
        Ground = 0x20,
        Mood = 0x40,
        Sound = 0x80,
        Light = 0x100,
        Explosion = 0x200,
        Magma = 0x400,
        Crystal = 0x800,
    }
}
