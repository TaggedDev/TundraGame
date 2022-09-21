using Creatures.Player.Magic;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class MagicPanelController : MonoBehaviour
{
    private PlayerMagic _playerMagic;
    private GameObject _segmentHolder;
    private GameObject[] _draftElements;
    private GameObject _spellIcon;
    private int _selectedSegment;

    // Start is called before the first frame update
    void Start()
    {
        _playerMagic = UIController._rootCanvas.GetComponent<UIController>()._player.GetComponent<PlayerMagic>();
        _playerMagic.MagicPanelVisibilityChange += SwitchVisibility;
        _segmentHolder = transform.Find("ElementSelectionCircle").Find("SegmentHolder").gameObject;
        _draftElements = new GameObject[2];
        _draftElements[0] = transform.Find("ElementSelectionCircle").Find("CenterCircle").Find("SelectedElementIcon1").gameObject;
        _draftElements[1] = transform.Find("ElementSelectionCircle").Find("CenterCircle").Find("SelectedElementIcon2").gameObject;
        _spellIcon = UIController._rootCanvas.transform.Find("SpellIcon").gameObject;
        gameObject.SetActive(false);
    }

    private void SwitchVisibility(object sender, System.EventArgs e)
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePosition = Input.mousePosition - new Vector3(Screen.width / 2, Screen.height / 2);
        float rot;
        if (Mathf.Abs(mousePosition.x) < Mathf.Abs(mousePosition.y))
        {
            if (mousePosition.y > 0)
            {
                rot = 0;
                _selectedSegment = 0;
            }
            else
            { 
                rot = 180;
                _selectedSegment = 2;
            }
        }
        else
        {
            if (mousePosition.x > 0)
            {
                rot = 270;
                _selectedSegment = 3;
            }
            else
            {
                rot = 90;
                _selectedSegment = 1;
            }
        }
        _segmentHolder.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, rot));
        if (Input.GetMouseButtonDown(0))
        {
            if (_playerMagic.IsSpellingPanelOpened)
            {
                _playerMagic.AddElement(_selectedSegment);
                print($"Element with index {_selectedSegment} has been added!");
            }
        }
    }

    private void FixedUpdate()
    {
        for (int i = 0; i < 2; i++)
        {
            Color selectionColor = Color.gray;
            switch (_playerMagic.DraftSpell[i])
            {
                case MagicElement.Fire:
                    selectionColor = Color.red;
                    break;
                case MagicElement.Air:
                    selectionColor = Color.white;
                    break;
                case MagicElement.Ground:
                    selectionColor = new Color(1f, 0.6367924f, 0.6367924f);
                    break;
                case MagicElement.Water:
                    selectionColor = Color.blue;
                    break;
            }
            _draftElements[i].GetComponent<Image>().color = selectionColor;
        }
    }
}
