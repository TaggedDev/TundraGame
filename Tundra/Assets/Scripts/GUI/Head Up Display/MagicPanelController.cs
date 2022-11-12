using Creatures.Player.Inventory;
using Creatures.Player.Magic;
using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class MagicPanelController : MonoBehaviour
{
    [SerializeField]
    private GameObject bookSheetSlot;
    [SerializeField]
    private List<Sprite> elementIcons;
    [SerializeField]
    private List<Color> elementColors;
    [SerializeField]
    private Sprite defaultSlotIcon;
    [SerializeField]
    private Sprite filledStoneProgressIcon;
    [SerializeField]
    private Sprite nonFilledStoneProgressIcon;
    private PlayerMagic _playerMagic;
    private PlayerEquipment _playerEquipment;
    private GameObject _elementsPanel;
    private BookEquipmentConfiguration _currentMagicBook;
    private GameObject _bookSlot;
    private readonly List<GameObject> _elements = new List<GameObject>();
    private readonly List<GameObject> _bookSheets = new List<GameObject>();
    private readonly List<Image> _sheetIcons = new List<Image>();

    private void Start()
    {
        GameObject player = UIController._rootCanvas.GetComponent<UIController>()._player;
        _playerMagic = player.GetComponent<PlayerMagic>();
        _playerEquipment = player.GetComponent<PlayerEquipment>();
        _playerEquipment.EquipmentChanged += (sender, e) =>
        {
            if (e is BookEquipmentConfiguration)
                _currentMagicBook = _playerEquipment.Book as BookEquipmentConfiguration;
            SetSheets();
            SetElements();
        };
        _currentMagicBook = _playerEquipment.Book as BookEquipmentConfiguration;
        _elementsPanel = gameObject.transform.Find("SpellCastingPanel").gameObject;
        _bookSlot = gameObject.transform.Find("GrimuarSlot").gameObject;
        for (int i = 1; i <= 5; i++)
        {
            _elements.Add(_elementsPanel.transform.Find("ElementSlot" + i).gameObject);
        }
        SetSheets();
        SetElements();
        _playerMagic.MagicPanelVisibilityChange += SwitchVisibility;
        SwitchVisibility(null, null);
    }

    /// <summary>
    /// Sets elements icon of this controller.
    /// </summary>
    private void SetElements()
    {
        for (int i = 0; i < _elements.Count; i++)
        {
            try
            {
                //That was hard to get natural number of element when it has number associated with binary field.
                var icon = _elements[i].transform.Find("ElementIcon").gameObject.GetComponent<Image>();
                icon.sprite = elementIcons[1 + (int)Math.Log((int)_currentMagicBook.MagicElements[i].Element, 2)];
                bool allowed = true;
                _elements[i].GetComponent<Button>().interactable = allowed;
                icon.color = allowed ? Color.white : new Color(0.5f, 0.5f, 0.5f, 0.5f);
            }
            catch
            {
                var icon = _elements[i].transform.Find("ElementIcon").gameObject.GetComponent<Image>();
                icon.sprite = elementIcons.First();
                icon.color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
            }
        }
    }

    /// <summary>
    /// Sets sheets icons to this controller.
    /// </summary>
    private void SetSheets()
    {
        foreach (var sheet in _bookSheets)
        {
            Destroy(sheet);
        }
        _bookSheets.Clear();
        _sheetIcons.Clear();
        Image bookImage = _bookSlot.transform.Find("ItemIcon").gameObject.GetComponent<Image>();
        if (_currentMagicBook != null)
        {
            if (_currentMagicBook.Icon != null)
                bookImage.sprite = _currentMagicBook.Icon;
            bookImage.color = new Color(bookImage.color.r, bookImage.color.g, bookImage.color.b, 1f);
            float width = _currentMagicBook.FreeSheets * 150;
            Vector3 start = new Vector3(75 - width / 2, 75);
            for (int i = 0; i < _currentMagicBook.FreeSheets; i++)
            {
                var sheet = Instantiate(bookSheetSlot, _elementsPanel.transform);
                _bookSheets.Add(sheet);
                sheet.transform.localPosition = start;
                start += new Vector3(150, 0);
                _sheetIcons.Add(sheet.transform.Find("InternalIcon").gameObject.GetComponent<Image>());
            }
        }
        else
        {
            bookImage.sprite = defaultSlotIcon;
            bookImage.color = new Color(bookImage.color.r, bookImage.color.g, bookImage.color.b, 0.5f);
        }
    }

    /// <summary>
    /// Switches visibility of a panel.
    /// </summary>
    private void SwitchVisibility(object sender, EventArgs e)
    {
        _elementsPanel.SetActive(_playerMagic.IsSpellingPanelOpened);
    }

    /// <summary>
    /// Handles user click on element.
    /// </summary>
    /// <param name="source"></param>
    public void OnElementClicked(GameObject source)
    {
        int index = _elements.IndexOf(source);
        _playerMagic.AddElement(index);
        SetElements();
    }

    private void FixedUpdate()
    {
        if (_currentMagicBook != null)
        {
            // Colorizing book sheets.
            for (int i = 0; i < _bookSheets.Count; i++)
            {
                Image sheetIcon = _bookSheets[i].transform.Find("InternalIcon").gameObject.GetComponent<Image>();
                if (i < _playerMagic.DraftSpell.Count)
                {
                    sheetIcon.color = Color.green;
                }
                else if (_playerMagic.IsReadyForCasting)
                {
                    sheetIcon.color = new Color(0.56f, 0f, 1f);
                }
                else sheetIcon.color = new Color(0.7f, 0.7f, 0.7f, 0.7f);
            }
            // Updating elements info
            for (int i = 0; i < _elements.Count; i++)
            {
                var stonesNode = _elements[i].transform.Find("StonesAmountPlace");
                MagicElementSlot element = _currentMagicBook.MagicElements[i];
                Text text = stonesNode.Find("StonesAmount").gameObject.GetComponent<Text>();
                text.text = element.CurrentStonesAmount.ToString();
                Image progress = stonesNode.Find("ProgressIndicator").gameObject.GetComponent<Image>();
                progress.color = elementColors[1 + (int)Math.Log((int)_currentMagicBook.MagicElements[i].Element, 2)];
                if (element.CurrentStonesAmount == element.MaxStonesAmount)
                {
                    progress.sprite = filledStoneProgressIcon;
                    progress.fillAmount = 1f;
                }
                else
                {
                    progress.sprite = nonFilledStoneProgressIcon;
                    progress.fillAmount = element.ReloadProgress;
                }
                Image icon = _elements[i].transform.Find("ElementIcon").gameObject.GetComponent<Image>();
                if (element.CurrentStonesAmount == 0)
                {
                    icon.color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
                }
                else
                {
                    bool allowed = true;
                    icon.color = allowed ? Color.white : new Color(0.5f, 0.5f, 0.5f, 0.5f);
                }
            }
            // Handling input
            if (Input.GetKeyDown(KeyCode.Alpha1)) OnElementClicked(_elements[0]);
            if (Input.GetKeyDown(KeyCode.Alpha2)) OnElementClicked(_elements[1]);
            if (Input.GetKeyDown(KeyCode.Alpha3)) OnElementClicked(_elements[2]);
            if (Input.GetKeyDown(KeyCode.Alpha4)) OnElementClicked(_elements[3]);
            if (Input.GetKeyDown(KeyCode.Alpha5)) OnElementClicked(_elements[4]);
        }
    }
}
