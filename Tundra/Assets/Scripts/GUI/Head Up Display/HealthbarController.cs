using Creatures.Player.Behaviour;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UIElements;


namespace GUI
{
    public class HealthbarController : MonoBehaviour
    {
        // Properties
        /// <summary>
        /// Ссылка на компонент, отвечающий за здоровье игрока.
        /// </summary>
        private PlayerProperties PlayerProperties => _player.GetComponent<PlayerProperties>();
        /// <summary>
        /// Глобальная координата начала этого индикатора.
        /// </summary>
        private Vector3 HealthbarStart => transform.position - transform.rotation * new Vector3(ActualWidth / 2, 0, 0);
        /// <summary>
        /// Реальная ширина индикатора в мире, нужна для преобразований.
        /// </summary>
        private float ActualWidth => (transform as RectTransform).sizeDelta.x * _rootCanvas.scaleFactor;

        // Public fields
        private Canvas _rootCanvas;
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
        /// Трансформация красной полоски для анимации потери здоровья.
        /// </summary>
        private RectTransform _deltaIndicator;
        /// <summary>
        /// Трансформация основной полоски для отображения здоровья.
        /// </summary>
        private RectTransform _indicator;

        /// <summary>
        /// Величина, к которой будет стремиться полоска здоровья.
        /// </summary>
        private float _targetScale;
        /// <summary>
        /// Текущая величина полоски.
        /// </summary>
        private float _currentScale;
        /// <summary>
        /// Расстояние от начала индикатора до начала полоски здоровья в последний момент до потери здоровья и до конца анимации.
        /// </summary>
        private float _lastMaxWidthPoint = 0;


        // Start is called before the first frame update
        void Start()
        {
            //Инициализируем некоторые значения.
            UIController controller = UIController._rootCanvas.GetComponent<UIController>();
            _rootCanvas = controller.GetComponent<Canvas>();
            _player = controller._player;
            _indicator = transform.Find("HealthBarInner") as RectTransform;
            _deltaIndicator = transform.Find("DeltaHealthBarInner") as RectTransform;
            _currentScale = _indicator.localScale.x;
        }

        // Update is called once per frame
        void Update()
        {
            //Определяем, куда нам нужно стремиться.
            _targetScale = PlayerProperties.CurrentHealth / PlayerProperties.MaxHealth;
            //Вычисляем, какой путь проделаем в этом кадре.
            float deltaScaleValue = (float)Math.Round((_targetScale - _currentScale), 3) * animationSpeedModifier * Time.deltaTime;
            //Debug.Log($"target: {targetScale}, current: {currentScale}, delta: {deltaScaleValue}, actualWidth: {actualWidth}");
            //Если анимация завершена (не надо особо стремиться), то окргуляем величины для точности информации и выключаем анимацию потери хп
            if (Math.Abs(deltaScaleValue) < 0.0000002)
            {
                deltaScaleValue = 0;
                _currentScale = _targetScale;
                _lastMaxWidthPoint = ActualWidth - ActualWidth * _currentScale;
                _deltaIndicator.gameObject.SetActive(false);
            }
            //Иначе рассчитываем анимацию потери хп (тут начинается какая-то матемтаическая дичь, вроде я и сам это сделал, но всё равно не хочется в неё лишний раз лезть).
            else
            {
                //Ну а если точнее, то определяем предполагаемую ширину красной полоски, потом рассчитываем сдвиг от начала индикатора и двигаем и масштабируем.
                float realDeltaWidth = ActualWidth - ActualWidth * _currentScale - _lastMaxWidthPoint;
                //Debug.Log(realDeltaWidth + " lmwp " + lastMaxWidthPoint);
                Vector3 deltaPos = transform.rotation * new Vector3(realDeltaWidth / 2 + _lastMaxWidthPoint, 0);
                _deltaIndicator.position = HealthbarStart + new Vector3((float)Math.Round(deltaPos.x, 1), (float)Math.Round(deltaPos.y, 1), (float)Math.Round(deltaPos.z, 1));
                _deltaIndicator.localScale = new Vector3(realDeltaWidth / ActualWidth, 1, 1);
                if (deltaScaleValue < 0) _deltaIndicator.gameObject.SetActive(true);//Перезапускаем анимацию, если надо снова терять хп ещё до конца анимации.
                if (ActualWidth - ActualWidth * _currentScale < _lastMaxWidthPoint)//И наоборот, выключаем её, если игрок активно хилится.
                {
                    _lastMaxWidthPoint = ActualWidth - ActualWidth * _currentScale;
                    _deltaIndicator.gameObject.SetActive(false);
                }
            }
            _indicator.localScale = new Vector3(deltaScaleValue + _currentScale, _indicator.localScale.y, _indicator.localScale.z);//Ну а тут уже переопределяем значения для основной шкалы.
            _currentScale = _indicator.localScale.x;//И напоследок обновляем текущее значение.
        }
    }

}
