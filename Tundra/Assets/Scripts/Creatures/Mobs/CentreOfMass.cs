using System;
using UnityEngine;

namespace Creatures.Mobs
{
    public class CentreOfMass : MonoBehaviour
    {
        [SerializeField] private Vector3 CenterOfMass;

        [SerializeField] private bool IsAwaken;

        [SerializeField] private Rigidbody _rigidbody;
        // Start is called before the first frame update
        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _rigidbody.centerOfMass = CenterOfMass;
        }
        
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(transform.position + transform.rotation * CenterOfMass, 1f);
        }
    }
}
