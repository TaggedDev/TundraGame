using UnityEngine;

namespace Creatures.Mobs
{
    public abstract class MobBasicState
    {
        protected readonly Transform _player;
        protected readonly Mob _mob;
        protected IMobStateSwitcher _switcher;

        protected MobBasicState(Transform player, Mob mob, IMobStateSwitcher switcher)
        {
            _mob = mob;
            _player = player;
            _switcher = switcher;
        }

        /// <summary>
        /// Moves mob to chosen target
        /// </summary>
        public abstract void MoveMob();
    }
}