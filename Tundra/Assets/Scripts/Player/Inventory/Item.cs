using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Player.Inventory
{
    public class ItemConfiguration : ScriptableObject
    {
		public Sprite Icon { get; private set; }

        public string Title { get; private set; }

        public string Description { get; private set; }

        public GameObject ItemInWorldPrefab { get; private set; }

        public GameObject ThrowAway(Vector3 originPosition, Vector3 throwForce)
        {
            var obj = Instantiate(ItemInWorldPrefab, originPosition, Quaternion.identity);
            obj.TryGetComponent(out Rigidbody rigidbody);
            if (rigidbody != null)
                rigidbody.AddForce(throwForce);
            return obj;
        }

        public List<GameObject> MassThrowAway(int amount, Vector3 originPosition, Vector3 throwForce)
        {
            List<GameObject> result = new List<GameObject>();
            for (int i = 0; i < amount; i++)
            {
                result.Add(ThrowAway(originPosition, throwForce));
            }
            return result;
        }
    }
}
