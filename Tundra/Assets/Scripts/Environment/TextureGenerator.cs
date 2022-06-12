using UnityEngine;

namespace Environment
{
    public static class TextureGenerator
    {
        /// <summary>
        /// Colored map
        /// </summary>
        /// <param name="colorMap">Color array to apply on texture</param>
        /// <param name="width">Width of the array (and texture)</param>
        /// <param name="height">Height of the array (and texture)</param>
        /// <returns></returns>
        public static Texture2D TextureFromColorMap(Color[] colorMap, int width, int height)
        {
            Texture2D texture = new Texture2D(width, height)
            {
                filterMode = FilterMode.Point,
                wrapMode = TextureWrapMode.Clamp
            };
            texture.SetPixels(colorMap);
            texture.Apply();
            return texture;
        }

        /// <summary>
        /// Returns black & white color map
        /// </summary>
        /// <param name="noiseMap">The array of perlin noise values</param>
        /// <returns>A texture to apply on plane object</returns>
        public static Texture2D TextureFromHeightMap(float[,] noiseMap)
        {
            int width = noiseMap.GetLength(0);
            int height = noiseMap.GetLength(1);

            Color[] colorMap = new Color[width * height];
            
            for (int y = 0; y < height; y++)
            for (int x = 0; x < width; x++)
                colorMap[y * width + x] = Color.Lerp(Color.black, Color.white, noiseMap[x, y]);
            
            return TextureFromColorMap(colorMap, width, height);
        } 
    }
}