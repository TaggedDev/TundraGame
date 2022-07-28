using System;
using UnityEngine;

namespace Creatures.Mobs
{
    public class CentreOfMass : MonoBehaviour
    {
        [SerializeField] private Vector3 centerOfMass;

        [SerializeField] private bool isAwaken;

        [SerializeField] private Rigidbody _rigidbody;
        // Start is called before the first frame update
        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _rigidbody.centerOfMass = centerOfMass;
        }
        
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(transform.position + transform.rotation * centerOfMass, 1f);
        }
    }
}
