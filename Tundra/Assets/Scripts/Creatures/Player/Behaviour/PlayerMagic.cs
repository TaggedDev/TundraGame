﻿using Creatures.Player.Inventory;
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
    private bool _currentSpellSelected;
    /// <summary>
    /// A magic book player holds in his hand.
    /// </summary>
    internal BookEquipmentConfiguration _config;
    /// <summary>
    /// Spells which are available for player.
    /// </summary>
    private List<Type> _availableSpells;
    /// <summary>
    /// A built spell which is ready for casting.
    /// </summary>
    private Spell _currentSpell;
    /// <summary>
    /// Prefabs of spells. Idk where to place them.
    /// </summary>
    [SerializeField]
    private List<GameObject> spellPrefabs;

    //public int MaxSpellElementCount { get; set; }

    //public MagicElement AllowedElements { get; private set; } = MagicElement.All;

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
    public event EventHandler<Spell> SpellCast;
    // Start is called before the first frame update
    void Start()
    {
        DraftSpell = new List<MagicElement>(5);
        StartCoroutine(ReloadStones());
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
                CheckCurrentElements();
                //if (DraftSpell.Count == _config.FreeSheets)
                //{
                //    PrepareForCasting();
                //}
            }
        }
    }

    internal void CheckCurrentElements()
    {
        if (!_currentSpellSelected)
        {
            _availableSpells = Spell.FindSpellTypes(DraftSpell, _availableSpells);
            print($"Spells: {_availableSpells.Count}");
            //AllowedElements = _availableSpells.Aggregate(MagicElement.Empty, (x, y) => x |= y.GetCustomAttribute<ElementRestrictionsAttribute>().UsedElements);
            //print($"Elements: {AllowedElements}");
            if (_availableSpells.Count == 1)
            {
                _currentSpellSelected = true;
            }
            if (_availableSpells.Count == 0)
            {
                //TODO: an action to case when no more spells are available.
            }
        }
    }

    public void Dispell()
    {
        foreach (var element in DraftSpell)
        {
            MagicElementSlot slot = _config.MagicElements.FirstOrDefault(x => x.Element == element);
            if (slot == null) continue;
            slot.CurrentStonesAmount++;
            if (slot.CurrentStonesAmount > slot.MaxStonesAmount) slot.CurrentStonesAmount = slot.MaxStonesAmount;
        }
        DraftSpell.Clear();
        IsSpellingPanelOpened = false;
        IsReadyForCasting = false;
        //AllowedElements = MagicElement.All;
    }

    public void StartSpelling()
    {
        IsSpellingPanelOpened = true;
    }

    public void PrepareForCasting()
    {
        //IsSpellingPanelOpened = false;
        var spell = (from x in Spell.FindSpellTypes(DraftSpell, null)
                     let elems = x.GetCustomAttribute<SpellAttribute>().Elements.Length
                     //where elems == MaxSpellElementCount //It's filter of pages.
                     orderby elems ascending
                     select x).LastOrDefault();
        if (spell != null)
        {
            _currentSpell = Activator.CreateInstance(spell) as Spell;
            _currentSpell?.Build(DraftSpell);
            if (_currentSpell == null) return;
            print("Spell is ready for casting!");
            IsReadyForCasting = true;
        }
    }

    public void CastSpell()
    {
        _currentSpell?.Cast(gameObject, this);
        SpellCast?.Invoke(this, _currentSpell);
        IsReadyForCasting = false;
        _currentSpellSelected = false;
        _currentSpell = null;
        _availableSpells = null;
        //AllowedElements = MagicElement.All;
        DraftSpell.Clear();
        print("Spell has been casted!");
    }

    public GameObject GetSpellPrefabByID(int id)
    {
        return spellPrefabs[id];
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

