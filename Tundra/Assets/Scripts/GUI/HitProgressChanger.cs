using Creatures.Player.Behaviour;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HitProgressChanger : MonoBehaviour
{

    public Image HitProgress;

    [SerializeField]
    private GameObject player;
    private PlayerProperties _playerProperties;
    // Start is called before the first frame update
    void Start()
    {
        _playerProperties = player.GetComponent<PlayerProperties>();
    }

    // Update is called once per frame
    void Update()
    {
        HitProgress.fillAmount = _playerProperties.CurrentHitProgress / _playerProperties.HitPreparationTime;
    }

    private void FixedUpdate()
    {
        transform.position = RectTransformUtility.WorldToScreenPoint(Camera.main, player.transform.position) + new Vector2(0, 80);
    }
}
