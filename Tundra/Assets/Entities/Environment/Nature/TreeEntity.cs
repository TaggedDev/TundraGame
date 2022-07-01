using UnityEngine;

namespace ScriptableObjects.Environment.Nature
{
    public class TreeEntity : GeneratedObject
    {
        [SerializeField] private float health;
        [SerializeField] private GameObject model;

        public TreeEntity(Transform parent)
        {
            transform.parent = parent;
        }
    }
}