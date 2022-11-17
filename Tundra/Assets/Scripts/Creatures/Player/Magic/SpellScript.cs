using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Creatures.Player.Magic
{
    /// <summary>
    /// Base class for spell scripts
    /// </summary>
    /// <typeparam name="T">Type of spell to control.</typeparam>
    public class SpellScript<T> : MonoBehaviour where T : Spell
    {
        /// <summary>
        /// Instance of spell properties.
        /// </summary>
        public T Configuration { get; set; }
    }
}
