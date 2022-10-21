using Creatures.Player.Magic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayScript : MonoBehaviour
{
    public RaySpell Configuration { get; set; }

    private const double time = 3;
    private const float maxLength = 100;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(TryHitEnemy());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator TryHitEnemy()
    {
        for (int t = 0; t < time; t++)
        {
            yield return new WaitForSecondsRealtime(1);
            var hit = Physics.RaycastAll(transform.position, transform.forward, maxLength);
            if (hit != null)
            {

            }
        }
    }
}
