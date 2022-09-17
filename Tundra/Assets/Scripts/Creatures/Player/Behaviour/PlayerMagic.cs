using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class PlayerMagic : MonoBehaviour
{
    public bool IsEnabled { get; private set; }

    public event EventHandler MagicPanelVisibilityChange;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            MagicPanelVisibilityChange?.Invoke(this, null);
        }
    }

    public void AddElement(int elementIndex)
    {

    }
}
