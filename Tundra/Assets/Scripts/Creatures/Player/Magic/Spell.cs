using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace Creatures.Player.Magic
{
    public abstract class Spell
    {
        private MagicElement allowedElements;

        internal static List<Type> allSpells;

        public MagicElement AllowedMagicElements => allowedElements;

        static Spell()
        {
            var spellType = typeof(Spell);
            allSpells = (from type in Assembly.GetExecutingAssembly().GetTypes()
                         where spellType.IsAssignableFrom(type)
                         select type).ToList();
        }

        public Spell()
        {
            allowedElements = GetType().GetAttribute<ElementRestrictionsAttribute>().UsedElements;
        }

        /// <summary>
        /// Finds spells that passes <see cref="MagicElement"/> restcrictions.
        /// </summary>
        /// <param name="filter">Filter to find elements with given requirements.</param>
        /// <param name="spells">List of spells to find between them. If not defined, in will find in all spells in this assembly.</param>
        /// <returns>List of spells which formula can be compatible with given filter.</returns>
        public static List<Type> FindSpellTypes(List<MagicElement> filter, List<Type> spells = null)
        {
            spells = spells ?? allSpells;
            var selectedTypes = (from x in spells
                                 let desc = x.GetAttribute<SpellAttribute>()
                                 where filter.Take(desc.Elements.Length).SequenceEqual(desc.Elements)
                                 orderby desc.Elements.Length descending
                                 select x);
            return selectedTypes.ToList();
        }

        /// <summary>
        /// Build a spell to get it ready for cast. Created spell will get all modifiers given by reagents.
        /// </summary>
        /// <param name="elements">List of elements which are used to make a spell. In the start of list are needed elements, after them are reagents.</param>
        public abstract void Build(List<MagicElement> elements);

        /// <summary>
        /// Initiates a spell casting.
        /// </summary>
        /// <param name="player">Requires player object to get spell initial position and parameters to cast.</param>
        /// <param name="magic">The <see cref="PlayerMagic"/> instance which has casted this spell.</param>
        public abstract void Cast(GameObject player, PlayerMagic magic);
    }
}
