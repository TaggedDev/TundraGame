using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class PlayerMagic : MonoBehaviour
{
    private bool isEnabled;

    public int MaxSpellElementCount { get; private set; } = 2;

    public bool IsEnabled
    {
        get 
        { 
            return isEnabled; 
        }
        internal set
        {
            isEnabled=value;
            if (!isEnabled)
            {
                DraftSpell = new List<MagicElement>() { MagicElement.Empty, MagicElement.Empty };
            }
            MagicPanelVisibilityChange?.Invoke(this, null);
        }
    }

    public List<MagicElement> MagicElements { get; private set; } = new List<MagicElement>() { MagicElement.Air, MagicElement.Fire, MagicElement.Ground, MagicElement.Water };

    public List<MagicElement> DraftSpell { get; private set; } = new List<MagicElement>() { MagicElement.Empty, MagicElement.Empty };

    public event EventHandler MagicPanelVisibilityChange;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddElement(int elementIndex)
    {
        DraftSpell[DraftSpell.IndexOf(MagicElement.Empty)] = MagicElements[elementIndex];
        print($"Element {MagicElements[elementIndex]} has been added!");
    }
}

[Flags]
public enum MagicElement
{
    Empty = 0,
    Air = 1,
    Fire = 2,
    Ground = 4,
    Water = 8,
}