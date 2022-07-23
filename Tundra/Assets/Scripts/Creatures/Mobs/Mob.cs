﻿using UnityEngine;

namespace Creatures.Mobs
{
    public abstract class Mob : MonoBehaviour
    {
        /// <summary>
        /// Initialises basic parameters. Can't use constructor because objects with this class are initialized by
        /// instantiate method during the game
        /// </summary>
        /// <param name="playerParameter">Link to player in scene</param>
        public abstract void Initialise(Transform playerParameter);
    }
}