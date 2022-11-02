using UnityEngine;

namespace Creatures.Player.Inventory
{
    public class ItemHolder : MonoBehaviour
    {
        private MeshFilter _meshFilter;
        private MeshRenderer _meshRenderer;
        
        private void Start()
        {
            _meshFilter = GetComponent<MeshFilter>();
            _meshRenderer = GetComponent<MeshRenderer>();
        }

        /// <summary>
        /// Set the mesh of itemHolder to current mesh
        /// </summary>
        /// <param name="handedScale">Scale of item when handed</param>
        /// <param name="model">MeshFilter of this item</param>
        /// <param name="materials">MeshRenderer of this item</param>
        public void SetNewMesh(Vector3 handedScale, Quaternion handedRotation, MeshFilter model, MeshRenderer materials)
        {
            _meshFilter.mesh = model.sharedMesh;
            _meshRenderer.materials = materials.sharedMaterials;
            transform.localRotation = handedRotation;
            transform.localScale = handedScale;
        }

        /// <summary>
        /// Resets mesh and scale values to default
        /// </summary>
        public void ResetMesh()
        {
            _meshFilter.mesh = null;
            _meshRenderer.materials = null;
            transform.localScale = Vector3.one;
            transform.localRotation = Quaternion.identity;
        }
    }
}