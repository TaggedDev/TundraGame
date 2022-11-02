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

        private RecipeCofiguration[] _allRecipes;

        public IEnumerable<RecipeCofiguration> AllRecipes => _allRecipes;

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

        public IEnumerable<RecipeCofiguration> GetAvailableConfigurations(PlayerInventory inventoryScript, PlaceableItemConfiguration workbench)
        {
            var result = _allRecipes.Where(x => x.Workbench == workbench && x.RequiredItems.All(y => inventoryScript.Inventory.CountItemOfTypeInTheInventory(y.Item) >= y.Amount));
            return result;
        }
    }
}
