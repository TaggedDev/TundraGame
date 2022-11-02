using Creatures.Player.Behaviour;
using Creatures.Player.Crafts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PocketCraftUI : MonoBehaviour
{
    private PlayerInventory _playerInventory;
    private CraftHelper _craftHelper;
    private Sprite[] _recipesImages;

    // Start is called before the first frame update
    void Start()
    {
        GameObject player = UIController._rootCanvas.GetComponent<UIController>()._player;
        _craftHelper = CraftHelper.Instance;
        _playerInventory = player.GetComponent<PlayerInventory>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
