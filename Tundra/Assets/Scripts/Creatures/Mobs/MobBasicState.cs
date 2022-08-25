using UnityEngine.AI;

namespace Creatures.Mobs
{
    public abstract class MobBasicState
    {
        protected const int TERRAIN_LAYER_INDEX = 8;
        protected const int MOBS_LAYER_INDEX = 11;
        protected const int PLAYER_LAYER_INDEX = 9;
        
        protected readonly Mob _mob;
        protected readonly IMobStateSwitcher _switcher;
        protected readonly NavMeshAgent _agent;

        protected MobBasicState(Mob mob, IMobStateSwitcher switcher, NavMeshAgent agent)
        {
            _mob = mob;
            _switcher = switcher;
            _agent = agent;
        }

        /// <summary>
        /// Is called on state launch 
        /// </summary>
        public abstract void Start();

        /// <summary>
        /// Is called before state is switched to another one  
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