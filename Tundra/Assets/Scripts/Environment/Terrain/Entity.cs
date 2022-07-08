using UnityEngine;

namespace Environment.Terrain
{
    /// <summary>
    /// This class describes an entity that is spawned on map.
    /// </summary>
    public class Entity : MonoBehaviour
    {
        [Range(0, 1)] [SerializeField] private float spawnRateForLevel;

        public float SpawnRateForLevel => spawnRateForLevel;
        private const float ENTITY_VIEW_RANGE = 3000f;
        private Transform _player;
        private Vector2 _entityPosition;

        /// <summary>
        /// Due Entities are spawned as Initialise() function, there is no built-in constructor for this method.
        /// Call this function right after prop is initialised.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="player"></param>
        public void Initialise(Vector3 position, Transform player)
        {
            _entityPosition = new Vector2(position.x, position.z);
            _player = player;
            transform.localScale /= WorldConstants.Scale;
            transform.localScale *= Random.Range(0.8f, 1.2f);
            transform.rotation = Quaternion.Euler(0, Random.Range(0f, 360f), 0);
            transform.position = position;
        }

        /// <summary>
        /// Updates current entity's visibility.
        /// </summary>
        public void UpdateSelf()
        {
            Vector2 playerPosition = new Vector2(_player.position.x, _player.position.z);
            if ((playerPosition - _entityPosition).sqrMagnitude <= ENTITY_VIEW_RANGE)
            {
                gameObject.SetActive(true);
                return;
            }
            gameObject.SetActive(false);
        }
    }
}