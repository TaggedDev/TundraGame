using UnityEngine;

namespace Creatures.Mobs.Behaviour
{
    public class MobMovement : MonoBehaviour
    {
        [SerializeField] private Transform player;
        [SerializeField] private float maxDeltaRotate;
        [SerializeField] private float speed;
        [SerializeField] private int entityLayerMaskIndex;
        
        private float _deltaRotate;
        private Vector3 _mobSize;
        private Renderer _renderer;
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
            // Check if a tree in front of us
            // Rotate at Y axis by 3 degrees and move forward for 3 seconds

            // We move -> check if entity is in front of us
            // if there is an entity -> we rotate away
            // if there is no entity -> we move for few more seconds and face towards player

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
        /// <returns></returns>
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