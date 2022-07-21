using UnityEngine;

namespace Creatures.Mobs
{
    /// <summary>
    /// Defines methods that are in common with all mobs in game
    /// </summary>
    public interface IMob
    {
        /// <summary>
        /// Initialises basic parameters. Can't use constructor because objects with this class are initialized by
        /// instantiate method during the game
        /// </summary>
        /// <param name="playerParameter">Link to player in scene</param>
        void Initialise(Transform playerParameter);
    }
}