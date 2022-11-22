using Creatures.Player.Behaviour;
using Creatures.Player.Magic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Creatures.Player.Magic
{

    /// <summary>
    /// A script to control freshness spell.
    /// </summary>
    public class FreshnessScript : SpellScript<FreshnessSpell>
    {
        const double Time = 10;
        private double _lifeTime;
        private PlayerProperties _props;

        private void Start()
        {
            _props = Configuration.Caster.GetComponent<PlayerProperties>();
            _props.CurrentSpeed *= (float)Configuration.PlayerSpeedCoefficient;
        }

        private void Update()
        {
            gameObject.transform.position = Configuration.Caster.transform.position;
            double coefficient = UnityEngine.Time.deltaTime;
            _lifeTime += coefficient;
            if (_lifeTime >= Time)
            {
                Destroy(gameObject);
                _props.CurrentSpeed /= (float)Configuration.PlayerSpeedCoefficient;
            }
            // Heals player
            _props.CurrentHealthPoints += (float)(Configuration.RegenerationCoefficient * coefficient);
            if (_props.CurrentHealthPoints > _props.MaxHealthPoints)
            {
                _props.CurrentHealthPoints = _props.MaxHealthPoints;
            }
            
            _props.CurrentWarmthPoints += (float)(Configuration.PlayerWarmCoefficient * coefficient);
            if (_props.CurrentWarmthPoints > _props.MaxWarmthPoints)
            {
                _props.CurrentWarmthPoints = _props.MaxWarmthPoints;
            }
        }
    }
}