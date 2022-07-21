using UnityEngine;

namespace Creatures.Mobs.Fox
{
    /// <summary>
    /// Fabric that instantiates fox objects around player
    /// </summary>
    public class FoxFabric : MobFabric
    {
        [SerializeField] private FoxMovement foxPrefab;
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
            FoxMovement fox = Instantiate(foxPrefab, new Vector3(100, 20, 100), Quaternion.identity, transform);
            fox.Initialise(player);
        }
    }
}