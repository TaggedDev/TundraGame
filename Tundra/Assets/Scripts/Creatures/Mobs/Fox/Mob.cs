using UnityEngine;
using UnityEngine.AI;

namespace Creatures.Mobs.Fox
{
    public abstract class Mob : MonoBehaviour
    {
        protected const int MOB_LAYER_INDEX = 11;
        protected const int TERRAIN_LAYER_INDEX = 8;
        public Vector3 targetPoint;

        public Transform Player
        {
            get => _player;
            set => _player = value;
        }
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
            get => maxDeltaRotate;
            set => maxDeltaRotate = value;
        }
        public float RotationSpeed
        {
            get => rotationSpeed;
            set => rotationSpeed = value;
        }
        public float MaxMobHealth
        {
            get => maxMobHealth;
            set => maxMobHealth = value;
        }
        public float CurrentMobHealth
        {
            get => _currentMobHealth;
            set => _currentMobHealth = value;
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
            get => roamingRadius;
            set => roamingRadius = value;
        }
        public float SniffingRadius
        {
            get => sniffingRadius;
            set => sniffingRadius = value;
        }
        public int MobID
        {
            get => mobID;
            set => mobID = value;
        }
        public bool IsGrounded
        {
            get => _isGrounded;
            set => _isGrounded = value;
        }
        public float FearHealthThreshold
        {
            get => _fearHealthThreshold;
            set => _fearHealthThreshold = value;
        }
        public Rigidbody Rigidbody
        {
            get => _rigidbody;
            set => _rigidbody = value;
        }
        public NavMeshAgent Agent
        {
            get => _agent;
            set => _agent = value;
        }

        [SerializeField] private int mobID;
        [SerializeField] private float rotationSpeed;
        [SerializeField] private float maxDeltaRotate;
        [SerializeField] private float roamingRadius;
        [SerializeField] private float sniffingRadius;
        [SerializeField] private float maxMobHealth;
        [SerializeField] private Transform _player;

        private NavMeshAgent _agent;
        private RaycastHit _slopeHit;
        private Rigidbody _mobRigidbody;
        private Vector3 _spawnPosition;
        [SerializeField] private float _currentMobHealth;
        private float _fearHealthThreshold;
        private float _deltaRotate;
        private bool _isEntitySensed;
        private bool _isIgnoringSensor;
        [SerializeField] private bool _isGrounded;
        private Rigidbody _rigidbody;

        /// <summary>
        /// Initialises basic parameters. Can't use constructor because objects with this class are initialized by
        /// instantiate method during the game
        /// </summary>
        public abstract void Initialise();

        private void OnValidate()
        {
            if (roamingRadius < 5)
                roamingRadius = 5;
        }
    }
}