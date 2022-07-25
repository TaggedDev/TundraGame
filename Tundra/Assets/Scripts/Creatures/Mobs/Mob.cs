using UnityEngine;

namespace Creatures.Mobs
{
    public abstract class Mob : MonoBehaviour
    {
        protected const int MOB_LAYER_INDEX = 11;
        
        public RaycastHit SlopeHit
        {
            get => _slopeHit;
            set => _slopeHit = value;
        }
        public Rigidbody MobRigidbody
        {
            get => _mobMobRigidbody;
            set => _mobMobRigidbody = value;
        }
        public float DeltaRotate
        {
            get => _deltaRotate;
            set => _deltaRotate = value;
        }
        public float MaxDeltaRotate
        {
            get => _maxDeltaRotate;
            set => _maxDeltaRotate = value;
        }
        public float MoveSpeed
        {
            get => _moveSpeed;
            set => _moveSpeed = value;
        }
        public float RotationSpeed
        {
            get => _rotationSpeed;
            set => _rotationSpeed = value;
        }
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

        [SerializeField] private float _moveSpeed;
        [SerializeField] private float _rotationSpeed;
        [SerializeField] private float _maxDeltaRotate;

        private RaycastHit _slopeHit;
        private Rigidbody _mobMobRigidbody;
        private float _deltaRotate;

        private bool _isEntitySensed;
        private bool _ignoreSensor;

        /// <summary>
        /// Initialises basic parameters. Can't use constructor because objects with this class are initialized by
        /// instantiate method during the game
        /// </summary>
        /// <param name="playerParameter">Link to player in scene</param>
        public abstract void Initialise(Transform playerParameter);
    }
}