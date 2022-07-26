using UnityEngine;

namespace Creatures.Mobs
{
    public abstract class Mob : MonoBehaviour
    {
        protected const int MOB_LAYER_INDEX = 11;
        protected const int TERRAIN_LAYER_INDEX = 8;
        
        public RaycastHit SlopeHit
        {
            get => _slopeHit;
            set => _slopeHit = value;
        }
        public Vector3 SpawnPosition
        {
            get => _spawnPosition;
            set => _spawnPosition = value;
        }
        public Rigidbody MobRigidbody
        {
            get => _mobRigidbody;
            set => _mobRigidbody = value;
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
        public bool IsIgnoringSensor
        {
            get => _isIgnoringSensor;
            set => _isIgnoringSensor = value;
        }
        public float RoamingRadius
        {
            get => _roamingRadius;
            set => _roamingRadius = value;
        }
        public float SniffingRadius
        {
            get => _sniffingRadius;
            set => _sniffingRadius = value;
        }

        [SerializeField] private float _moveSpeed;
        [SerializeField] private float _rotationSpeed;
        [SerializeField] private float _maxDeltaRotate;
        [SerializeField] private float _roamingRadius;
        [SerializeField] private float _sniffingRadius;

        private RaycastHit _slopeHit;
        private Rigidbody _mobRigidbody;
        private Vector3 _spawnPosition;
        private float _deltaRotate;
        private bool _isEntitySensed;
        private bool _isIgnoringSensor;

        /// <summary>
        /// Initialises basic parameters. Can't use constructor because objects with this class are initialized by
        /// instantiate method during the game
        /// </summary>
        /// <param name="playerParameter">Link to player in scene</param>
        public abstract void Initialise(Transform playerParameter);

        private void OnValidate()
        {
            if (_roamingRadius < 5)
                _roamingRadius = 5;
        }
    }
}