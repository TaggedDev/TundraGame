using UnityEngine;

namespace Creatures.Mobs
{
    public abstract class Mob : MonoBehaviour
    {
        private bool _isEntitySensed;
        private bool _ignoreSensor;

        public bool IsEntitySensed
        {
            get => _isEntitySensed;
            set => _isEntitySensed = value;
        }
        
        public bool IgnoreSensor
        {
            get => _ignoreSensor;
            set => _ignoreSensor = value;
        }
        
        /// <summary>
        /// Initialises basic parameters. Can't use constructor because objects with this class are initialized by
        /// instantiate method during the game
        /// </summary>
        /// <param name="playerParameter">Link to player in scene</param>
        public abstract void Initialise(Transform playerParameter);
    }
}