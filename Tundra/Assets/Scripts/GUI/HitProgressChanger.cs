﻿using Creatures.Player.Behaviour;
using Creatures.Player.Inventory;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HitProgressChanger : MonoBehaviour
{

    public Image HitProgress;
    private PlayerProperties _playerProperties;
    private PlayerInventory _playerInventory;
    private GameObject _player;

    private void Start()
    {
        _player = UIController._rootCanvas.GetComponent<UIController>()._player;
        _playerProperties = _player.GetComponent<PlayerProperties>();
        _playerInventory = _player.GetComponent<PlayerInventory>();
    }

    private void Update()
    {
        HitProgress.fillAmount = _playerProperties.CurrentHitProgress / (_playerInventory.SelectedItem as MeleeWeaponConfiguration).FullWindupTime;
    }

    private void FixedUpdate()
    {
        transform.position = RectTransformUtility.WorldToScreenPoint(Camera.main, _player.transform.position) + new Vector2(0, 80);
    }
}
