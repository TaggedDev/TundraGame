using UnityEngine;

namespace Environment.Terrain
{
    public class Entity : MonoBehaviour
    {
        private Transform _player;
        private Vector2 _entityPosition;

        public void Initialise(Vector2 position, Transform player)
        {
            _entityPosition = position;
            _player = player;
        }

        public void UpdateSelf()
        {
            Vector2 playerPosition = new Vector2(_player.position.x, _player.position.z);
            if ((playerPosition - _entityPosition).sqrMagnitude <= 50f)
            {
                gameObject.SetActive(true);
                return;
            }
            gameObject.SetActive(false);
        }
    }
}