using Player.Inventory;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player.Behaviour
{
    public class PlayerInventoryController : MonoBehaviour
    {
        public static float ItemPickingUpTime => 3f;

        public InventoryContainer Inventory { get; private set; }

        public GameObject NearestInteractableItem { get; private set; }

        public float NearestInteractableItemDistance => NearestInteractableItem == null ? -1 : Vector3.Distance(transform.position, NearestInteractableItem.transform.position);

        public float ItemPickingProgress { get; private set; } = 0f;

        // Start is called before the first frame update
        void Start()
        {
            Inventory = new InventoryContainer();
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKey(KeyCode.F) && NearestInteractableItem != null)
            {
                ItemPickingProgress += Time.deltaTime;
                if (ItemPickingProgress > ItemPickingUpTime)
                {
                    PickItemUp();
                }
            }
            else ItemPickingProgress = 0f;
        }

        private void PickItemUp()
        {
            var drop = NearestInteractableItem.GetComponent<DroppedItemBehaviour>();
            if (Inventory.AddItem(drop.AssociatedItem, 
                drop.DroppedItemsAmount, out int rem))
            {
                if (rem == 0)
                {
                    drop.OnPickupHandler();
                }
                NearestInteractableItem = null;
            }
            NearestInteractableItem = null;
            ItemPickingProgress = 0f;
        }

        public void ResetNearestItem(GameObject item)
        {
            NearestInteractableItem = item;
        }
    }
}

