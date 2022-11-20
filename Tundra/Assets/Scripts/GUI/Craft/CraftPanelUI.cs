using Creatures.Player.Behaviour;
using Creatures.Player.Crafts;
using Creatures.Player.Inventory;
using Creatures.Player.States;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GUI.HeadUpDisplay
{
    /// <summary>
    /// A class to control craft panel.
    /// </summary>
    public class CraftPanelUI : MonoBehaviour
    {
        [SerializeField] private GameObject recipePrefab;
        [SerializeField] private GameObject contentObject;
        [SerializeField] private PlayerInventory playerInventory;
        [SerializeField] private bool areAllRecipesVisible;

        private GameObject[] _recipeTiles;
        private PlaceableItemConfiguration _currentWorkspace;
        private bool _loaded;

        private void Start()
        {
            UIController.CraftPanel = this;
            ClosePanel();
            _loaded = true;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                ClosePanel();
            }
        }

        /// <summary>
        /// Closes the panel.
        /// </summary>
        public void ClosePanel()
        {
            _currentWorkspace = null;
            gameObject.SetActive(false);
        }

        /// <summary>
        /// Opens the panel to show recipes.
        /// </summary>
        /// <param name="workspace">Workspace to filter recipes.</param>
        public void ShowPanel(PlaceableItemConfiguration workspace)
        {
            _currentWorkspace = workspace;
            gameObject.SetActive(true);
            ReloadRecipesList();
        }

        /// <summary>
        /// Reloads recipes list.
        /// </summary>
        private void ReloadRecipesList()
        {
            // Stage 1. Prepare recipes list.
            var recipes = CraftHelper.Instance.GetAvailableConfigurations(null, _currentWorkspace);
            if (!areAllRecipesVisible)
            {
                recipes = CraftHelper.Instance.GetAvailableConfigurations(playerInventory, _currentWorkspace);
            }
            // Stage 2. Remove old recipe tiles.
            if (_recipeTiles != null)
            {
                foreach (var tile in _recipeTiles)
                {
                    Destroy(tile);
                }
            }
            // Stage 3. Fill list with new tiles.
            _recipeTiles = new GameObject[recipes.Count()];
            if (recipes.Count() == 0) return;
            int i = 0;
            foreach (var recipe in recipes)
            {
                var tile = Instantiate(recipePrefab, contentObject.transform);
                _recipeTiles[i++] = tile;
                bool isAvailable = !areAllRecipesVisible || recipe.CheckIfAvailable(_currentWorkspace, playerInventory);
                var tileUI = tile.GetComponent<CraftTileUI>();
                tileUI.SetRecipe(recipe, playerInventory, isAvailable);
                tileUI.RecipeCrafted += (_, __) =>
                {
                    playerInventory.gameObject.GetComponent<PlayerBehaviour>().SwitchState<IdlePlayerState>();
                    ClosePanel();
                };
            }
        }

        /// <summary>
        /// A handler of the checkbox selection changed event in Unity.
        /// </summary>
        /// <param name="value">New checkbox value.</param>
        public void SelectionChanged(bool value)
        {
            areAllRecipesVisible = !value;
            if (_loaded) ReloadRecipesList();
        }
    }
}