using Creatures.Player.Magic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Creatures.Player.Magic
{
    [AttributeUsage(AttributeTargets.Class)]
    public class SpellAttribute : Attribute
    {
        public string Name { get; }

        public string Description { get; }

        public MagicElement[] Elements { get; }

        public SpellAttribute(string name, string description, MagicElement[] elements)
        {
            Name=name;
            Description=description;
            Elements=elements;
        }
    }
}
