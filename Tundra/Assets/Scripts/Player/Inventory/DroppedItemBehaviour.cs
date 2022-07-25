using Player.Behaviour;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Player.Inventory
{
    public class DroppedItemBehaviour : MonoBehaviour
    {
        [SerializeField]
        private BasicItemConfiguration _associatedItem;
        [SerializeField]
        private int droppedItemsAmount;

        


        public BasicItemConfiguration AssociatedItem { get => _associatedItem; private set => _associatedItem=value; }

        public int DroppedItemsAmount { get => droppedItemsAmount; private set => droppedItemsAmount=value; }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                CheckPlayerNearestItem(other.gameObject);
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                CheckPlayerNearestItem(other.gameObject);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.CompareTag("Player") && other.gameObject.GetComponent<PlayerInventoryController>().NearestInteractableItem == gameObject)
            {
                other.gameObject.GetComponent<PlayerInventoryController>().ResetNearestItem(null);
                Debug.Log("Removed item object from this");
            }
        }

        void CheckPlayerNearestItem(GameObject player)
        {
            float oldDistance = player.GetComponent<PlayerInventoryController>().NearestInteractableItemDistance;
            float currentDistance = Vector3.Distance(player.transform.position, transform.position);
            if (currentDistance < oldDistance || oldDistance == -1)
            {
                player.GetComponent<PlayerInventoryController>().ResetNearestItem(gameObject);
                Debug.Log("Updated object to this");
            }
        }

        public void OnPickupHandler()
        {
            Destroy(gameObject);
        }
    }
}
