﻿using Creatures.Player.Behaviour;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GUI
{
    public class TemperatureIndicatorController : MonoBehaviour
    {

        // Properties
        /// <summary>
        /// Ссылка на компонент, отвечающий за температуру игрока.
        /// </summary>
        private PlayerBehaviour PlayerBehaviour => Player.GetComponent<PlayerBehaviour>();

        // Public fields
        /// <summary>
        /// Ссылка на игрока.
        /// </summary>
        public GameObject Player;
        /// <summary>
        /// Ссылка на холст, нужна для получения данных о масштабировании холста.
        /// </summary>
        public Canvas Canvas;

        /// <summary>
        /// Модификатор, определяющий скорость анимации. 
        /// <!-- главное – не переборщить со скоростью, а то эта полоска может начать туда-сюда скакать.
        /// Но с нормальными величинами всё нормально будет. -->
        /// </summary>
        [SerializeField]
        private float animationSpeedModifier = 1f;

        // Private fields
        /// <summary>
        /// Трансформация основной полоски для отображения температуры.
        /// </summary>
        private RectTransform _indicator;

        /// <summary>
        /// Величина, к которой будет стремиться полоска температуры.
        /// </summary>
        private float _targetScale;
        /// <summary>
        /// Текущая величина полоски.
        /// </summary>
        private float _currentScale;


        // Start is called before the first frame update
        void Start()
        {
            _indicator = transform.Find("HeatBarInner") as RectTransform;
            _currentScale = _indicator.localScale.x;
        }

        // Update is called once per frame
        void Update()
        {
            _targetScale = PlayerBehaviour.CurrentWarmLevel / PlayerBehaviour.MaxWarmLevel;
            float deltaScaleValue = (float)Math.Round((_targetScale - _currentScale), 3) * animationSpeedModifier * Time.deltaTime;
            if (Math.Abs(deltaScaleValue) < 0.00002)
            {
                deltaScaleValue = 0;
                _currentScale = _targetScale;
            }
            _indicator.localScale = new Vector3(deltaScaleValue + _currentScale, _indicator.localScale.y, _indicator.localScale.z);
            _currentScale = _indicator.localScale.x;
        }
    }
}
