using System;

namespace Creatures.Player.Magic
{
    public class ElementRestrictionsAttribute : Attribute
    {
        public MagicElement UsedElements { get; }

        public ElementRestrictionsAttribute(MagicElement usedElements)
        {
            UsedElements=usedElements;
        }
    }
}