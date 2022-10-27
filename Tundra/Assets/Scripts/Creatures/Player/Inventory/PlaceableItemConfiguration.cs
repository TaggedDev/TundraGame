using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Environment;

namespace Creatures.Player.Inventory
{
    [CreateAssetMenu(fileName = "New Placeable Configuration", menuName = "Items/Placable Configuration")]
    public class PlaceableItemConfiguration : BasicItemConfiguration
    {
        [SerializeField] private PlaceableObject _representedObject;

        public PlaceableObject RepresentedObject { get => _representedObject; protected set => _representedObject = value; }

        public override GameObject Drop(Vector3 originPosition, Vector3 throwForce)
        {
            var obj = Instantiate(ItemInWorldPrefab, new Vector3(originPosition.x, originPosition.y + .55f, originPosition.z + .55f), Quaternion.identity);
            obj.TryGetComponent(out Rigidbody rigidbody);
            if (rigidbody != null)
                rigidbody.AddForce(throwForce);
            return obj;
        }

    }
}
