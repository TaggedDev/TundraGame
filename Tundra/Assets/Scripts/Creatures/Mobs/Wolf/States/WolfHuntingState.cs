using UnityEngine;
using UnityEngine.AI;

namespace Creatures.Mobs.Wolf.States
{
    public class WolfHuntingState : MobBasicState
    {
        private const float ATTACK_DISTANCE_THRESHOLD = 1f;
        public WolfHuntingState(Mob mob, IMobStateSwitcher switcher, NavMeshAgent agent) : base(mob, switcher, agent)
        {
            _mob.DeltaRotate = _mob.MaxDeltaRotate;
            _mob.MobRigidbody = _mob.GetComponent<Rigidbody>();
        }

        public override void MoveMob()
        {
            float dist = Vector3.Distance(_mob.transform.position, _mob.Player.position);
            
            // If wolf has reached the player, he gets into preparing mode
            if (dist <= ATTACK_DISTANCE_THRESHOLD)
                _switcher.SwitchState<WolfPreparingState>();
            
            _agent.SetDestination(_mob.Player.position);
            LookAtPosition(_mob.Player.position);
            _agent.updateRotation = true;
        }
        
        /// <summary>
        /// Rotates wolf to face the player
        /// </summary>
        /// <param name="spot"></param>
        private void LookAtPosition(Vector3 spot)
        {
            Vector3 direction = spot - _mob.transform.position;
            direction.y = 0;
            Quaternion rotation = Quaternion.LookRotation(direction);
            _mob.transform.rotation = Quaternion.Lerp(_mob.transform.rotation, rotation,
                Time.deltaTime * _mob.RotationSpeed);
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