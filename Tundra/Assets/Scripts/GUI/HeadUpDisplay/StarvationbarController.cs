using System;
using Creatures.Player.Behaviour;
using GUI.HeadUpDisplay;
using Creatures.Player.Behaviour;
using UnityEngine;

namespace GUI.HeadUpDisplay
{
    public class StarvationbarController : MonoBehaviour
    {
        // Properties
        /// <summary>
        /// Ссылка на компонент, отвечающий за голод игрока.
        /// </summary>
        private PlayerProperties PlayerProperties => _player.GetComponent<PlayerProperties>();

        // Public fields
        /// <summary>
        /// Ссылка на игрока.
        /// </summary>
        private GameObject _player;

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
        
        private void Start()
        {
            UIController controller = UIController.RootCanvas.GetComponent<UIController>();
            _player = controller.Player;
            _indicator = transform.Find("StarvationBarInner") as RectTransform;
            _currentScale = _indicator.localScale.x;
        }
        
        private void Update()
        {
            _targetScale = PlayerProperties.CurrentStarvePoints / PlayerProperties.MaxStarvePoints;
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
