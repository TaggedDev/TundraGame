using Creatures.Player.Behaviour;
using Creatures.Player.Crafts;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PocketCraftUI : MonoBehaviour
{
    private PlayerInventory _playerInventory;
    private CraftHelper _craftHelper;
    private Sprite[] _recipesImages;
    private RecipeCofiguration[] _basicRecipes;
    private int _selectedRecipe;

    private int SelectedRecipe 
    {
        get => _selectedRecipe;
        set
        {
            if (_selectedRecipe != value)
            {
                _selectedRecipe = value;
                SelectedRecipeIndexChanged();
            }
        }
    }

    private void SelectedRecipeIndexChanged()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        GameObject player = UIController._rootCanvas.GetComponent<UIController>()._player;
        _craftHelper = CraftHelper.Instance;
        _playerInventory = player.GetComponent<PlayerInventory>();
        _basicRecipes = _craftHelper.BasicRecipes.OrderBy(x => x.name).ToArray();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
