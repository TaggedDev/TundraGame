using System;
using UnityEngine;

namespace Creatures.Mobs
{
    [RequireComponent(typeof(BoxCollider))]
    public class MobGroundChecker : MonoBehaviour
    {
        private Mob _owner;
        private BoxCollider _groundCollider;
        private void Start()
        {
            _owner = GetComponentInParent<Mob>();
            _groundCollider = GetComponent<BoxCollider>();
        }

        private void OnTriggerEnter(Collider other)
        {
            throw new NotImplementedException();
        }
    }
}