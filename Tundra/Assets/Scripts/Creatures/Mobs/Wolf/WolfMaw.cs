using Creatures.Player.Behaviour;
using Environment;
using UnityEngine;

namespace Creatures.Mobs.Wolf
{
    public class WolfMaw : MonoBehaviour
    {
        private readonly Collider[] _colliders = new Collider[1];
        private PlayerProperties _player;

        public void Initialise(PlayerProperties player)
        {
            _player = player;
        }
        
        private void FixedUpdate()
        {
            Physics.OverlapSphereNonAlloc(transform.position, 10f, _colliders, 1 << WorldConstants.PLAYER_LAYER_INDEX);
            if (_colliders[0] is null)
                return;

            _player.CurrentHealth -= 30f;
            gameObject.SetActive(false);
        }
    }
}