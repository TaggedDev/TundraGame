using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Creatures.Mobs.Wolf
{
    public class WolfPatrollingState : MobBasicState
    {
        private const float _maxPatrolTime = 10f;
        private float patrolTime = _maxPatrolTime;
        
        public WolfPatrollingState(Mob mob, IMobStateSwitcher switcher) : base(mob, switcher)
        {
            _mob.Sensor = _mob.GetComponentInChildren<MobEntitySensor>();
            _mob.DeltaRotate = _mob.MaxDeltaRotate;
            _mob.MobRigidbody = _mob.GetComponent<Rigidbody>();
        }

        public override void MoveMob()
        {
            // Thresholding time mob is able to get to target if it's too high
            patrolTime -= Time.fixedDeltaTime;
            if (Vector3.Distance(_mob.transform.position, _mob.Sensor.TargetPosition) <= 4f || patrolTime <= 0f)
            {
                GenerateNewPatrolPoint();
                patrolTime = _maxPatrolTime;
            }

            // Thresholding mob's velocity on slopes
            if (_mob.MobRigidbody.velocity.magnitude > _mob.MoveSpeed)
                _mob.MobRigidbody.velocity = _mob.MobRigidbody.velocity.normalized * _mob.MoveSpeed;
            
            _mob.MobRigidbody.AddForce(NormalizeSlopeMovement() * (_mob.MoveSpeed * 100f), ForceMode.Force);
            
            // Prevent sliding down when on high slope
            if (_mob.MobRigidbody.velocity.y > 0)
                _mob.MobRigidbody.AddForce(Vector3.down * 80f, ForceMode.Force);

            if (_mob.IsEntitySensed && !_mob.IsIgnoringSensor)
            {
                _mob.transform.Rotate(0, 22.5f, 0);
                _mob.DeltaRotate = _mob.MaxDeltaRotate;
            }
            else
            {
                if (_mob.DeltaRotate > 0)
                {
                    _mob.DeltaRotate -= Time.deltaTime;
                }
                else
                {
                    _mob.StartCoroutine(FaceTarget());
                }
            }
        }

        public override void SniffForTarget()
        {
            var colliders = Physics.OverlapSphere(_mob.transform.position, _mob.SniffingRadius,
               (1 << MOBS_LAYER_INDEX) | (1 << PLAYER_LAYER_INDEX));
            
            // There is always 1 object in overlap sphere (self)
            if (colliders.Length <= 1) return;
            
            for (int i = 1; i < colliders.Length; i++)
            {
                var mob = colliders[i].GetComponent<Mob>();
                if (!mob)
                {
                    // if mob is null -> object is a player;
                    _mob.Sensor.Target = colliders[i].transform;
                    _switcher.SwitchState<WolfHuntingState>();
                }
                else
                {
                    // if mob is not null -> check mobID -> differs? go hunting
                    if (mob.MobID == _mob.MobID)
                        continue;
                    _mob.Sensor.Target = mob.transform;
                }
            }
        }

        /// <summary>
        /// Gets a random point to roam to within _roamRadius
        /// </summary>
        /// <returns>Vector3 coordinates of the point</returns>
        private void GenerateNewPatrolPoint()
        {
            Vector3 point = new Vector3(_mob.SpawnPosition.x + Random.Range(-_mob.RoamingRadius, _mob.RoamingRadius),
                500, _mob.SpawnPosition.z + Random.Range(-_mob.RoamingRadius, _mob.RoamingRadius));
            
            if (Physics.Raycast(point, Vector3.down, out RaycastHit hit,
                    Mathf.Infinity, 1 << TERRAIN_LAYER_INDEX))
            {
                _mob.Sensor.TargetPosition = new Vector3(point.x, hit.point.y, point.z);
            }
        } 

        /// <summary>
        /// Rotates self to the roaming point
        /// </summary>
        /// <returns></returns>
        private IEnumerator FaceTarget()
        {
            Quaternion lookRotation = Quaternion.LookRotation(_mob.Sensor.TargetPosition - _mob.transform.position);
            float time = 0;
            while (time < .3f)
            {
                _mob.transform.rotation = Quaternion.Slerp(_mob.transform.rotation, lookRotation, time);
                time += Time.fixedDeltaTime * _mob.RotationSpeed;
                yield return null;
            }
        }

        private Vector3 NormalizeSlopeMovement()
        {
            return Vector3.ProjectOnPlane(_mob.transform.forward, _mob.SlopeHit.normal).normalized;
        }
        
        public override void Start()
        {
            GenerateNewPatrolPoint();
        }

        public override void Stop()
        {
            
        }
    }
}