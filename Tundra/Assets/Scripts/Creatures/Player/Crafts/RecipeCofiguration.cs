using Creatures.Player.Behaviour;
using Creatures.Player.Crafts;
using Creatures.Player.Inventory;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "New Recipe Configuration", menuName = "Recipes/Recipe Configuration")]
public class RecipeCofiguration : ScriptableObject
{
    /// <summary>
    /// A result of the recipe. 
    /// </summary>
    [SerializeField]
    private BasicItemConfiguration result;
    /// <summary>
    /// Items required for this recipe.
    /// </summary>
    [SerializeField]
    private List<RecipeComponent> requiredItems;
    /// <summary>
    /// A work space to craft this recipe.
    /// </summary>
    [SerializeField]
    private PlaceableItemConfiguration workbench;
    /// <summary>
    /// A result of the recipe. 
    /// </summary>
    public BasicItemConfiguration Result => result;
    /// <summary>
    /// Items required for this recipe.
    /// </summary>
    public ReadOnlyCollection<RecipeComponent> RequiredItems => requiredItems.AsReadOnly();
    /// <summary>
    /// A work space to craft this recipe.
    /// </summary>
    public PlaceableItemConfiguration Workbench => workbench;

    public RecipeCofiguration()
    {

    }

    /// <summary>
    /// Checks if this recipe available for current selection filter.
    /// </summary>
    /// <param name="workbench">A workbanech to craft this recipe.</param>
    /// <param name="inv">An inventory script which controls player's inventory.</param>
    /// <returns><see langword="true"/> if this recipe is available to craft, <see langword="false"/> otherwise.</returns>
    public bool CheckIfAvailable(PlaceableItemConfiguration workbench, PlayerInventory inv = null)
    {
        return workbench == this.workbench && (inv == null || requiredItems.All(x => inv.Inventory.CountItemOfTypeInTheInventory(x.Item) >= x.Amount));
    }
    /// <summary>
    /// Does a craft for this recipe.
    /// </summary>
    /// <param name="inventoryScript">Player inventory script which controls inventory.</param>
    /// <returns><see langword="true"/> if craft was done successfully, <see langword="false"/> otherwise.</returns>
    public bool Craft(PlayerInventory inventoryScript, out int resultSlot)
    {
        if (!CheckIfAvailable(workbench, inventoryScript))
        {
            resultSlot = -1;
            return false;
        }
        // Removes from inventory as much items as required in the craft.
        foreach (var component in requiredItems)
        {
            int remainder = component.Amount;
            while (remainder > 0)
            {
                var slot = inventoryScript.Inventory.Slots.Last(x => x.Item == component.Item);
                int items = slot.ItemsAmount;
                if (remainder >= items)
                {
                    slot.Clear();
                    remainder -= items;
                }
                else
                {
                    slot.RemoveItems(remainder);
                    remainder = 0;
                }
            }
        }
        // Adds an item to player's inventory or drops it if player's inventory is full.
        inventoryScript.Inventory.AddItem(result, 1, out int rem);
        if (rem == 1)
        {
            resultSlot = -1;
            result.Drop(inventoryScript.transform.position, Vector3.up);
        }
        else resultSlot = Array.FindLastIndex(inventoryScript.Inventory.Slots, x => x.Item == result);
        return true;
    }
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