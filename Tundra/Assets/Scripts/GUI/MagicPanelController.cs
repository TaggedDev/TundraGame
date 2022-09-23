using Creatures.Player.Magic;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class MagicPanelController : MonoBehaviour
{
    private PlayerMagic _playerMagic;

    // Start is called before the first frame update
    void Start()
    {
        _playerMagic = UIController._rootCanvas.GetComponent<UIController>()._player.GetComponent<PlayerMagic>();
        _playerMagic.MagicPanelVisibilityChange += SwitchVisibility;
    }

    private void SwitchVisibility(object sender, System.EventArgs e)
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void FixedUpdate()
    {
    }
}
