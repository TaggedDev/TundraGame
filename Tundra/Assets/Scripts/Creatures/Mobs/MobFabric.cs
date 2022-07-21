using UnityEngine;

namespace Creatures.Mobs
{
    /// <summary>
    /// Abstract fabric class that is used to instantiate mobs around player
    /// </summary>
    public abstract class MobFabric : MonoBehaviour
    {
        /// <summary>
        /// Spawn mob somewhere in the world with patrolling as a basic state 
        /// </summary>
        public abstract void SpawnMob();
    }
}