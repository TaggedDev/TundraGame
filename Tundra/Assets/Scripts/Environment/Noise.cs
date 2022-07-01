using System;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

namespace Environment
{
	public enum NormalizeMode
	{
		Local,
		Global
	}
	
	public static class Noise
	{
		public static MapGenerator Generator;

		public static float[,] GenerateNoiseMap(int mapWidth, int mapHeight, int seed, float scale, int octaves,
			float persistance, float lacunarity, Vector2 offset, NormalizeMode normalizeMode)
		{
			float[,] noiseMap = new float[mapWidth, mapHeight];

			System.Random prng = new System.Random(seed);
			Vector2[] octaveOffsets = new Vector2[octaves];

			float maxPossibleHeight = 0;
			float amplitude = 1;

			for (int i = 0; i < octaves; i++)
			{
				float offsetX = prng.Next(-100000, 100000) + offset.x;
				float offsetY = prng.Next(-100000, 100000) - offset.y;
				octaveOffsets[i] = new Vector2(offsetX, offsetY);

				maxPossibleHeight += amplitude;
				amplitude *= persistance;
			}

			if (scale <= 0)
			{
				scale = 0.0001f;
			}

			float maxLocalNoiseHeight = float.MinValue;
			float minLocalNoiseHeight = float.MaxValue;

			float halfWidth = mapWidth / 2f;
			float halfHeight = mapHeight / 2f;


			for (int y = 0; y < mapHeight; y++)
			{
				for (int x = 0; x < mapWidth; x++)
				{

					amplitude = 1;
					float frequency = 1;
					float noiseHeight = 0;

					for (int i = 0; i < octaves; i++)
					{
						float sampleX = (x - halfWidth + octaveOffsets[i].x) / scale * frequency;
						float sampleY = (y - halfHeight + octaveOffsets[i].y) / scale * frequency;

						float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;
						noiseHeight += perlinValue * amplitude;

						amplitude *= persistance;
						frequency *= lacunarity;
					}

					if (noiseHeight > maxLocalNoiseHeight)
					{
						maxLocalNoiseHeight = noiseHeight;
					}
					else if (noiseHeight < minLocalNoiseHeight)
					{
						minLocalNoiseHeight = noiseHeight;
					}

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
						float normalizedHeight = (noiseMap[x, y] + 1) / (maxPossibleHeight / 0.9f);
						noiseMap[x, y] = Mathf.Clamp(normalizedHeight, 0, int.MaxValue);
					}
				}
			}

			return noiseMap;
		}

		public static float[,] GenerateNoiseMap(int mapWidth, int mapHeight, Vector2 playerPos)
		{
			if (Generator == null)
			{
				throw new Exception("Generator was null or empty");
			}
			
			int seed = Generator.Seed; 
			int octaves = Generator.Octaves;
			float scale = Generator.NoiseScale;
			float persistance = Generator.Persistance;
			float lacunarity = Generator.Lacunarity;
			NormalizeMode normalizeMode = Generator.normalizeMode;
			
			float[,] noiseMap = new float[mapWidth, mapHeight];

			System.Random prng = new System.Random(seed);
			Vector2[] octaveOffsets = new Vector2[octaves];

			float maxPossibleHeight = 0;
			float amplitude = 1;

			for (int i = 0; i < octaves; i++)
			{
				float offsetX = prng.Next(-100000, 100000) + playerPos.x ;
				float offsetY = prng.Next(-100000, 100000) - playerPos.y;
				octaveOffsets[i] = new Vector2(offsetX, offsetY);

				maxPossibleHeight += amplitude;
				amplitude *= persistance;
			}

			if (scale <= 0)
			{
				scale = 0.0001f;
			}

			float maxLocalNoiseHeight = float.MinValue;
			float minLocalNoiseHeight = float.MaxValue;

			float halfWidth = mapWidth / 2f;
			float halfHeight = mapHeight / 2f;


			for (int y = 0; y < mapHeight; y++)
			{
				for (int x = 0; x < mapWidth; x++)
				{

					amplitude = 1;
					float frequency = 1;
					float noiseHeight = 0;

					for (int i = 0; i < octaves; i++)
					{
						float sampleX = (x - halfWidth + octaveOffsets[i].x) / scale * frequency;
						float sampleY = (y - halfHeight + octaveOffsets[i].y) / scale * frequency;

						float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;
						noiseHeight += perlinValue * amplitude;

						amplitude *= persistance;
						frequency *= lacunarity;
					}

					if (noiseHeight > maxLocalNoiseHeight)
					{
						maxLocalNoiseHeight = noiseHeight;
					}
					else if (noiseHeight < minLocalNoiseHeight)
					{
						minLocalNoiseHeight = noiseHeight;
					}

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
						float normalizedHeight = (noiseMap[x, y] + 1) / (maxPossibleHeight / 0.9f);
						noiseMap[x, y] = Mathf.Clamp(normalizedHeight, 0, int.MaxValue);
					}
				}
			}

			return noiseMap;
		}
		
		public static float GetNoiseValue(float xCoord, float yCoord, int radius)
		{
			int mapWidth = Generator.MapChunkSize; 
			int mapHeight = Generator.MapChunkSize;
			int seed = Generator.Seed; 
			int octaves = Generator.Octaves;
			float scale = Generator.NoiseScale;
			float persistance = Generator.Persistance;
			float lacunarity = Generator.Lacunarity; 
			Vector2 offset = Generator.Offset; 
			NormalizeMode normalizeMode = Generator.normalizeMode;
			
			float[,] noiseMap = new float[mapWidth, mapHeight];

			System.Random prng = new System.Random(seed);
			Vector2[] octaveOffsets = new Vector2[octaves];

			float maxPossibleHeight = 0;
			float amplitude = 1;

			for (int i = 0; i < octaves; i++)
			{
				float offsetX = prng.Next(-100000, 100000) + offset.x;
				float offsetY = prng.Next(-100000, 100000) - offset.y;
				octaveOffsets[i] = new Vector2(offsetX, offsetY);

				maxPossibleHeight += amplitude;
				amplitude *= persistance;
			}

			if (scale <= 0)
			{
				scale = 0.0001f;
			}

			float maxLocalNoiseHeight = float.MinValue;
			float minLocalNoiseHeight = float.MaxValue;

			float halfWidth = mapWidth / 2f;
			float halfHeight = mapHeight / 2f;

			int xIndex = Mathf.FloorToInt(xCoord);
			int yIndex = Mathf.FloorToInt(yCoord);

			for (int y = 0; y < mapHeight; y++)
			{
				for (int x = 0; x < mapWidth; x++)
				{
					if (x != xIndex || y != yIndex) continue;
					
					amplitude = 1;
					float frequency = 1;
					float noiseHeight = 0;

					for (int i = 0; i < octaves; i++)
					{
						float sampleX = (x - halfWidth + octaveOffsets[i].x) / scale * frequency;
						float sampleY = (y - halfHeight + octaveOffsets[i].y) / scale * frequency;

						float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;
						noiseHeight += perlinValue * amplitude;

						amplitude *= persistance;
						frequency *= lacunarity;
					}

					if (noiseHeight > maxLocalNoiseHeight)
					{
						maxLocalNoiseHeight = noiseHeight;
					}
					else if (noiseHeight < minLocalNoiseHeight)
					{
						minLocalNoiseHeight = noiseHeight;
					}

					noiseMap[x, y] = noiseHeight;
				}
			}

			if (normalizeMode == NormalizeMode.Local)
				return Mathf.InverseLerp(minLocalNoiseHeight, maxLocalNoiseHeight,
					noiseMap[xIndex + radius, yIndex + radius]);
			// if normalize mode is global
			float normalizedHeight = (noiseMap[xIndex + radius, yIndex + radius] + 1) / (maxPossibleHeight / 0.9f);
			return Mathf.Clamp(normalizedHeight, 0, int.MaxValue);
		}
	}
}
