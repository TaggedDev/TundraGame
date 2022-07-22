using UnityEngine;

namespace Environment.Terrain
{
    /// <summary>
    /// Makes noise map be visible in Editor Mode.
    /// </summary>
    public class MapDisplay : MonoBehaviour
    {
        public Renderer textureRender;
        public MeshFilter meshFilter;
        public MeshRenderer meshRenderer;

        /// <summary>
        /// Disables the test plane on game starts.
        /// </summary>
        private void Start()
        {
            meshFilter.gameObject.SetActive(false);
        }

        /// <summary>
        /// Draws the texture by the passed parameter
        /// </summary>
        public void DrawTexture(Texture2D texture) {
            textureRender.sharedMaterial.mainTexture = texture;
            textureRender.transform.localScale = new Vector3 (texture.width, 1, texture.height);
        }

        /// <summary>
        /// Draws the mesh based on parameters. 
        /// </summary>
        /// <param name="meshData">MeshData with values to build Mesh on test plane</param>
        /// <param name="texture">Texture to apply to test plane after Mesh is generated</param>
        public void DrawMesh(MeshData meshData, Texture2D texture) {
            meshFilter.sharedMesh = meshData.CreateMesh ();
            meshRenderer.sharedMaterial.mainTexture = texture;
        }
    }
}
