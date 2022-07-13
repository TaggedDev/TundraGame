using Player.Behaviour;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBarController : MonoBehaviour
{
    [SerializeField] GameObject player;


    PlayerBehaviour playerBehaviour;
    Image imageComponent;

    // Start is called before the first frame update
    void Start()
    {
        imageComponent = GetComponent<Image>();
        playerBehaviour = player.GetComponent<PlayerBehaviour>();
    }

    // Update is called once per frame
    void Update()
    {
        
        float displayValue = playerBehaviour.CurrentStamina / playerBehaviour.MaxStamina;
        imageComponent.fillAmount = displayValue;
    }
}
