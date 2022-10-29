using Creatures.Player.Behaviour;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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

        public void OnPickupHandler()
        {
            Destroy(gameObject);
        }
    }
}
