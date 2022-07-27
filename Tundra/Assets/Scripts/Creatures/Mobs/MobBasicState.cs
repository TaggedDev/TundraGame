using UnityEngine;

namespace Creatures.Mobs
{
    public abstract class MobBasicState
    {
        protected const int TERRAIN_LAYER_INDEX = 8;
        protected const int MOBS_LAYER_INDEX = 11;
        protected const int PLAYER_LAYER_INDEX = 9;
        
        protected readonly Mob _mob;
        protected IMobStateSwitcher _switcher;

        protected MobBasicState(Mob mob, IMobStateSwitcher switcher)
        {
            _mob = mob;
            _switcher = switcher;
        }

        /// <summary>
        /// Is called on state launch 
        /// </summary>
        public abstract void Start();

        /// <summary>
        /// Is called on when state was switched 
        /// </summary>
        public abstract void Stop();

        /// <summary>
        /// Moves mob to chosen target
        /// </summary>
        public abstract void MoveMob();
        
        /// <summary>
        /// Checks if there are mobs within sniffing radius 
        /// </summary>
        public abstract void SniffForTarget();
    }
}