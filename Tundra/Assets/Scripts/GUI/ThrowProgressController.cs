﻿using Creatures.Player.Behaviour;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThrowProgressController : MonoBehaviour
{
    [SerializeField]
    private GameObject player;

    private PlayerProperties properties;
    private Image progressBar;


    // Start is called before the first frame update
    void Start()
    {
        properties = player.GetComponent<PlayerProperties>();
        progressBar = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = RectTransformUtility.WorldToScreenPoint(Camera.main, player.transform.position) + new Vector2(0, 80);
        progressBar.fillAmount = (properties.ThrowPrepareTime - properties._throwLoadingProgress) / properties.ThrowPrepareTime;
    }
}
