using Player.Behaviour;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarvationbarController : MonoBehaviour
{
    // Properties
    /// <summary>
    /// Ссылка на компонент, отвечающий за голод игрока.
    /// </summary>
    private PlayerStarvation playerStarvation => Player.GetComponent<PlayerStarvation>();

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
    private RectTransform Indicator;

    /// <summary>
    /// Величина, к которой будет стремиться полоска голода.
    /// </summary>
    private float targetScale;
    /// <summary>
    /// Текущая величина полоски.
    /// </summary>
    private float currentScale;


    // Start is called before the first frame update
    void Start()
    {
        Indicator = transform.Find("StarvationBarInner") as RectTransform;
        currentScale = Indicator.localScale.x;
    }

    // Update is called once per frame
    void Update()
    {
        targetScale = playerStarvation.CurrentStarveCapacity / playerStarvation.MaxStarve;
        float deltaScaleValue = (float)Math.Round((targetScale - currentScale), 3) * animationSpeedModifier * Time.deltaTime;
        if (Math.Abs(deltaScaleValue) < 0.00002)
        {
            deltaScaleValue = 0;
            currentScale = targetScale;
        }
        Indicator.localScale = new Vector3(deltaScaleValue + currentScale, Indicator.localScale.y, Indicator.localScale.z);
        currentScale = Indicator.localScale.x;
    }
}
