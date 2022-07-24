using System.Collections;
using UnityEngine;

namespace Creatures.Mobs.Wolf
{
    public class WolfMovement : Mob
    {
        [SerializeField] private Transform player;
        [SerializeField] private float maxDeltaRotate;
        [SerializeField] private float speed;
        [SerializeField] private float rotationSpeed;
        
        
        private const int MOB_LAYER = 11;
        
        private RaycastHit _slopeHit;
        private Rigidbody _rigidbody;
        private float _deltaRotate;

        public override void Initialise(Transform playerParameter)
        {
            player = playerParameter;
            transform.gameObject.layer = MOB_LAYER;
        }
        
        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _deltaRotate = maxDeltaRotate;
        }

        private void FixedUpdate()
        {
            if (_rigidbody.velocity.magnitude > speed)
                _rigidbody.velocity = _rigidbody.velocity.normalized * speed;
            
            _rigidbody.AddForce(NormalizeSlopeMovement() * (speed * 100f), ForceMode.Force);
            
            // Prevent sliding down when on high slope
            if (_rigidbody.velocity.y > 0)
                _rigidbody.AddForce(Vector3.down * 80f, ForceMode.Force);
            
            
            
            if (IsEntitySensed && !IgnoreSensor)
            {
                transform.Rotate(0, 22.5f, 0);
                _deltaRotate = maxDeltaRotate;
            }
            else
            {
                if (_deltaRotate > 0)
                {
                    _deltaRotate -= Time.deltaTime;
                }
                else
                {
                    StartCoroutine(FacePlayer());
                }
            }
        }

        /// <summary>
        /// Rotates mob towards player
        /// </summary>
        private IEnumerator FacePlayer()
        {
            Quaternion lookRotation = Quaternion.LookRotation(player.position - transform.position);
            float time = 0;
            while (time < .3f)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, time);
                time += Time.fixedDeltaTime * rotationSpeed;
                yield return null;
            }
        }
        
        /// <summary>
        /// Normalizes movement on higher slopes
        /// </summary>
        /// <returns>Normalized Vector 3 - direction to move</returns>
        private Vector3 NormalizeSlopeMovement()
        {
            return Vector3.ProjectOnPlane(transform.forward, _slopeHit.normal).normalized;
        }
    }
}