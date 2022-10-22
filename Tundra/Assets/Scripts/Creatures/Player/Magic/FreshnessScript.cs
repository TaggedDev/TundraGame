using Creatures.Player.Behaviour;
using Creatures.Player.Magic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreshnessScript : SpellScript<FreshnessSpell>
{
    const double time = 10;
    double _lifeTime;
    private PlayerProperties props;

    // Start is called before the first frame update
    void Start()
    {
        props = Configuration.Caster.GetComponent<PlayerProperties>();
        props.CurrentSpeed *= (float)Configuration.PlayerSpeedCoefficient;
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
            props.CurrentSpeed /= (float)Configuration.PlayerSpeedCoefficient;
        }
        // Heals player
        props.CurrentHealth += (float)(Configuration.RegenerationCoefficient * coefficient);
        if (props.CurrentHealth > props.MaxHealth) props.CurrentHealth = props.MaxHealth;
        props.CurrentWarmLevel += (float)(Configuration.PlayerWarmCoefficient * coefficient);
        if (props.CurrentWarmLevel > props.MaxWarmLevel) props.CurrentWarmLevel = props.MaxWarmLevel;
    }
}
