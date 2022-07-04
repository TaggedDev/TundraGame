using UnityEngine;

namespace Environment.Terrain
{
    public class Entity : MonoBehaviour
    {
        private const float ENTITIY_VIEW_RANGE = 3000f;
        private Transform _player;
        private Vector2 _entityPosition;

        public void Initialise(Vector3 position, Transform player)
        {
            _entityPosition = new Vector2(position.x, position.z);
            _player = player;
            transform.localScale /= WorldConstants.Scale;
            transform.position = position;
        }

        public void UpdateSelf()
        {
            Vector2 playerPosition = new Vector2(_player.position.x, _player.position.z);
            if ((playerPosition - _entityPosition).sqrMagnitude <= ENTITIY_VIEW_RANGE)
            {
                gameObject.SetActive(true);
                return;
            }
            gameObject.SetActive(false);
        }
    }
}