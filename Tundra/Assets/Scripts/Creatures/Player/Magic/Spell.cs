using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Creatures.Player.Magic
{
    /// <summary>
    /// A base class of all spells. It provides functionality to spell initializaton logic.
    /// </summary>
    public abstract class Spell
    {
        private readonly int _descFormulaLength;

        private static List<Type> s_allSpells;

        public MagicElement AllowedMagicElements { get; }

        public GameObject Caster { get; protected set; }

        static Spell()
        {
            var spellType = typeof(Spell);
            s_allSpells = (from type in Assembly.GetExecutingAssembly().GetTypes()
                         where spellType.IsAssignableFrom(type) && type != typeof(Spell)
                         select type).ToList();
        }

        public Spell()
        {
            AllowedMagicElements = GetType().GetCustomAttribute<ElementRestrictionsAttribute>().UsedElements;
            _descFormulaLength = GetType().GetCustomAttribute<SpellAttribute>().Elements.Length;
        }

        /// <summary>
        /// Checks if spell valid with given formula.
        /// </summary>
        /// <param name="elements">List of elements to check.</param>
        /// <returns></returns>
        protected bool CheckValidity(List<MagicElement> elements)
        {
            var desc = GetType().GetCustomAttribute<SpellAttribute>();
            if (desc == null) return false;
            if (elements.Take(_descFormulaLength).SequenceEqual(desc.Elements))
            {
                if (elements.Skip(_descFormulaLength).All(x => AllowedMagicElements.HasFlag(x))) return true;
            }
            return false;
        }

        /// <summary>
        /// Finds spells that passes <see cref="MagicElement"/> restcrictions.
        /// </summary>
        /// <param name="filter">Filter to find elements with given requirements.</param>
        /// <param name="spells">List of spells to find between them. If not defined, in will find in all spells in this assembly.</param>
        /// <returns>List of spells which formula can be compatible with given filter.</returns>
        public static List<Type> FindSpellTypes(List<MagicElement> filter, List<Type> spells = null)
        {
            if (filter == null) return s_allSpells;
            spells = spells ?? s_allSpells;
            var selectedTypes = (from x in spells
                                 let desc = x.GetCustomAttribute<SpellAttribute>()
                                 let constraints = x.GetCustomAttribute<ElementRestrictionsAttribute>()
                                 where desc.Elements.Take(filter.Count).SequenceEqual(filter) || constraints != null && filter.All(x => constraints.UsedElements.HasFlag(x))
                                 orderby desc.Elements.Length descending
                                 select x);
            return selectedTypes.ToList();
        }

        /// <summary>
        /// Build a spell to get it ready for cast. Created spell will get all modifiers given by reagents.
        /// </summary>
        /// <param name="elements">List of elements which are used to make a spell. In the start of list are needed elements, after them are reagents.</param>
        public virtual void Build(List<MagicElement> elements)
        {
            if (!CheckValidity(elements)) throw new ArgumentException("Some of elements are prohibited for this spell!", nameof(elements));
            var reagents = elements.Skip(_descFormulaLength);
            if (reagents.Count() == 0) return;
            foreach (var prop in GetType().GetProperties())
            {
                var attributes = prop.GetCustomAttributes<IncreasablePropertyAttribute>();
                if (attributes.Count() != 0)
                {
                    foreach (var attribute in attributes)
                    {
                        foreach (var reagent in reagents)
                        {
                            attribute.IncreaseValue(this, prop, reagent);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Initiates a spell casting.
        /// </summary>
        /// <param name="player">Requires player object to get spell initial position and parameters to cast.</param>
        /// <param name="magic">The <see cref="PlayerMagic"/> instance which has casted this spell.</param>
        public abstract void Cast(GameObject player, PlayerMagic magic);
    }
}
