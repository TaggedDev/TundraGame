using UnityEngine;
using UnityEngine.AI;

namespace Creatures.Mobs.Wolf.States
{
    public class WolfEscapingState : MobBasicState
    {
        public WolfEscapingState(Mob mob, IMobStateSwitcher switcher, NavMeshAgent agent) : base(mob, switcher, agent) { }

        public override void Start() { }

        public override void Stop()
        { }

        public override void MoveMob()
        {
            Vector3 awayDirection = _mob.transform.position - _mob.Player.transform.position;
            _agent.SetDestination(_mob.transform.position + awayDirection);
        }

        
        public override void SniffForTarget()
        {
            if (Vector3.Distance(_mob.Player.position, _mob.transform.position) > _mob.SniffingRadius)
            {
                _switcher.SwitchState<WolfPatrollingState>();
            }
        }
    }
}