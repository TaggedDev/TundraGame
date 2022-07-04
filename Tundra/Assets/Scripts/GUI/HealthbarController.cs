using Player.Behaviour;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class HealthbarController : MonoBehaviour
{
    // Properties
    /// <summary>
    /// Ссылка на компонент, отвечающий за здоровье игрока.
    /// </summary>
    private PlayerHealth playerHealth => Player.GetComponent<PlayerHealth>();
    /// <summary>
    /// Глобальная координата начала этого индикатора.
    /// </summary>
    private Vector3 healthbarStart => transform.position - transform.rotation * new Vector3(actualWidth / 2, 0, 0);
    /// <summary>
    /// Реальная ширина индикатора в мире, нужна для преобразований.
    /// </summary>
    private float actualWidth => (transform as RectTransform).sizeDelta.x * Canvas.scaleFactor;

    // Public fields
    /// <summary>
    /// Ссылка на игрока (нужна только для скрипта со здоровьем).
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
    /// Трансформация красной полоски для анимации потери здоровья.
    /// </summary>
    private RectTransform DeltaIndicator;
    /// <summary>
    /// Трансформация основной полоски для отображения здоровья.
    /// </summary>
    private RectTransform Indicator;

    /// <summary>
    /// Величина, к которой будет стремиться полоска здоровья.
    /// </summary>
    private float targetScale;
    /// <summary>
    /// Текущая величина полоски.
    /// </summary>
    private float currentScale;
    /// <summary>
    /// Расстояние от начала индикатора до начала полоски здоровья в последний момент до потери здоровья и до конца анимации.
    /// </summary>
    private float lastMaxWidthPoint = 0;
    

    // Start is called before the first frame update
    void Start()
    {
        //Инициализируем некоторые значения.
        Indicator = transform.Find("HealthBarInner") as RectTransform;
        DeltaIndicator = transform.Find("DeltaHealthBarInner") as RectTransform;
        currentScale = Indicator.localScale.x;
    }

    // Update is called once per frame
    void Update()
    {
        //Определяем, куда нам нужно стремиться.
        targetScale = playerHealth.CurrentHealth / playerHealth.MaxHealth;
        //Вычисляем, какой путь проделаем в этом кадре.
        float deltaScaleValue = (float)Math.Round((targetScale - currentScale), 3) * animationSpeedModifier * Time.deltaTime;
        //Debug.Log($"target: {targetScale}, current: {currentScale}, delta: {deltaScaleValue}, actualWidth: {actualWidth}");
        //Если анимация завершена (не надо особо стремиться), то окргуляем величины для точности информации и выключаем анимацию потери хп
        if (Math.Abs(deltaScaleValue) < 0.00002)
        {
            deltaScaleValue = 0;
            currentScale = targetScale;
            lastMaxWidthPoint = actualWidth - actualWidth * currentScale;
            DeltaIndicator.gameObject.SetActive(false);
        }
        //Иначе рассчитываем анимацию потери хп (тут начинается какая-то матемтаическая дичь, вроде я и сам это сделал, но всё равно не хочется в неё лишний раз лезть).
        else
        {
            //Ну а если точнее, то определяем предполагаемую ширину красной полоски, потом рассчитываем сдвиг от начала индикатора и двигаем и масштабируем.
            float realDeltaWidth = actualWidth - actualWidth * currentScale - lastMaxWidthPoint;
            //Debug.Log(realDeltaWidth + " lmwp " + lastMaxWidthPoint);
            Vector3 deltaPos = transform.rotation * new Vector3(realDeltaWidth / 2 + lastMaxWidthPoint, 0);
            DeltaIndicator.position = healthbarStart + new Vector3((float)Math.Round(deltaPos.x, 1), (float)Math.Round(deltaPos.y, 1), (float)Math.Round(deltaPos.z, 1));
            DeltaIndicator.localScale = new Vector3(realDeltaWidth / actualWidth, 1, 1);
            if (deltaScaleValue < 0) DeltaIndicator.gameObject.SetActive(true);//Перезапускаем анимацию, если надо снова терять хп ещё до конца анимации.
            if (actualWidth - actualWidth * currentScale < lastMaxWidthPoint)//И наоборот, выключаем её, если игрок активно хилится.
            {
                lastMaxWidthPoint = actualWidth - actualWidth * currentScale;
                DeltaIndicator.gameObject.SetActive(false);
            }
        }
        Indicator.localScale = new Vector3(deltaScaleValue + currentScale, Indicator.localScale.y, Indicator.localScale.z);//Ну а тут уже переопределяем значения для основной шкалы.
        currentScale = Indicator.localScale.x;//И напоследок обновляем текущее значение.
    }
}
