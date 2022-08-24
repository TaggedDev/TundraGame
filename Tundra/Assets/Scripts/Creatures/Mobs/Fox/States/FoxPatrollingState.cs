using UnityEngine;
using UnityEngine.AI;

namespace Creatures.Mobs.Fox.States
{
    public class FoxPatrollingState : MobBasicState
    {
        private const float MAX_PATROL_TIME = 10f;
        private float patrolTime = MAX_PATROL_TIME;
        
        public FoxPatrollingState(Mob mob, IMobStateSwitcher switcher, NavMeshAgent agent) : base(mob, switcher, agent)
        {
            _mob.DeltaRotate = _mob.MaxDeltaRotate;
            _mob.MobRigidbody = _mob.GetComponent<Rigidbody>();
            GenerateNewPatrolPoint();
        }

        public override void MoveMob()
        {
            // Thresholding time mob is able to get to target if it's too far away
            patrolTime -= Time.fixedDeltaTime;
            if (patrolTime <= 0f || Vector3.Distance(_mob.targetPoint, _mob.transform.position) <= .5f)
            {
                GenerateNewPatrolPoint();
                patrolTime = MAX_PATROL_TIME;
            }
        }

        public override void SniffForTarget()
        {
            if (Vector3.Distance(_mob.Player.position, _mob.transform.position) <= _mob.SniffingRadius)
            {
                _switcher.SwitchState<FoxHuntingState>();
            }
        }

        /// <summary>
        /// Generates and sets the point within the _roamRadius to move to
        /// </summary>
        private void GenerateNewPatrolPoint()
        {
            bool hasSetPoint = false;
            while (!hasSetPoint)
            {
                Vector3 point = new Vector3(_mob.SpawnPosition.x + Random.Range(-_mob.RoamingRadius, _mob.RoamingRadius), 500, _mob.SpawnPosition.z + Random.Range(-_mob.RoamingRadius, _mob.RoamingRadius));

                if (Physics.Raycast(point, Vector3.down, out RaycastHit hit, Mathf.Infinity, 1 << TERRAIN_LAYER_INDEX))
                {
                    //_mob.Sensor.TargetPosition = new Vector3(point.x, hit.point.y + .5f, point.z);
                    /* SET DESTINATION .... */
                    _mob.targetPoint = new Vector3(point.x, hit.point.y + .1f, point.z);
                    _agent.SetDestination(_mob.targetPoint);
                    hasSetPoint = true;
                }
            }
        }
        
        public override void Start()
        {
            GenerateNewPatrolPoint();
            _mob.MaxMobHealth = _mob.CurrentMobHealth;
            _mob.FearHealthThreshold = _mob.MaxMobHealth * .1f;
        }

        public override void Stop() { }
    }
}