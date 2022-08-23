using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Creatures.Mobs.Fox.States
{
    public class FoxEscapingState : MobBasicState
    {
        private Transform _dangerSource;
        private bool _isFearless;
        
        public FoxEscapingState(Mob mob, IMobStateSwitcher switcher, NavMeshAgent agent) : base(mob, switcher, agent) { }

        public override void Start() { }

        public override void Stop()
        { }

        public override void MoveMob()
        {
            _mob.StartCoroutine(SetEscapeDestinationPoint());
            _agent.Move(Vector3.forward);
        }

        private IEnumerator SetEscapeDestinationPoint()
        {
            Quaternion lookRotation = Quaternion.LookRotation(_dangerSource.position - _mob.transform.position);
            lookRotation *= Quaternion.Euler(0f, 180f, 0f);
            float time = 0;
            while (time < .3f)
            {
                _mob.transform.rotation = Quaternion.Slerp(_mob.transform.rotation, lookRotation, time);
                time += Time.fixedDeltaTime * _mob.RotationSpeed;
                yield return null;
            }
        }
        
        private IEnumerator TurnAsideDangerSource()
        {           
            Quaternion lookRotation = Quaternion.LookRotation(_dangerSource.position - _mob.transform.position);
            lookRotation *= Quaternion.Euler(0f, 180f, 0f);
            float time = 0;
            while (time < .3f)
            {
                _mob.transform.rotation = Quaternion.Slerp(_mob.transform.rotation, lookRotation, time);
                time += Time.fixedDeltaTime * _mob.RotationSpeed;
                yield return null;
            }
        }

        public override void SniffForTarget()
        {
            if (_isFearless)
                return;
            
            var colliders = Physics.OverlapSphere(_mob.transform.position, _mob.SniffingRadius,
                (1 << MOBS_LAYER_INDEX) | (1 << PLAYER_LAYER_INDEX));
            
            // There is always 1 object in overlap sphere (self)
            if (colliders.Length == 1) _switcher.SwitchState<FoxPatrollingState>();
        }
    }
}