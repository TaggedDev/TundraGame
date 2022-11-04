﻿using Creatures.Player.Behaviour;
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

        private RecipeCofiguration[] _allRecipes;
        /// <summary>
        /// Full list of recipes existing in game.
        /// </summary>
        public IEnumerable<RecipeCofiguration> AllRecipes => _allRecipes;
        /// <summary>
        /// List of bassic recipes can be crafted from pocket craft mode.
        /// </summary>
        public IEnumerable<RecipeCofiguration> BasicRecipes => _allRecipes.Where(recipe => recipe.Workbench == null);
        /// <summary>
        /// Single instance of the <see cref="CraftHelper"/>.
        /// </summary>
        public static CraftHelper Instance 
        {
            get 
            { 
                if (_instance == null) _instance = new CraftHelper();
                return _instance;
            } 
        }

        private CraftHelper()
        {
            _allRecipes = Resources.FindObjectsOfTypeAll<RecipeCofiguration>();
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
