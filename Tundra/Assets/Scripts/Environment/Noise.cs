using UnityEngine;
using Random = System.Random;
using Vector2 = UnityEngine.Vector2;

namespace Environment
{
    public enum NormalizeMode
    {
        Local,
        Global
    };

    public static class Noise
    {
        public static float[,] GenerateNoiseMap(int mapWidth, int mapHeight, int seed, float scale, int octaves, 
            float persistance, float lacunarity, Vector2 offset, NormalizeMode normalizeMode)
        { 
            float maxPossibleHeight = 0;
            float amplitude = 1;

            Vector2[] octaveOffset = new Vector2[octaves];
            Random random = new Random(seed);


            for (int i = 0; i < octaves; i++)
            {
                float offsetX = random.Next(-100000, 100000) + offset.x;
                float offsetY = random.Next(-100000, 100000) - offset.y;
                octaveOffset[i] = new Vector2(offsetX, offsetY);

                maxPossibleHeight += amplitude;
                amplitude *= persistance;
            }

            if (scale <= 0)
                scale = 0.001f;

            float[,] noiseMap = new float[mapWidth, mapHeight];
            float maxLocalNoiseHeight = float.MinValue;
            float minLocalNoiseHeight = float.MaxValue;

            float halfWidth = mapWidth / 2f;
            float halfHeight = mapHeight / 2f;

            for (int y = 0; y < mapHeight; y++)
            {
                for (int x = 0; x < mapWidth; x++)
                {
                    float noiseHeight = 0;
                    float frequency = 1;
                    amplitude = 1;

                    for (int i = 0; i < octaves; i++)
                    {
                        float sampleX = (x - halfWidth + octaveOffset[i].x) / scale * frequency;
                        float sampleY = (y - halfHeight + octaveOffset[i].y) / scale * frequency; 

                        float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;
                        noiseHeight += perlinValue * amplitude;
                        
                        amplitude *= persistance;
                        frequency *= lacunarity;
                    }

                    if (noiseHeight > maxLocalNoiseHeight)
                        maxLocalNoiseHeight = noiseHeight;
                    else if (noiseHeight < minLocalNoiseHeight)
                        minLocalNoiseHeight = noiseHeight;

                    noiseMap[x, y] = noiseHeight; 
                }
            }

            for (int y = 0; y < mapHeight; y++)
            {
                for (int x = 0; x < mapWidth; x++)
                {
                    if (normalizeMode == NormalizeMode.Local)
                    {
                        noiseMap[x, y] = Mathf.InverseLerp(minLocalNoiseHeight, maxLocalNoiseHeight, noiseMap[x, y]);
                    }
                    else
                    {
                        float normalizedHeight = (noiseMap[x, y] + 1) / (2f * maxPossibleHeight / 0.9f);
                        noiseMap[x, y] = Mathf.Clamp(normalizedHeight, 0, int.MaxValue);
                    }
                }
            }
            return noiseMap;
        }
    }
}
