using System;

namespace Creatures.Player.Magic
{
    /// <summary>
    /// Provides constraints to 
    /// </summary>
    public class ElementRestrictionsAttribute : Attribute
    {
        /// <summary>
        /// Elements used to create a spell.
        /// </summary>
        public MagicElement UsedElements { get; }

        public ElementRestrictionsAttribute(MagicElement usedElements)
        {
            UsedElements=usedElements;
        }
    }
}