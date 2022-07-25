using System.Collections;
using UnityEngine;

namespace Creatures.Mobs.Wolf
{
    public abstract class MobBasicState
    {
        protected Transform _player;
        protected Mob _mob;
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