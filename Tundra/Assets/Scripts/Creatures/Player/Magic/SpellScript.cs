using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Creatures.Player.Magic
{
    public class SpellScript<T> : MonoBehaviour where T : Spell
    {
        public T Configuration { get; set; }
    }
}
