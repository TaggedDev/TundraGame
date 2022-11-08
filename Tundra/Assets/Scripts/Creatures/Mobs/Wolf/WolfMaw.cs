using Creatures.Player.Behaviour;
using Environment;
using JetBrains.Annotations;
using UnityEngine;

namespace Creatures.Mobs.Wolf
{
    public class WolfMaw : MonoBehaviour
    {
        private readonly Collider[] _colliders = new Collider[1];
        private PlayerProperties _playerProperties;
        private Rigidbody _playerRigidbody;

        public void Initialise(PlayerProperties playerProperties, Rigidbody playerRigidbody)
        {
            _playerProperties = playerProperties;
            _playerRigidbody = playerRigidbody;

        }
        
        private void FixedUpdate()
        {
            Physics.OverlapSphereNonAlloc(transform.position, 10f, _colliders, 1 << WorldConstants.PLAYER_LAYER_INDEX);
            if (_colliders[0] is null)
                return;

            _playerRigidbody.AddForce((Vector3.up + Vector3.left) * 100f * _playerRigidbody.mass);
            _playerProperties.CurrentHealthPoints -= 30f;
            gameObject.SetActive(false);
        }
    }
}