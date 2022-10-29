using Creatures.Player.Inventory;
using System.Data;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace Creatures.Player.Behaviour
{
    [RequireComponent(typeof(SphereCollider))]
    [RequireComponent(typeof(PlayerBehaviour))]
    public class PlayerInventory : MonoBehaviour
    {
        private InventoryContainer inventory;

        public static float ItemPickingUpTime => 3f;

        private PlayerBehaviour _playerBehaviour;
        private int _lastSlotIndex = 0;

        public InventoryContainer Inventory
        {
            get
            {
                if (inventory == null) Init();
                return inventory;
            }
            private set => inventory = value;
        }

        public BasicItemConfiguration SelectedItem
        {
            get
            {
                if (SelectedInventorySlot != -1) return Inventory[SelectedInventorySlot]?.Item;
                else return null;
            }
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

        private void CheckNearestInteractableItem(GameObject test)
        {
            var itemBehaviour = test.GetComponent<DroppedItemBehaviour>();
            if (itemBehaviour == null || itemBehaviour.IsThrown) return;
            float oldDistance = NearestInteractableItemDistance;
            float currentDistance = Vector3.Distance(transform.position, transform.position);
            if (currentDistance < oldDistance || oldDistance == -1)
            {
                ResetNearestItem(test);
                Debug.Log($"Updated object to {test}");
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Item"))
            {
                CheckNearestInteractableItem(other.gameObject);
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.CompareTag("Item"))
            {
                CheckNearestInteractableItem(other.gameObject);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.CompareTag("Item") && NearestInteractableItem == other)
            {
                ResetNearestItem(null);
                Debug.Log("Removed item object from this");
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

        public void UnselectItem()
        {
            _lastSlotIndex = SelectedInventorySlot;
            SelectedInventorySlot = -1;
        }

        public void ReselectItem()
        {
            SelectedInventorySlot = _lastSlotIndex;
        }

        public void ResetNearestItem(GameObject item)
        {
            NearestInteractableItem = item;
            print($"New item: {item}");
        }
    }
}

