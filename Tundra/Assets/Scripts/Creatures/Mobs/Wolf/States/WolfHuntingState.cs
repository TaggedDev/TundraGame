using UnityEngine;
using UnityEngine.AI;

namespace Creatures.Mobs.Wolf.States
{
    public class WolfHuntingState : MobBasicState
    {
        public WolfHuntingState(Mob mob, IMobStateSwitcher switcher, NavMeshAgent agent) : base(mob, switcher, agent)
        {
            _mob.DeltaRotate = _mob.MaxDeltaRotate;
            _mob.MobRigidbody = _mob.GetComponent<Rigidbody>();
        }

        public override void MoveMob()
        {
            if (_mob.CurrentMobHealth <= _mob.FearHealthThreshold)
                _switcher.SwitchState<WolfEscapingState>();

            _agent.SetDestination(_mob.Player.position);
        }

        public override void SniffForTarget()
        {
            /*var colliders = Physics.OverlapSphere(_mob.transform.position, _mob.SniffingRadius,
                (1 << MOBS_LAYER_INDEX) | (1 << PLAYER_LAYER_INDEX));
            
            // There is always 1 object in overlap sphere (self)
            if (colliders.Length <= 1) _switcher.SwitchState<FoxPatrollingState>();*/
        }

        /*private IEnumerator FacePlayer()
        {           
            Quaternion lookRotation = Quaternion.LookRotation(_mob.Sensor.Target.position - _mob.transform.position);
            float time = 0;
            while (time < .3f)
            {
                _mob.transform.rotation = Quaternion.Slerp(_mob.transform.rotation, lookRotation, time);
                time += Time.fixedDeltaTime * _mob.RotationSpeed;
                yield return null;
            }
        }*/

        private Vector3 NormalizeSlopeMovement() =>
            Vector3.ProjectOnPlane(_mob.transform.forward, _mob.SlopeHit.normal).normalized;
        
        public override void Start() { Debug.Log("Hunting state");}

        public override void Stop() { }
    }
}