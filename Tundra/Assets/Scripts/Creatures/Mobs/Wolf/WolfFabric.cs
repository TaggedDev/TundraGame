using UnityEngine;

namespace Creatures.Mobs.Wolf
{
    /// <summary>
    /// Fabric that instantiates wolf objects around player
    /// </summary>
    public class WolfFabric : MobFabric
    {
        [SerializeField] private WolfMovement wolfPrefab;
        [SerializeField] private Transform player;

        /// <summary>
        /// Temporary solution. Mob spawning will be implemented with counter in update method
        /// </summary>
        private void Start()
        {
            SpawnMob();
        }

        public override void SpawnMob()
        {
            WolfMovement wolf = Instantiate(wolfPrefab, new Vector3(100, 20, 100), Quaternion.identity, transform);
            wolf.Initialise(player);
        }
    }
}