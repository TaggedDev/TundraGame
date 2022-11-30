using System;
using Creatures.Player.Behaviour;
using UnityEngine;

namespace GUI.HeadUpDisplay
{
    public class HealthBarController : MonoBehaviour
    {
        // Properties
        
        /// <summary>
        /// Health component link
        /// </summary>
        private PlayerProperties PlayerProperties => _player.GetComponent<PlayerProperties>();
        
        /// <summary>
        /// Global start position of this indicator start
        /// </summary>
        private Vector3 HealthBarStart => transform.position - transform.rotation * new Vector3(ActualWidth / 2, 0, 0);
        
        /// <summary>
        /// An actual width of indicator in game
        /// </summary>
        private float ActualWidth => (transform as RectTransform).sizeDelta.x * _rootCanvas.scaleFactor;

        // Public fields
        private Canvas _rootCanvas;
        private GameObject _player;
        
        /// <summary>
        /// A modifier to change the animation speed 
        /// <!-- главное – не переборщить со скоростью, а то эта полоска может начать туда-сюда скакать.
        /// Но с нормальными величинами всё нормально будет. -->
        /// </summary>
        [SerializeField] private float animationSpeedModifier = 1f;

        // Private fields
        /// <summary>
        /// Red bar transform (loosing health indicator)
        /// </summary>
        private RectTransform _deltaIndicator;
        /// <summary>
        /// Health bar indicator 
        /// </summary>
        private RectTransform _indicator;

        /// <summary>
        /// A scale that health bar wants to reach
        /// </summary>
        private float _targetScale;
        /// <summary>
        /// Current health bar value
        /// </summary>
        private float _currentScale;
        /// <summary>
        /// upd: ??Не понял как перевести??
        /// Расстояние от начала индикатора до начала полоски здоровья в последний момент до потери здоровья и до конца анимации.
        /// </summary>
        private float _lastMaxWidthPoint;
        
        private void Start()
        {
            //Инициализируем некоторые значения.
            UIController controller = UIController.RootCanvas.GetComponent<UIController>();
            _rootCanvas = controller.GetComponent<Canvas>();
            _player = controller.Player;
            _indicator = transform.Find("HealthBarInner") as RectTransform;
            _deltaIndicator = transform.Find("DeltaHealthBarInner") as RectTransform;
            _currentScale = _indicator.localScale.x;
        }
        
        private void Update()
        {
            // Check which scale is needed
            _targetScale = PlayerProperties.CurrentHealthPoints / PlayerProperties.MaxHealthPoints;
            // Calculate value to change this frame
            float deltaScaleValue = (float)Math.Round((_targetScale - _currentScale), 3) * animationSpeedModifier * Time.deltaTime;
            // If animation should over, we round value up and end it.
            if (Math.Abs(deltaScaleValue) < 0.0002)
            {
                deltaScaleValue = 0;
                _currentScale = _targetScale;
                _lastMaxWidthPoint = ActualWidth - ActualWidth * _currentScale;
                _deltaIndicator.gameObject.SetActive(false);
            }
            // Else we calculate animation
            else
            {
                // Calculate width to change in process of scaling
                float realDeltaWidth = ActualWidth - ActualWidth * _currentScale - _lastMaxWidthPoint;
                // Move the red line center
                Vector3 deltaPos = transform.rotation * new Vector3(realDeltaWidth / 2 + _lastMaxWidthPoint, 0);
                _deltaIndicator.position = HealthBarStart + new Vector3((float)Math.Round(deltaPos.x, 1), (float)Math.Round(deltaPos.y, 1), (float)Math.Round(deltaPos.z, 1));
                _deltaIndicator.localScale = new Vector3(realDeltaWidth / ActualWidth, 1, 1);
                // Restart the animation if it's updated
                if (deltaScaleValue < 0) _deltaIndicator.gameObject.SetActive(true);
                // Turn it off if the player healed up.
                if (ActualWidth - ActualWidth * _currentScale < _lastMaxWidthPoint)
                {
                    _lastMaxWidthPoint = ActualWidth - ActualWidth * _currentScale;
                    _deltaIndicator.gameObject.SetActive(false);
                }
            }
            // Reset main scale
            _indicator.localScale = new Vector3(deltaScaleValue + _currentScale, _indicator.localScale.y, _indicator.localScale.z);
            // Update current scale value.
            _currentScale = _indicator.localScale.x;
        }
    }

}
