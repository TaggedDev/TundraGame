using Creatures.Player.Inventory;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Recipe Configuration", menuName = "Recipes/Recipe Configuration")]
public class RecipeCofiguration : ScriptableObject
{
    /// <summary>
    /// A result of the recipe. 
    /// </summary>
    [SerializeField]
    private GameObject result;
    /// <summary>
    /// Items requiredd for this recipe.
    /// </summary>
    [SerializeField]
    private List<RecipeComponent> requiredItems;
    /// <summary>
    /// A work space to craft this recipe.
    /// </summary>
    [SerializeField]
    private PlaceableItemConfiguration workbench;
}

[Serializable]
public struct RecipeComponent
{
    [SerializeField]
    private BasicItemConfiguration item;
    [SerializeField]
    private int amount;

    public BasicItemConfiguration Item => item;

    public int Amount => amount;
}