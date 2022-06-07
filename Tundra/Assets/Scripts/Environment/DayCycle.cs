using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayCycle : MonoBehaviour
{
    [Range(0, 1)]
    public float CurrentTime;
    public float DayDuration;
    public Light Sun;
    public Light Moon;
    
    private float _moonLightIntensity;
    private float _sunLightIntensity;
    
    public AnimationCurve SunCurve;
    public AnimationCurve MoonCurve;

    private void Start()
    {
        _sunLightIntensity = Sun.intensity;
        _moonLightIntensity = Moon.intensity;
    }

    private void Update()
    {
        CurrentTime += Time.deltaTime / DayDuration;
        if (CurrentTime >= 1) CurrentTime -= 1;

        Sun.transform.localRotation = Quaternion.Euler(CurrentTime * 360f, 180, 0);
        Moon.transform.localRotation = Quaternion.Euler(CurrentTime * 360f + 180f, 180, 0);

        Sun.intensity = _sunLightIntensity * SunCurve.Evaluate(CurrentTime);
        Moon.intensity = _moonLightIntensity * MoonCurve.Evaluate(CurrentTime);
    }
}
