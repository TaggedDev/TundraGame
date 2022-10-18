using Creatures.Player.Inventory;
using Creatures.Player.Magic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using UnityEngine;

public class PlayerMagic : MonoBehaviour
{
    private bool _isSpellingPanelOpened;

    internal BookEquipmentConfiguration _config;

    private List<Type> _availableSpells;

    private Spell _currentSpell;

    public int MaxSpellElementCount { get; set; }

    public MagicElement AllowedElements { get; private set; }

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

    public List<MagicElement> DraftSpell { get; private set; }


    public event EventHandler MagicPanelVisibilityChange;
    public event EventHandler SpellCast;

    // Start is called before the first frame update
    void Start()
    {
        DraftSpell = new List<MagicElement>(MaxSpellElementCount);
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
            if (slot.CurrentStonesAmount > 0 && DraftSpell.Count < _config.FreeSheets)
            {
                slot.CurrentStonesAmount--;
                DraftSpell.Add(slot.Element);
                _availableSpells = Spell.FindSpellTypes(DraftSpell, _availableSpells);
                AllowedElements = _availableSpells.Aggregate(MagicElement.Empty, (x, y) => x |= y.GetCustomAttribute<ElementRestrictionsAttribute>().UsedElements);
                if (_availableSpells.Count == 1)
                {

                }
                //if (DraftSpell.Count == _config.FreeSheets)
                //{
                //    PrepareForCasting();
                //}
            }
        }
    }

    public void Dispell()
    {
        DraftSpell.Clear();
        IsSpellingPanelOpened = false;
        IsReadyForCasting = false;
    }

    public void StartSpelling()
    {
        IsSpellingPanelOpened = true;
    }

    public void PrepareForCasting()
    {
        //IsSpellingPanelOpened = false;
        _currentSpell = Activator.CreateInstance((from x in _availableSpells
                         let elems = x.GetCustomAttribute<SpellAttribute>().Elements.Length
                         orderby elems ascending
                         select x).LastOrDefault()) as Spell;
        _currentSpell?.Build(DraftSpell);
        if (_currentSpell == null) return;
        print("Spell is ready for casting!");
        IsReadyForCasting = true;
    }

    public void CastSpell()
    {
        // TODO: make spellcasting logic
        // Check spell for the recipes

        // Apply additional elements

        // Delete spell
        _currentSpell?.Cast(gameObject, this);
        IsReadyForCasting = false;
        DraftSpell.Clear();
        print("Spell has been casted!");
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

