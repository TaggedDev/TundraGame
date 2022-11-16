using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Creatures.Player.Crafts
{
    /// <summary>
    /// A class which contains recipes list. It's needed only because Unity doesn't see scriptable objects until they're loaded into scene.
    /// </summary>
    [CreateAssetMenu(fileName = "New recipes list", menuName = "Recipes/Recipe List")]
    public class RecipesListConfig : ScriptableObject
    {
        [SerializeField]
        private RecipeCofiguration[] recipes;
        /// <summary>
        /// List of recipes in this player configuration.
        /// </summary>
        public ReadOnlyCollection<RecipeCofiguration> Recipes => Array.AsReadOnly(recipes);

        public RecipesListConfig()
        {
            var helper = CraftHelper.Instance;
            if (!helper.AreAllRecipesLoaded) helper.SetConfig(this);
        }
    }
}
