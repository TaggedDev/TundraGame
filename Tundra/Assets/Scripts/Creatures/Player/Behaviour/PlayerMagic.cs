using Creatures.Player.Inventory;
using Creatures.Player.Magic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

/// <summary>
/// A script that controls player's magic logic.
/// </summary>
public class PlayerMagic : MonoBehaviour
{
    private bool _isSpellingPanelOpened;
    /// <summary>
    /// A magic book player holds in his hand.
    /// </summary>
    internal BookEquipmentConfiguration _book;
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

    /// <summary>
    /// Checks if spelling panel opened.
    /// </summary>
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

    /// <summary>
    /// Checks if player ready to cast a spell.
    /// </summary>
    public bool IsReadyForCasting { get; private set; }

    /// <summary>
    /// A spell prototype which player has selected.
    /// </summary>
    public List<MagicElement> DraftSpell { get; private set; }

    /// <summary>
    /// An event which invokes when panel visibilty should change.
    /// </summary>
    public event EventHandler MagicPanelVisibilityChange;

    /// <summary>
    /// En event which invokes when spell is cast.
    /// </summary>
    public event EventHandler<Spell> SpellCast;


    private void Start()
    {
        DraftSpell = new List<MagicElement>(5);
        StartCoroutine(ReloadStones());
    }

    /// <summary>
    /// Adds an element to <see cref="DraftSpell"/>.
    /// </summary>
    /// <param name="elementIndex">Index of element in book.</param>
    public void AddElement(int elementIndex)
    {
        // We assume that book contains only 5 elements so index must be less than 5.
        if (elementIndex < 5)
        {
            // Select current magic element slot in book.
            MagicElementSlot slot = _book.MagicElements[elementIndex];
            // If player can use this element, we spend his stone and add it to the current spell.
            if (slot.CurrentStonesAmount > 0 && DraftSpell.Count < _book.FreeSheets)
            {
                slot.CurrentStonesAmount--;
                DraftSpell.Add(slot.Element);
            }
        }
    }

    /// <summary>
    /// Clears the draft spell and resets logic in case of player cancels casting.
    /// </summary>
    public void Dispell()
    {
        // We should return all spent stones to player
        foreach (var element in DraftSpell)
        {
            MagicElementSlot slot = _book.MagicElements.FirstOrDefault(x => x.Element == element);
            if (slot == null) continue;
            slot.CurrentStonesAmount++;
            if (slot.CurrentStonesAmount > slot.MaxStonesAmount) slot.CurrentStonesAmount = slot.MaxStonesAmount;
        }
        // And reset all spell system.
        DraftSpell.Clear();
        IsSpellingPanelOpened = false;
        IsReadyForCasting = false;
    }

    /// <summary>
    /// Prepares to create a spell.
    /// </summary>
    public void StartSpelling()
    {
        IsSpellingPanelOpened = true;
        // You can make here a logic to bind animations etc.
    }

    /// <summary>
    /// Prepares to cast selected spell.
    /// </summary>
    public void PrepareForCasting()
    {
        // Finds a spell in full spells list which passes all of conditions.
        // If there's more than one spell which passes conditions, it selects a spell with the longest formula.
        var spell = (from x in Spell.FindSpellTypes(DraftSpell, null)
                     let elems = x.GetCustomAttribute<SpellAttribute>().Elements.Length
                     orderby elems ascending
                     select x).LastOrDefault();
        // If spell type isn't null, it creates it's instance and builds its properties in order of used elements
        if (spell != null)
        {
            _currentSpell = Activator.CreateInstance(spell) as Spell;
            _currentSpell?.Build(DraftSpell);
            if (_currentSpell == null) return;
            IsReadyForCasting = true;
        }
    }

    /// <summary>
    /// Casts prepared spell and resets system.
    /// </summary>
    public void CastSpell()
    {
        _currentSpell?.Cast(gameObject, this);
        SpellCast?.Invoke(this, _currentSpell);
        IsReadyForCasting = false;
        _currentSpell = null;
        _availableSpells = null;
        //AllowedElements = MagicElement.All;
        DraftSpell.Clear();
        print("Spell has been casted!");
    }

    /// <summary>
    /// Gets spell prefab by its ID.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public GameObject GetSpellPrefabByID(int id)
    {
        return spellPrefabs[id];
    }

    /// <summary>
    /// A background function which reloads player's stones every frame.
    /// </summary>
    /// <returns></returns>
    IEnumerator ReloadStones()
    {
        while (true)
        {
            if (_book != null)
                _book.ReloadStones();
            yield return new WaitForEndOfFrame();
        }
    }
}

