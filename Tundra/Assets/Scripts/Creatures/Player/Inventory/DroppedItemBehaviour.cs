using Creatures.Player.Behaviour;
using UnityEngine;
using Creatures.Player.Inventory.ItemConfiguration;

namespace Creatures.Player.Inventory
{
    public class DroppedItemBehaviour : MonoBehaviour
    {
        [SerializeField]
        private BasicItemConfiguration _associatedItem;
        [SerializeField]
        private int droppedItemsAmount;
        [SerializeField]
        private bool isThrown;

        private Rigidbody _rigidbody;

        public BasicItemConfiguration AssociatedItem { get => _associatedItem; private set => _associatedItem=value; }

        public int DroppedItemsAmount { get => droppedItemsAmount; private set => droppedItemsAmount=value; }

        public bool IsThrown { get => isThrown; set => isThrown = value; }

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }
        
        private void Update()
        {
            if (_rigidbody.velocity.sqrMagnitude <= .1f)
            {
                isThrown = false;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!isThrown && other.gameObject.CompareTag("Player"))
            {
                CheckPlayerNearestItem(other.gameObject);
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (!isThrown && other.gameObject.CompareTag("Player"))
            {
                CheckPlayerNearestItem(other.gameObject);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.CompareTag("Player") && other.gameObject.GetComponent<PlayerInventory>().NearestInteractableItem == gameObject)
            {
                other.gameObject.GetComponent<PlayerInventory>().ResetNearestItem(null);
                Debug.Log("Removed item object from this");
            }
        }

        void CheckPlayerNearestItem(GameObject player)
        {
            float oldDistance = player.GetComponent<PlayerInventory>().NearestInteractableItemDistance;
            float currentDistance = Vector3.Distance(player.transform.position, transform.position);
            if (currentDistance < oldDistance || oldDistance == -1)
            {
                player.GetComponent<PlayerInventory>().ResetNearestItem(gameObject);
                Debug.Log("Updated object to this");
            }
        }

        public void OnPickupHandler()
        {
            //Destroy(gameObject);
        }
    }
}
