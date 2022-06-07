using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class DayCycle : MonoBehaviour
{
    [Range(0, 1)]
    [SerializeField] private float currentTime;
    
    [FormerlySerializedAs("CurrentTime (s)")]
    [SerializeField] private float dayDuration;
    [SerializeField] private Light sun;
    [SerializeField] private Light moon;
    
    private float _moonLightIntensity;
    private float _sunLightIntensity;
    
    public AnimationCurve sunCurve;
    public AnimationCurve moonCurve;

    private void Start()
    {
        currentTime = .2f;
        _sunLightIntensity = sun.intensity;
        _moonLightIntensity = moon.intensity;
    }

    private void Update()
    {
        currentTime += Time.deltaTime / dayDuration;
        if (currentTime >= 1) currentTime -= 1;

        sun.transform.localRotation = Quaternion.Euler(currentTime * 360f, 180, 0);
        moon.transform.localRotation = Quaternion.Euler(currentTime * 360f + 180f, 180, 0);

        sun.intensity = _sunLightIntensity * sunCurve.Evaluate(currentTime);
        moon.intensity = _moonLightIntensity * moonCurve.Evaluate(currentTime);
    }
}
