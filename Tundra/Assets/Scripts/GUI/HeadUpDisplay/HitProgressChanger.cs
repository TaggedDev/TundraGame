using Creatures.Player.Behaviour;
using GUI.HeadUpDisplay;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HitProgressChanger : MonoBehaviour
{

    public Image HitProgress;

    private GameObject _player;
    private PlayerProperties _playerProperties;

    private  void Start()
    {
        _player = UIController.RootCanvas.GetComponent<UIController>().Player;
        _playerProperties = _player.GetComponent<PlayerProperties>();
    }

    private void Update()
    {
        HitProgress.fillAmount = _playerProperties.CurrentHitProgress / _playerProperties.HitPreparationTime;
    }

    private void FixedUpdate()
    {
        transform.position = RectTransformUtility.WorldToScreenPoint(Camera.main, _player.transform.position) + new Vector2(0, 80);
    }
}
