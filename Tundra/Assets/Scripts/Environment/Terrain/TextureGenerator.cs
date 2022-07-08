using UnityEngine;

namespace Environment.Terrain
{
    /// <summary>
    /// This class is used for generating textures by height maps, which is generated procedurally.
    /// </summary>
    public static class TextureGenerator {
        
        /// <summary>
        /// Generates 2DTexture from color map by passed values.
        /// </summary>
        /// <param name="colourMap">The color map to apply</param>
        /// <param name="width">The width of the texture</param>
        /// <param name="height">The height of the texture</param>
        /// <returns>Generated 2D texture</returns>
        public static Texture2D TextureFromColourMap(Color[] colourMap, int width, int height) {
            Texture2D texture = new Texture2D (width, height)
            {
                filterMode = FilterMode.Point,
                wrapMode = TextureWrapMode.Clamp
            };
            texture.SetPixels (colourMap);
            texture.Apply ();
            return texture;
        }

        /// <summary>
        /// Generates 2DTexture by height map only.
        /// </summary>
        /// <param name="heightMap">The height map to generate texture from</param>
        /// <returns>Generated 2D texture</returns>
        public static Texture2D TextureFromHeightMap(float[,] heightMap) {
            int width = heightMap.GetLength (0);
            int height = heightMap.GetLength (1);

            Color[] colourMap = new Color[width * height];
            for (int y = 0; y < height; y++) {
                for (int x = 0; x < width; x++) {
                    colourMap [y * width + x] = Color.Lerp (Color.black, Color.white, heightMap [x, y]);
                }
            }

            return TextureFromColourMap (colourMap, width, height);
        }

    }
}