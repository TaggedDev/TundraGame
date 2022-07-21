using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Player.Inventory
{
    [CreateAssetMenu(fileName = "New ItemConfiguration", menuName = "Items")]
    public class ItemConfiguration : ScriptableObject
    {
        [SerializeField]
        private Sprite icon;
        [SerializeField]
        private string title;
        [SerializeField]
        private string description;
        [SerializeField]
        private GameObject itemInWorldPrefab;

        public Sprite Icon { get => icon; private set => icon=value; }

        public string Title { get => title; private set => title=value; }

        public string Description { get => description; private set => description=value; }

        public GameObject ItemInWorldPrefab { get => itemInWorldPrefab; private set => itemInWorldPrefab=value; }

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
