﻿using Creatures.Player.Behaviour;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBarController : MonoBehaviour
{
    GameObject _player;


    PlayerProperties playerProperties;
    Image imageComponent;

    // Start is called before the first frame update
    void Start()
    {
        _player = UIController._rootCanvas.GetComponent<UIController>()._player;
        imageComponent = GetComponent<Image>();
        playerProperties = _player.GetComponent<PlayerProperties>();
    }

    private void FixedUpdate()
    {
        transform.position = RectTransformUtility.WorldToScreenPoint(Camera.main, _player.transform.position) - new Vector2(0, 50);
        float displayValue = playerProperties.CurrentStamina / playerProperties.MaxStamina;
        imageComponent.fillAmount = displayValue;
    }
}