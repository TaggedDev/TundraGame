﻿using Creatures.Player.Behaviour;
using Creatures.Player.Crafts;
using Creatures.Player.Inventory;
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
        [SerializeField]
        private GameObject recipePrefab;
        [SerializeField]
        private GameObject contentObjectPrefab;
        [SerializeField]
        private PlayerInventory playerInventory;
        [SerializeField]
        private bool areAllRecipesVisible;

        private GameObject[] _recipeTiles;
        private PlaceableItemConfiguration _currentWorkspace;

        private void Start()
        {
            UIController.CraftPanel = this;
            ClosePanel();
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
            var recipes = CraftHelper.Instance.AllRecipes;
            if (!areAllRecipesVisible)
            {
                recipes = from recipe in recipes
                          where recipe.CheckIfAvailable(_currentWorkspace, playerInventory)
                          select recipe;
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
            int i = 0;
            var rect = (recipePrefab.transform as RectTransform).rect;
            Vector3 position = new Vector3(rect.width / 2, 0);
            foreach (var recipe in recipes)
            {
                var tile = Instantiate(recipePrefab, contentObjectPrefab.transform);
                _recipeTiles[i++] = tile;
                tile.transform.position = position;
                position.y -= rect.height + 10;
                tile.GetComponent<CraftTileUI>().SetRecipe(recipe);
            }
            //var size = (contentObjectPrefab.transform as RectTransform).sizeDelta;
            //(contentObjectPrefab.transform as RectTransform).sizeDelta = new Vector2(size.x, Mathf.Abs(position.y));
        }
    }
}