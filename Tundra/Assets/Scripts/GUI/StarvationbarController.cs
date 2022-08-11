using Creatures.Player.Behaviour;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GUI
{
    public class StarvationbarController : MonoBehaviour
    {
        // Properties
        /// <summary>
        /// Ссылка на компонент, отвечающий за голод игрока.
        /// </summary>
        private PlayerBehaviour playerStarvation => Player.GetComponent<PlayerBehaviour>();

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
        /// Трансформация основной полоски для отображения голода.
        /// </summary>
        private RectTransform _indicator;

        /// <summary>
        /// Величина, к которой будет стремиться полоска голода.
        /// </summary>
        private float _targetScale;
        /// <summary>
        /// Текущая величина полоски.
        /// </summary>
        private float _currentScale;


        // Start is called before the first frame update
        void Start()
        {
            _indicator = transform.Find("StarvationBarInner") as RectTransform;
            _currentScale = _indicator.localScale.x;
        }

        // Update is called once per frame
        void Update()
        {
            _targetScale = playerStarvation.CurrentStarveCapacity / playerStarvation.MaxStarve;
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
