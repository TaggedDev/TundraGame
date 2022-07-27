using UnityEngine;
using Random = UnityEngine.Random;

namespace Creatures.Mobs
{
    /// <summary>
    /// Abstract fabric class that is used to instantiate mobs around player
    /// </summary>
    public class MobFabric : MonoBehaviour
    {
        private const float innerCircleRadius = 50f;
        private const float outerCircleRadius = 60f;
        [SerializeField] private int maxMobsCapacity;
        [SerializeField] private Transform player;
        [SerializeField] private Mob mobPrefab;
        private float halfYSize;
        private int currentMobsCount;
        private const int TERRAIN_LAYER_INDEX = 8;

        private void Start()
        {
            halfYSize = mobPrefab.GetComponent<Collider>().bounds.size.y / 2;
            currentMobsCount = 0;
            if (maxMobsCapacity == 0)
                Debug.Log($"Max Mob Capacity is 0 for {name}");
        }

        private void Update()
        {
            if (currentMobsCount < maxMobsCapacity)
                SpawnMob();
        }

        /// <summary>
        /// Spawn mob somewhere in the world with patrolling as a basic state 
        /// </summary>
        private void SpawnMob()
        {
            Vector3 position = GenerateMobPosition();
            // V3 zero means that mob has spawned not on terrain layer
            if (position == Vector3.zero)
                return;
            
            Mob mob = Instantiate(mobPrefab, position, Quaternion.identity, transform);
            mob.Initialise();
            currentMobsCount++;
        }

        /// <summary>
        /// Generates Vector3 position where animal will be spawned
        /// </summary>
        /// <returns>Vector3 coordinates - coordinates for animal to spawn. Returns V3.zero if spawn point is not
        /// a terrain</returns>
        private Vector3 GenerateMobPosition()
        {
            float radius = Random.Range(innerCircleRadius, outerCircleRadius);
            float angle = Random.Range(1, 360);
            float xPosition = Mathf.Cos(angle) * radius;
            float zPosition = Mathf.Sin(angle) * radius;

            // Raycasts at XZ down to check if we land on the ground (not a tree or other entity)
            if (Physics.Raycast(new Vector3(xPosition, 500, zPosition), Vector3.down, out RaycastHit hit, Mathf.Infinity))
            {
                // Check if we land on environment layer
                if (hit.transform.gameObject.layer == TERRAIN_LAYER_INDEX)
                    return new Vector3(xPosition, hit.point.y + halfYSize + 1f, zPosition);
                return Vector3.zero;
            }
            return Vector3.zero;
        }
    }
}