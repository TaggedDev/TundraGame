using UnityEngine;

namespace Creatures.Mobs
{
    public abstract class Mob : MonoBehaviour
    {
        protected const int MOB_LAYER_INDEX = 11;

        public MobEntitySensor Sensor
        {
            get => _sensor;
            set => _sensor = value;
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
        public float MoveSpeed
        {
            get => moveSpeed;
            set => moveSpeed = value;
        }
        public float RotationSpeed
        {
            get => rotationSpeed;
            set => rotationSpeed = value;
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

        [SerializeField] private int mobID;
        [SerializeField] private float moveSpeed;
        [SerializeField] private float rotationSpeed;
        [SerializeField] private float maxDeltaRotate;
        [SerializeField] private float roamingRadius;
        [SerializeField] private float sniffingRadius;

        private MobEntitySensor _sensor;
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
        public abstract void Initialise();

        private void OnValidate()
        {
            if (roamingRadius < 5)
                roamingRadius = 5;
        }
    }
}