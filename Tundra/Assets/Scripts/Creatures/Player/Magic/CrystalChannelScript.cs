using Creatures.Player.Behaviour;
using Creatures.Player.Magic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalChannelScript : MonoBehaviour
{
    public CrystalChannelSpell Configuration { get; set; }

    private const double time = 10;
    private double _lifeTime;
    private PlayerProperties props;
    // Start is called before the first frame update
    void Start()
    {
        props = Configuration.Caster.GetComponent<PlayerProperties>();
        props.CurrentSpeed *= (float)Configuration.MovementFineCoefficient;
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position = Configuration.Caster.transform.position;
        double coefficient = Time.deltaTime;
        _lifeTime += coefficient;
        if (_lifeTime >= time)
        {
            Destroy(gameObject);
            props.CurrentSpeed /= (float)Configuration.MovementFineCoefficient;
        }
        // Heals player
        props.CurrentHealth += (float)(Configuration.Regeneration * coefficient);
        if (props.CurrentHealth > props.MaxHealth) props.CurrentHealth = props.MaxHealth;
        //TODO: deal damage to enemy which attacks player.
    }
}
