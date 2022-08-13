using Creatures.Player.Behaviour;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBarController : MonoBehaviour
{
    [SerializeField] GameObject player;


    PlayerProperties playerProperties;
    Image imageComponent;

    // Start is called before the first frame update
    void Start()
    {
        imageComponent = GetComponent<Image>();
        playerProperties = player.GetComponent<PlayerProperties>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = RectTransformUtility.WorldToScreenPoint(Camera.main, player.transform.position) - new Vector2(0, 50);
        float displayValue = playerProperties.CurrentStamina / playerProperties.MaxStamina;
        imageComponent.fillAmount = displayValue;
    }
}
