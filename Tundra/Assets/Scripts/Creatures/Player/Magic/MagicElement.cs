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
        Air = 1,
        Fire = 2,
        Ground = 4,
        Water = 8,
    }
}
