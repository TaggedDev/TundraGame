using Player.Behaviour;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class HealthbarController : MonoBehaviour
{
    // Properties
    private PlayerHealth playerHealth => Player.GetComponent<PlayerHealth>();
    private Vector3 healthbarStart => transform.position - transform.rotation * new Vector3(actualWidth / 2, 0, 0);

    // Public fields
    public GameObject Player;
    public Canvas Canvas;

    [SerializeField]
    private float animationSpeedModifier = 1f;

    // Private fields
    private RectTransform DeltaIndicator;
    private RectTransform Indicator;

    private float actualWidth;
    private float targetScale;
    private float currentScale;
    private float lastMaxWidthPoint = 0;
    

    // Start is called before the first frame update
    void Start()
    {
        Indicator = transform.Find("HealthBarInner") as RectTransform;
        DeltaIndicator = transform.Find("DeltaHealthBarInner") as RectTransform;
        actualWidth = (transform as RectTransform).sizeDelta.x * Canvas.scaleFactor;
        currentScale = Indicator.localScale.x;
    }

    // Update is called once per frame
    void Update()
    {
        targetScale = playerHealth.CurrentHealth / playerHealth.MaxHealth;
        float deltaScaleValue = (float)Math.Round((targetScale - currentScale), 3) * animationSpeedModifier * Time.deltaTime;
        //Debug.Log($"target: {targetScale}, current: {currentScale}, delta: {deltaScaleValue}, actualWidth: {actualWidth}");
        if (Math.Abs(deltaScaleValue) < 0.00002)
        {
            deltaScaleValue = 0;
            currentScale = targetScale;
            lastMaxWidthPoint = actualWidth - actualWidth * currentScale;
            DeltaIndicator.gameObject.SetActive(false);
        }
        else
        {
            float realDeltaWidth = actualWidth - actualWidth * currentScale - lastMaxWidthPoint;
            Debug.Log(realDeltaWidth + " lmwp " + lastMaxWidthPoint);
            DeltaIndicator.position = healthbarStart + transform.rotation * new Vector3(realDeltaWidth / 2 + lastMaxWidthPoint, 0);
            DeltaIndicator.localScale = new Vector3(realDeltaWidth / actualWidth, 1, 1);
            if (deltaScaleValue < 0) DeltaIndicator.gameObject.SetActive(true);
            if (actualWidth - actualWidth * currentScale < lastMaxWidthPoint)
            {
                lastMaxWidthPoint = actualWidth - actualWidth * currentScale;
                DeltaIndicator.gameObject.SetActive(false);
            }
        }
        Indicator.localScale = new Vector3(deltaScaleValue + currentScale, Indicator.localScale.y, Indicator.localScale.z);
        currentScale = Indicator.localScale.x;
    }
}
