using UnityEngine;

namespace Environment.Terrain
{
    public class MapDisplay : MonoBehaviour
    {
        public Renderer textureRender;
        public MeshFilter meshFilter;
        public MeshRenderer meshRenderer;

        private void Start()
        {
            meshFilter.gameObject.SetActive(false);
        }

        public void DrawTexture(Texture2D texture) {
            textureRender.sharedMaterial.mainTexture = texture;
            textureRender.transform.localScale = new Vector3 (texture.width, 1, texture.height);
        }

        public void DrawMesh(MeshData meshData, Texture2D texture) {
            meshFilter.sharedMesh = meshData.CreateMesh ();
            meshRenderer.sharedMaterial.mainTexture = texture;
        }
    }
}
