using Creatures.Player.Behaviour;
using Creatures.Player.Magic;
using UnityEngine;

namespace Creatures.Player.Magic
{

    /// <summary>
    /// Script to control crystal channel spell.
    /// </summary>
    public class CrystalChannelScript : SpellScript<CrystalChannelSpell>
    {
        private const double Time = 10;
        private double _lifeTime;
        private PlayerProperties _props;


        private void Start()
        {
            _props = Configuration.Caster.GetComponent<PlayerProperties>();
            _props.CurrentSpeed *= (float)Configuration.MovementFineCoefficient;
        }


        private void Update()
        {
            gameObject.transform.position = Configuration.Caster.transform.position;
            double coefficient = UnityEngine.Time.deltaTime;
            _lifeTime += coefficient;
            if (_lifeTime >= Time)
            {
                Destroy(gameObject);
                _props.CurrentSpeed /= (float)Configuration.MovementFineCoefficient;
            }
            // Heals player
            _props.CurrentHealth += (float)(Configuration.Regeneration * coefficient);
            if (_props.CurrentHealth > _props.MaxHealth) _props.CurrentHealth = _props.MaxHealth;
            //TODO: deal damage to enemy which attacks player.
        }
    }
}