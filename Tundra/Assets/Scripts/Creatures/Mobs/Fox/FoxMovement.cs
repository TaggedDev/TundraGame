using UnityEngine;

namespace Creatures.Mobs.Fox
{
    public class FoxMovement : MonoBehaviour, IMob
    {
        [SerializeField] private Transform player;
        [SerializeField] private float maxDeltaRotate;
        [SerializeField] private float speed;
        [SerializeField] private int entityLayerMaskIndex;
        
        private const int MOB_LAYER = 11;
        private float _deltaRotate;
        private Vector3 _mobSize;
        private Renderer _renderer;
        
        public void Initialise(Transform playerParameter)
        {
            player = playerParameter;
            transform.gameObject.layer = MOB_LAYER;
        }
        
        private void Start()
        {
            _renderer = GetComponent<Renderer>();
            _mobSize = _renderer.bounds.size;
            _deltaRotate = maxDeltaRotate;
            FaceTowardsPlayer();
            _deltaRotate = maxDeltaRotate;
        }

        // Update is called once per frame
        private void Update()
        {
            transform.position += transform.forward * (speed * Time.deltaTime);
            
            if (IsEntitySensed())
            {
                RotateAwayFromSolidEntity();
                _deltaRotate = maxDeltaRotate;
            }
            else
            {
                if (_deltaRotate > 0)
                {
                    _deltaRotate -= Time.deltaTime;
                }
                else
                {
                    FaceTowardsPlayer();
                }
            }
        }

        /// <summary>
        /// Calls a boxcast in front of player to check if there is an active object with entityLayerMask.
        /// </summary>
        /// <returns>Bool value if there is an entity right in front of us</returns>
        private bool IsEntitySensed()
        {
            return Physics.BoxCast(transform.position, _mobSize / 2, transform.forward, Quaternion.identity, 5f,
                1 << entityLayerMaskIndex);
        }

        /// <summary>
        /// Rotates mob away from the entity.
        /// </summary>
        private void RotateAwayFromSolidEntity()
        {
            for (int i = 0; i <= 30; i++)
            {
                if (IsEntitySensed())
                    transform.eulerAngles += new Vector3(0, 3f, 0);
                else
                    return;
            }
        }

        /// <summary>
        /// Calculates the destination by subtracting two V3 (but doesn't affect Y) 
        /// </summary>
        private void FaceTowardsPlayer()
        {
            Vector3 playerPosition = player.transform.position;
            Vector3 mobPosition = transform.position;
            Vector3 direction = new Vector3(playerPosition.x - mobPosition.x, 0, playerPosition.z - mobPosition.z);
            transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
        }
    }
}