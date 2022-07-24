using UnityEngine;

namespace Creatures.Mobs
{
    [RequireComponent(typeof(BoxCollider))]
    public class MobEntitySensor : MonoBehaviour
    {
        private const int ENTITY_LAYER_INDEX = 10;
        private const int PLAYER_LAYER_INDEX = 9;
        private Mob _owner;

        private void Start()
        {
            _owner = GetComponentInParent<Mob>();
        }

        private void OnTriggerEnter(Collider other)
        {
            switch (other.gameObject.layer)
            {
                case PLAYER_LAYER_INDEX:
                    _owner.IgnoreSensor = true;
                    break;
                case ENTITY_LAYER_INDEX:
                    _owner.IsEntitySensed = true;
                    break;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            switch (other.gameObject.layer)
            {
                case PLAYER_LAYER_INDEX:
                    _owner.IgnoreSensor = false;
                    break;
                case ENTITY_LAYER_INDEX:
                    _owner.IsEntitySensed = false;
                    break;
            }
        }
    }
}