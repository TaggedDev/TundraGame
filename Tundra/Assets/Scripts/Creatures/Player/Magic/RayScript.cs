using Creatures.Mobs;
using Creatures.Player.Magic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A script to control ray spell game object.
/// </summary>
public class RayScript : SpellScript<RaySpell>
{
    private const double Time = 3;
    
    private void Start()
    {
        StartCoroutine(TryHitEnemy());
    }

    private void Update()
    {
        if (!Input.GetKey(KeyCode.F))
        {
            Destroy(gameObject);
            StopAllCoroutines();
        }
        Ray mouseCastPoint = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane worldPlane = new Plane(Vector3.up, Configuration.Caster.transform.position);
        worldPlane.Raycast(mouseCastPoint, out float enter);
        Vector3 castPos = mouseCastPoint.GetPoint(enter);
        transform.forward = castPos;
        transform.position = new Vector3(Configuration.Caster.transform.position.x, Configuration.Caster.transform.position.y + 0.15f, Configuration.Caster.transform.position.z);
    }

    /// <summary>
    /// Tries to attack mobs in its hit area every second.
    /// </summary>
    /// <returns></returns>
    IEnumerator TryHitEnemy()
    {
        for (int t = 0; t < Time; t++)
        {
            yield return new WaitForSecondsRealtime(1);
            // TODO: hit mobs every second.
        }
        Destroy(gameObject);
    }
}
