using System.Collections;
using UnityEngine;

namespace Creatures.Mobs.Wolf
{
    public class WolfHuntingState : MobBasicState
    {
        public WolfHuntingState(Transform player, Mob mob, IMobStateSwitcher switcher) : base(player, mob, switcher)
        {
            _mob.DeltaRotate = _mob.MaxDeltaRotate;
            _mob.MobRigidbody = _mob.GetComponent<Rigidbody>();
        }

        public override void MoveMob()
        {
            if (_mob.MobRigidbody.velocity.magnitude > _mob.MoveSpeed)
                _mob.MobRigidbody.velocity = _mob.MobRigidbody.velocity.normalized * _mob.MoveSpeed;
            
            _mob.MobRigidbody.AddForce(NormalizeSlopeMovement() * (_mob.MoveSpeed * 100f), ForceMode.Force);
            
            // Prevent sliding down when on high slope
            if (_mob.MobRigidbody.velocity.y > 0)
                _mob.MobRigidbody.AddForce(Vector3.down * 80f, ForceMode.Force);

            if (_mob.IsEntitySensed && !_mob.IgnoreSensor)
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
                    _mob.StartCoroutine(FacePlayer());
                }
            }
        }
        
        private IEnumerator FacePlayer()
        {           
            Quaternion lookRotation = Quaternion.LookRotation(_player.position - _mob.transform.position);
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