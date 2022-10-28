using Creatures.Mobs;
using Creatures.Player.Magic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayScript : SpellScript<RaySpell>
{
    private const double time = 3;
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(TryHitEnemy());
    }

    // Update is called once per frame
    void Update()
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
        transform.position = new Vector3(Configuration.Caster.transform.position.x, Configuration.Caster.transform.position.y + 0.4f, Configuration.Caster.transform.position.z);
        //Vector3 castPos = Configuration.Caster.transform.position;
        //Vector3 delta = Input.mousePosition - Camera.main.WorldToScreenPoint(castPos);
        //transform.forward = castPos + delta;
        //transform.position = castPos;
    }

    IEnumerator TryHitEnemy()
    {
        for (int t = 0; t < time; t++)
        {
            yield return new WaitForSecondsRealtime(1);
            // TODO: hit mobs every second.
        }
        Destroy(gameObject);
    }
}
