using Creatures.Player.Behaviour;
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
    // Start is called before the first frame update
    void Start()
    {
        _player = UIController._rootCanvas.GetComponent<UIController>()._player;
        _playerProperties = _player.GetComponent<PlayerProperties>();
    }

    // Update is called once per frame
    void Update()
    {
        HitProgress.fillAmount = _playerProperties.CurrentHitProgress / _playerProperties.HitPreparationTime;
    }

    private void FixedUpdate()
    {
        transform.position = RectTransformUtility.WorldToScreenPoint(Camera.main, _player.transform.position) + new Vector2(0, 80);
    }
}
