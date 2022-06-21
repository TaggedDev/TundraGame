using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HitProgressChanger : MonoBehaviour
{
    private float progress = 0;

    public float HitLoadTime = 1.5f;

    public Image HitProgress;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            progress += Time.smoothDeltaTime;
        }
        else progress -= Time.deltaTime;
        progress = Mathf.Clamp(progress, 0, HitLoadTime);
        HitProgress.fillAmount = progress / HitLoadTime;
    }
}
