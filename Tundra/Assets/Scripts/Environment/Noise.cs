using UnityEngine;
using Random = System.Random;
using Vector2 = UnityEngine.Vector2;

namespace Environment
{
    public static class Noise 
    {
        /// <summary>
        /// Generates noise map on various parameters
        /// </summary>
        /// <param name="mapWidth">X of the map</param>
        /// <param name="mapHeight">Y of the map</param>
        /// <param name="seed">Random generation parameter</param>
        /// <param name="scale">Zoom parameter</param>
        /// <param name="octaves">TBD: Layers?</param>
        /// <param name="persistance">TBD: ?</param>
        /// <param name="lacunarity">TBD: ?</param>
        /// <param name="offset">The offset on x and y axis </param>
        /// <returns>The 2d array of perlin noise values</returns>
        public static float[,] GenerateNoiseMap(int mapWidth, int mapHeight, int seed, float scale, int octaves, 
            float persistance, float lacunarity, Vector2 offset)
        {
            Vector2[] octaveOffset = GenerateOctaves(seed, octaves, offset);
            
            if (scale <= 0)
                scale = 0.001f;

            float[,] noiseMap = new float[mapWidth, mapHeight];
            float maxNoiseHeight = float.MinValue;
            float minNoiseHeight = float.MaxValue;

            float halfWidth = mapWidth / 2f;
            float halfHeight = mapHeight / 2f;

            for (int y = 0; y < mapHeight; y++)
            {
                for (int x = 0; x < mapWidth; x++)
                {
                    float amplitude = 1;
                    float frequency = 1;
                    float noiseHeight = 0;
                    
                    for (int i = 0; i < octaves; i++)
                    {
                        float sampleX = (x - halfWidth) / (scale * frequency) + octaveOffset[i].x;
                        float sampleY = (y - halfHeight) / (scale * frequency) + octaveOffset[i].y; 

                        float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;
                        noiseHeight += perlinValue * amplitude;
                        
                        amplitude *= persistance;
                        frequency *= lacunarity;
                    }

                    if (noiseHeight > maxNoiseHeight)
                        maxNoiseHeight = noiseHeight;
                    else if (noiseHeight < minNoiseHeight)
                        minNoiseHeight = noiseHeight;

                    noiseMap[x, y] = noiseHeight; 
                }
            }
            
            for (int y = 0; y < mapHeight; y++)
            for (int x = 0; x < mapWidth; x++)
                noiseMap[x, y] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, noiseMap[x, y]);
            
            return noiseMap;
        }

        /// <summary>
        /// Generates octaves based on seed and offset
        /// </summary>
        /// <param name="seed">Seed parameter</param>
        /// <param name="octaves">Amount of octaves</param>
        /// <param name="offset">The V2 offset for the noise map</param>
        /// <returns>Array of octaves represented as V2</returns>
        private static Vector2[] GenerateOctaves(int seed, int octaves, Vector2 offset)
        {
            var octaveOffset = new Vector2[octaves];
            Random random = new Random(seed);
            for (int i = 0; i < octaves; i++)
            {
                float offsetX = random.Next(-100000, 100000) + offset.x;
                float offsetY = random.Next(-100000, 100000) + offset.y;
                octaveOffset[i] = new Vector2(offsetX, offsetY);
            }

            return octaveOffset;
        }
    }
}
