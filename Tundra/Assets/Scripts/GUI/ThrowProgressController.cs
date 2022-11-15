using Creatures.Player.Behaviour;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThrowProgressController : MonoBehaviour
{
    private GameObject _player;

    private PlayerProperties properties;
    private Image progressBar;


    // Start is called before the first frame update
    void Start()
    {
        _player = UIController.RootCanvas.GetComponent<UIController>()._player;
        properties = _player.GetComponent<PlayerProperties>();
        progressBar = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        progressBar.fillAmount = (properties.ThrowPrepareTime - properties._throwLoadingProgress) / properties.ThrowPrepareTime;
    }

    private void FixedUpdate()
    {
        transform.position = RectTransformUtility.WorldToScreenPoint(Camera.main, _player.transform.position) + new Vector2(0, 80);
    }
}
