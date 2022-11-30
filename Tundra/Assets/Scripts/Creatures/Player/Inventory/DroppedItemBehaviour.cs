using Creatures.Player.Behaviour;
using UnityEngine;
using Creatures.Player.Inventory.ItemConfiguration;

namespace Creatures.Player.Inventory
{
    public class DroppedItemBehaviour : MonoBehaviour
    {
        // The MeshFilter and MeshRenderer of this model
        [SerializeField] private MeshFilter model;
        [SerializeField] private MeshRenderer materials;
        [SerializeField] private Vector3 handedRotation;
        [SerializeField] private Vector3 handedScale;
        [SerializeField] private BasicItemConfiguration _associatedItem;
        [SerializeField] private int droppedItemsAmount;
        [SerializeField] private bool isThrown;
        private Rigidbody _rigidbody;
        
        public MeshFilter Model { get => model; set => model = value; }
        public MeshRenderer Materials { get => materials; set => materials = value; }
        public Vector3 HandedRotation { get => handedRotation; set => handedRotation = value; }
        public Vector3 HandedScale { get => handedScale; set => handedScale = value; }
        public BasicItemConfiguration AssociatedItem { get => _associatedItem; private set => _associatedItem=value; }
        
        /// <summary>
        /// Amount of items stacked on the floor
        /// </summary>
        public int DroppedItemsAmount { get => droppedItemsAmount; set => droppedItemsAmount=value; }

        public bool IsThrown { get => isThrown; set => isThrown = value; }

        private void Start()
        {
            if (handedScale == Vector3.zero)
                handedScale = Vector3.one;
            _rigidbody = GetComponent<Rigidbody>();
        }
        
        private void Update()
        {
            if (_rigidbody.velocity.sqrMagnitude <= .1f)
            {
                isThrown = false;
            }
        }

        private void OnValidate()
        {
            if (handedScale == Vector3.zero) 
            {
                handedScale = Vector3.one / 4;
            }
        }

        /// <summary>
        /// Places the object in player's hand
        /// </summary>
        /// <param name="holderAnchor">The object where to place the object </param>
        public void OnPickupHandler(ItemHolder holderAnchor)
        {
            holderAnchor.SetNewMesh(handedScale,Quaternion.Euler(handedRotation), model, materials);
            Destroy(gameObject);
        }
    }
}
