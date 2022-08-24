using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Creatures.Mobs.Fox.States
{
    public class FoxEscapingState : MobBasicState
    {
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
            Quaternion lookRotation = Quaternion.LookRotation(_mob.Player.position - _mob.transform.position);
            lookRotation *= Quaternion.Euler(0f, 180f, 0f);
            float time = 0;
            while (time < .3f)
            {
                _mob.transform.rotation = Quaternion.Slerp(_mob.transform.rotation, lookRotation, time);
                time += Time.fixedDeltaTime * _mob.RotationSpeed;
                yield return null;
            }
        }
        
        /*private IEnumerator TurnAsideDangerSource()
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
        }*/

        public override void SniffForTarget()
        {
            if (Vector3.Distance(_mob.Player.position, _mob.transform.position) > _mob.SniffingRadius)
            {
                _switcher.SwitchState<FoxPatrollingState>();
            }
        }
    }
}