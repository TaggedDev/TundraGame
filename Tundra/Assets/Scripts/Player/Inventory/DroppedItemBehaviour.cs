﻿using Player.Behaviour;
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
        [SerializeField]
        private bool isThrown;

        private Rigidbody _rigidbody;

        public BasicItemConfiguration AssociatedItem { get => _associatedItem; private set => _associatedItem=value; }

        public int DroppedItemsAmount { get => droppedItemsAmount; private set => droppedItemsAmount=value; }

        public bool IsThrown { get => isThrown; set => isThrown = value; }

        // Start is called before the first frame update
        void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        // Update is called once per frame
        void Update()
        {
            if (_rigidbody.velocity.sqrMagnitude <= .1f)
            {
                isThrown = false;
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            
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
