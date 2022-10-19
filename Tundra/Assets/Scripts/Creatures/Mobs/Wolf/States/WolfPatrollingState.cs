using UnityEngine;
using UnityEngine.AI;

namespace Creatures.Mobs.Wolf.States
{
    public class WolfPatrollingState : MobBasicState
    {
        private const float MAX_PATROL_TIME = 10f;
        private float patrolTime = MAX_PATROL_TIME;
        
        public WolfPatrollingState(Mob mob, IMobStateSwitcher switcher, NavMeshAgent agent) : base(mob, switcher, agent)
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
                LookAtPosition(_mob.targetPoint);
                patrolTime = MAX_PATROL_TIME;
            }
        }

        public override void SniffForTarget()
        {
            if (Vector3.Distance(_mob.Player.position, _mob.transform.position) <= _mob.SniffingRadius)
                _switcher.SwitchState<WolfHuntingState>();
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
        
        /// <summary>
        /// Rotates mob to face the spot he is moving towards to
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
        
        public override void Start()
        {
            GenerateNewPatrolPoint();
            _mob.MaxMobHealth = _mob.CurrentMobHealth;
            _mob.FearHealthThreshold = _mob.MaxMobHealth * .1f;
        }

        public override void Stop() { }
    }
}