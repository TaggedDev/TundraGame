using Creatures.Player.Magic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;

public class PlayerMagic : MonoBehaviour
{
    private bool isSpellingPanelOpened;

    public int MaxSpellElementCount { get; private set; } = 2;

    public bool IsSpellingPanelOpened
    {
        get 
        { 
            return isSpellingPanelOpened; 
        }
        private set
        {
            isSpellingPanelOpened=value;
            MagicPanelVisibilityChange?.Invoke(this, null);
        }
    }

    public bool IsReadyForCasting { get; private set; }

    public List<SpellingElementSlot> MagicSlots { get; private set; } = new List<SpellingElementSlot>() 
    { 
        new SpellingElementSlot(MagicElement.Air, 5, 5f), 
        new SpellingElementSlot(MagicElement.Fire, 3, 6f), 
        new SpellingElementSlot(MagicElement.Ground, 4, 7f), 
        new SpellingElementSlot(MagicElement.Water, 4, 5f) 
    };

    public List<MagicElement> DraftSpell { get; private set; } = new List<MagicElement>() { MagicElement.Empty, MagicElement.Empty };

    public event EventHandler MagicPanelVisibilityChange;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ReloadStones());
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AddElement(int elementIndex)
    {
        if (MagicSlots[elementIndex].CurrentStonesAmount > 0)
        {
            int index = DraftSpell.IndexOf(MagicElement.Empty);
            if (index == -1)
            {
                DraftSpell = new List<MagicElement>() { MagicElement.Empty, MagicElement.Empty };
            }
            DraftSpell[index] = MagicSlots[elementIndex].Element;
            MagicSlots[elementIndex].CurrentStonesAmount--;
            print($"Element {MagicSlots[elementIndex]} has been added!");
            if (index == DraftSpell.Count - 1)
            {
                PrepareForCasting();
            }
        }
    }

    public void Dispell()
    {
        if (DraftSpell.IndexOf(MagicElement.Empty) == -1)
        {
            PrepareForCasting();
        }
        else
        {
            DraftSpell = new List<MagicElement>() { MagicElement.Empty, MagicElement.Empty };
            IsSpellingPanelOpened = false;
            IsReadyForCasting = false;
        }
    }

    public void StartSpelling()
    {
        IsSpellingPanelOpened = true;
    }

    public void PrepareForCasting()
    {
        IsSpellingPanelOpened = false;
        IsReadyForCasting = true;
    }

    public void CastSpell()
    {
        if (IsReadyForCasting)
        {
            MagicElement resultingSpellCode = DraftSpell.Aggregate((s, x) => s | x);
            //TODO: cast resulting spell.
        }
    }

    IEnumerator ReloadStones()
    {
        while (true)
        {
            foreach (var slot in MagicSlots)
            {
                slot.UpdateReloadState(Time.deltaTime);
            }
            yield return new WaitForEndOfFrame();
        }
    }
}

