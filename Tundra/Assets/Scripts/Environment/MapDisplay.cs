using UnityEngine;

namespace Environment
{
    public class MapDisplay : MonoBehaviour
    {
        [SerializeField] private Renderer textureRender;

        public void DrawTexture(Texture2D texture)
        {
            textureRender.sharedMaterial.mainTexture = texture;
            textureRender.transform.localScale = new Vector3(texture.width, 1, texture.height);
        }
    }
}
