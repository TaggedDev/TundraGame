using Creatures.Player.Behaviour;
using Creatures.Player.Inventory;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Creatures.Player.Behaviour
{
    public class PlayerInventory : MonoBehaviour
    {
        private InventoryContainer inventory;

        public static float ItemPickingUpTime => 3f;

        private PlayerBehaviour _playerBehaviour;

        public InventoryContainer Inventory 
        { 
            get
            {
                if (inventory == null) Init();
                return inventory;
            }
            private set => inventory = value;
        }

        public GameObject NearestInteractableItem { get; private set; }

        public float NearestInteractableItemDistance => NearestInteractableItem == null ? -1 : Vector3.Distance(transform.position, NearestInteractableItem.transform.position);

        public float ItemPickingProgress { get; private set; } = 0f;

        public int SelectedInventorySlot { get; set; }

        // Start is called before the first frame update
        void Start()
        {
            if (inventory == null) Init();
        }

        private void Init()
        {
            Inventory = new InventoryContainer();
            _playerBehaviour = GetComponent<PlayerBehaviour>();
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKey(KeyCode.E) && !Input.GetKey(KeyCode.LeftControl) && NearestInteractableItem != null)
            {
                ItemPickingProgress += Time.deltaTime;
                if (ItemPickingProgress > ItemPickingUpTime)
                {
                    PickItemUp();
                }
            }
            else ItemPickingProgress = 0f;
            if (Input.GetKeyDown(KeyCode.Q) && !Input.GetKey(KeyCode.LeftControl))
            {
                ThrowItemAway();
            }
        }

        private void ThrowItemAway()
        {
            Inventory.Slots[SelectedInventorySlot].DropItem(transform.position, transform.forward * 3 + Vector3.up);
        }

        private void PickItemUp()
        {
            if (_playerBehaviour.OverweightCoefficient < 2)
            {
                var drop = NearestInteractableItem.GetComponent<DroppedItemBehaviour>();
                if (Inventory.AddItem(drop.AssociatedItem,
                    drop.DroppedItemsAmount, out int rem))
                {
                    if (rem == 0)
                    {
                        drop.OnPickupHandler();
                    }
                }
            }
            NearestInteractableItem = null;
            ItemPickingProgress = 0f;
        }

        public void ResetNearestItem(GameObject item)
        {
            NearestInteractableItem = item;
            print($"New item: {item}");
        }
    }
}

