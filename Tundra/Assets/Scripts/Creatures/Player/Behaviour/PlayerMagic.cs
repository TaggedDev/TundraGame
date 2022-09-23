using Creatures.Player.Inventory;
using Creatures.Player.Magic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;

public class PlayerMagic : MonoBehaviour
{
    private bool _isSpellingPanelOpened;

    internal BookEquipmentConfiguration _config;

    public int MaxSpellElementCount { get; set; }

    public bool IsSpellingPanelOpened
    {
        get 
        { 
            return _isSpellingPanelOpened; 
        }
        private set
        {
            _isSpellingPanelOpened=value;
            MagicPanelVisibilityChange?.Invoke(this, null);
        }
    }

    public bool IsReadyForCasting { get; private set; }

    public List<MagicElement> DraftSpell { get; private set; } = new List<MagicElement>();

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
        if (elementIndex < 5)
        {
            MagicElementSlot slot = _config.MagicElements[elementIndex];
            if (slot.CurrentStonesAmount > 0)
            {
                slot.CurrentStonesAmount--;
                DraftSpell.Add(slot.Element);
                if (DraftSpell.Count == _config.FreeSheets)
                {
                    PrepareForCasting();
                }
            }
        }
    }

    public void Dispell()
    {
        DraftSpell.Clear();
        IsSpellingPanelOpened = false;
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

    }

    IEnumerator ReloadStones()
    {
        while (true)
        {
            if (_config != null)
                _config.ReloadStones();
            yield return new WaitForEndOfFrame();
        }
    }
}

