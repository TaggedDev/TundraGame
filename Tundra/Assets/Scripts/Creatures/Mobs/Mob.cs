using Creatures.Player.Inventory;
using UnityEngine;
using UnityEngine.AI;

namespace Creatures.Mobs
{
    public abstract class Mob : MonoBehaviour
    {
        protected const int MOB_LAYER_INDEX = 11;
        protected const int TERRAIN_LAYER_INDEX = 10;
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
        public NavMeshAgent Agent
        {
            get => _agent;
            set => _agent = value;
        }
        public MobFabric Fabric
        {
            get => _fabric;
            set => _fabric = value;
        }
        protected float Hp 
        { 
            get => _hp;
            set
            {
                _hp = value;
                if (_hp <= 0)
                    Die();
            }  
        }

        [SerializeField] private int mobID;
        [SerializeField] private float rotationSpeed;
        [SerializeField] private float maxDeltaRotate;
        [SerializeField] private float roamingRadius;
        [SerializeField] private float sniffingRadius;
        [SerializeField] private float maxMobHealth;
        [SerializeField] private Transform _player;
        [SerializeField] private float _hp;
        [SerializeField] private BasicItemConfiguration[] LootTable;
        [SerializeField] private int[] DropQuantity;
        [SerializeField] private int[] DropChance;

        private MobFabric _fabric;
        private NavMeshAgent _agent;
        private RaycastHit _slopeHit;
        private Rigidbody _mobRigidbody;
        private Vector3 _spawnPosition;
        private float _currentMobHealth;
        private float _fearHealthThreshold;
        private float _deltaRotate;
        private bool _isEntitySensed;
        private bool _isIgnoringSensor;
        private bool _isGrounded;
        /// <summary>
        /// Initialises basic parameters. Can't use constructor because objects with this class are initialized by
        /// instantiate method during the game
        /// </summary>
        public abstract void Initialise(MobFabric fabric, Transform player);

        /// <summary>
        /// Sets the spawn position, turns on the object and sets default values. Basically replaces the Start() method
        /// </summary>
        public abstract void SpawnSelf(Vector3 position);
        /// <summary>
        /// Kills a Mob 💀
        /// </summary>
        protected virtual void Die()
        {
            MonoBehaviour.Destroy(gameObject);
            //Goes through list, and drops everyting in apropriate amounts
            for (int i = 0; i < LootTable.Length; i++)
            {
                int proc = Random.Range(0, 101);
                if (proc <= DropChance[i])
                {
                    Instantiate(LootTable[i].ItemInWorldPrefab, new Vector3(transform.position.x + Random.Range(-2, 2), transform.position.y + Random.Range(1, 2), transform.position.z + Random.Range(-2, 2)), new Quaternion(Random.Range(0, 160), Random.Range(0, 160), Random.Range(0, 160), 0)).
                        GetComponent<DroppedItemBehaviour>().DroppedItemsAmount = DropQuantity[i];
                }
            }
        }

        /// <summary>
        /// Rotates wolf to face the player
        /// </summary>
        /// <param name="spot">Spot to face</param>
        public void LookAtPosition(Vector3 spot)
        {
            Vector3 direction = spot - transform.position;
            direction.y = 0;
            Quaternion rotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation,
                Time.deltaTime * RotationSpeed);
        }
        
        private void Start()
        {
            _mobRigidbody = GetComponent<Rigidbody>();
        }
    }
}