using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    static internal PocketCraftUI _pocketCraftUI;
    static internal GameObject _rootCanvas;


    [SerializeField] internal GameObject _player;

    private void Awake()
    {
        _rootCanvas = gameObject;
        _pocketCraftUI = GetComponentInChildren<PocketCraftUI>();
        _pocketCraftUI.gameObject.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }
}
