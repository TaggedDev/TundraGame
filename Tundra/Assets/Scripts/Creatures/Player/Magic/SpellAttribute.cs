using Creatures.Player.Magic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Creatures.Player.Magic
{
    /// <summary>
    /// An attribute to configure the spell.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class SpellAttribute : Attribute
    {
        /// <summary>
        /// Name of a spell.
        /// </summary>
        public string Name { get; }
        /// <summary>
        /// Description of a spell.
        /// </summary>
        public string Description { get; }
        /// <summary>
        /// List of elements represents formula to create a spell.
        /// </summary>
        public MagicElement[] Elements { get; }

        public SpellAttribute(string name, string description, MagicElement[] elements)
        {
            Name=name;
            Description=description;
            Elements=elements;
        }
    }
}
