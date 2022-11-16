using Creatures.Player.Behaviour;
using Creatures.Player.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Creatures.Player.Crafts
{
    public class CraftHelper
    {
        private static CraftHelper _instance;

        private RecipesListConfig _config;

        private List<RecipeCofiguration> _allRecipes;

        public bool AreAllRecipesLoaded { get; private set; }

        /// <summary>
        /// Full list of recipes existing in game.
        /// </summary>
        public IEnumerable<RecipeCofiguration> AllRecipes
        {
            get
            {
                if (_allRecipes == null) ReloadRecipes();
                return _allRecipes;
            }

            private set => _allRecipes = value.ToList();
        }

        /// <summary>
        /// List of bassic recipes can be crafted from pocket craft mode.
        /// </summary>
        public IEnumerable<RecipeCofiguration> BasicRecipes => AllRecipes.Where(recipe => recipe.Workbench == null);
        /// <summary>
        /// Single instance of the <see cref="CraftHelper"/>.
        /// </summary>
        public static CraftHelper Instance
        {
            get
            {
                return _instance ?? (_instance = new CraftHelper());
            }
        }

        private CraftHelper()
        {

        }
        /// <summary>
        /// Resets recipes list.
        /// </summary>
        /// <param name="config"></param>
        public void ResetRecipes(RecipesListConfig config)
        {
            SetConfig(config);
            ReloadRecipes();
        }
        /// <summary>
        /// Sets a configuration of available recipes.
        /// </summary>
        /// <param name="config">The configuration with current recipes list.</param>
        public void SetConfig(RecipesListConfig config)
        {
            _config = config;
        }
        /// <summary>
        /// Reloads all recipes from the configuration if it's not done yet.
        /// </summary>
        public void ReloadRecipes()
        {
            _allRecipes = new List<RecipeCofiguration>(_config.Recipes);
            AreAllRecipesLoaded = true;
        }

        /// <summary>
        /// Gets list of recipes available for this workbench type and which required items are contained in inventory.
        /// </summary>
        /// <param name="inventoryScript">Script which controls player's inventory.</param>
        /// <param name="workbench">A workbench instance in which player crafts an item.</param>
        /// <returns></returns>
        public IEnumerable<RecipeCofiguration> GetAvailableConfigurations(PlayerInventory inventoryScript, PlaceableItemConfiguration workbench)
        {
            var result = _allRecipes.Where(x => x.CheckIfAvailable(workbench, inventoryScript));
            return result;
        }
    }
}
