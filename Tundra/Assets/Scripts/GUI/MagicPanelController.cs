using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;

public class MagicPanelController : MonoBehaviour, IPointerClickHandler
{
    private PlayerMagic _playerMagic;
    private GameObject _segmentHolder;
    private int _selectedSegment;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_playerMagic.IsEnabled)
        {
            _playerMagic.AddElement(_selectedSegment);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _playerMagic = UIController._rootCanvas.GetComponent<UIController>()._player.GetComponent<PlayerMagic>();
        _playerMagic.MagicPanelVisibilityChange += SwitchVisibility;
        _segmentHolder = transform.Find("ElementSelectionCircle").Find("SegmentHolder").gameObject;
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
                _selectedSegment = 1;
            }
            else
            { 
                rot = 180;
                _selectedSegment = 3;
            }
        }
        else
        {
            if (mousePosition.x > 0)
            {
                rot = 270;
                _selectedSegment = 4;
            }
            else
            {
                rot = 90;
                _selectedSegment = 2;
            }
        }
        _segmentHolder.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, rot));
    }
}
