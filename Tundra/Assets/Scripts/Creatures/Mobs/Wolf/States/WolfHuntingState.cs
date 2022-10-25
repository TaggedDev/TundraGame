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
        }

        public override void MoveMob()
        {
            float dist = Vector3.Distance(_mob.transform.position, _mob.Player.position);
            
            // If wolf has reached the player, he gets into preparing mode
            if (dist <= ATTACK_DISTANCE_THRESHOLD)
            {
                _switcher.SwitchState<WolfPreparingState>();
                return;
            }
            
            if (dist > _mob.SniffingRadius)
            {
                _switcher.SwitchState<WolfPatrollingState>();
                return;
            }
            
            _agent.SetDestination(_mob.Player.position);
            _mob.LookAtPosition(_mob.Player.position);
        }
        
        public override void SniffForTarget()
        {
            /*var colliders = Physics.OverlapSphere(_mob.transform.position, _mob.SniffingRadius,
                (1 << MOBS_LAYER_INDEX) | (1 << PLAYER_LAYER_INDEX));
            
            // There is always 1 object in overlap sphere (self)
            if (colliders.Length <= 1) _switcher.SwitchState<FoxPatrollingState>();*/
        }
        
        public override void Start()
        {
            Debug.Log("Hunting state");
        }

        public override void Stop() { }
    }
}