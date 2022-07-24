using UnityEngine;

namespace Creatures.Mobs
{
    [RequireComponent(typeof(BoxCollider))]
    public class MobEntitySensor : MonoBehaviour
    {
        private const int ENTITY_LAYER_INDEX = 10;
        private Mob _owner;

        private void Start()
        {
            _owner = GetComponentInParent<Mob>();
        }

        private void OnTriggerEnter(Collider other)
        {

            if (other.gameObject.layer == ENTITY_LAYER_INDEX)
            {
                Debug.Log($"Enter + {other.name}");
                _owner.IsEntitySensed = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.layer == ENTITY_LAYER_INDEX)
            {
                Debug.Log($"Exit + {other.name}");
                _owner.IsEntitySensed = false;
            }
                
        }
    }
}