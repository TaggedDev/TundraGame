﻿using Creatures.Player.Behaviour;
using UnityEngine;
using UnityEngine.UI;

namespace GUI
{
    /// <summary>
    /// This class manages circle bar above the player
    /// </summary>
    public class CircleBarChanger : MonoBehaviour
    {
        [SerializeField] private Image hitProgress;
        private GameObject _player;
        private PlayerProperties _playerProperties;
        private Camera _mainCamera;
        
        private void Start()
        {
            _mainCamera = Camera.main;
            _player = UIController._rootCanvas.GetComponent<UIController>()._player;
            _playerProperties = _player.GetComponent<PlayerProperties>();
        }
        
        private void Update()
        {
            hitProgress.fillAmount = _playerProperties.CurrentCircleBarFillingTime / _playerProperties.MaxCircleBarFillingTime;
        }

        private void FixedUpdate()
        {
            transform.position = RectTransformUtility.WorldToScreenPoint(
                _mainCamera, _player.transform.position) + new Vector2(0, 80);
        }
    }
}