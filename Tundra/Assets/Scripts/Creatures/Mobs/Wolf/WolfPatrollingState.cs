using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Creatures.Mobs.Wolf
{
    public class WolfPatrollingState : MobBasicState
    {
        private const float _maxPatrolTime = 5f;
        private float patrolTime = _maxPatrolTime;
        
        public WolfPatrollingState(Mob mob, IMobStateSwitcher switcher) : base(mob, switcher)
        {
            _mob.DeltaRotate = _mob.MaxDeltaRotate;
            _mob.MobRigidbody = _mob.GetComponent<Rigidbody>();
        }

        public override void MoveMob()
        {
            // Thresholding time mob is able to get to target if it's too high
            patrolTime -= Time.fixedDeltaTime;
            if (Vector3.Distance(_mob.transform.position, _targetPosition) <= 4f || patrolTime <= 0f)
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
                _targetPosition = new Vector3(point.x, hit.point.y, point.z);
            }
        } 

        /// <summary>
        /// Rotates self to the roaming point
        /// </summary>
        /// <returns></returns>
        private IEnumerator FaceTarget()
        {            
            Quaternion lookRotation = Quaternion.LookRotation(_targetPosition - _mob.transform.position);
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
    }
}