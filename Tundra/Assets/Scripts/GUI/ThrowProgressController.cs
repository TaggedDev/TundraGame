using Player.Behaviour;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThrowProgressController : MonoBehaviour
{
    [SerializeField]
    private GameObject player;

    private PlayerBehaviour behaviour;
    private Image progressBar;


    // Start is called before the first frame update
    void Start()
    {
        behaviour = player.GetComponent<PlayerBehaviour>();
        progressBar = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = RectTransformUtility.WorldToScreenPoint(Camera.main, player.transform.position) + new Vector2(0, 80);
        progressBar.fillAmount = (behaviour.ThrowPrepareTime - behaviour._throwLoadingProgress) / behaviour.ThrowPrepareTime;
    }
}
