using Environment.Terrain;
using UnityEngine;

namespace Environment.Objects
{
	public class ObjectMapGenerator : MonoBehaviour
	{
		public enum DrawMode
		{
			NoiseMap,
			ColourMap,
			Mesh
		}
		
		// Properties
		public bool AutoUpdate => autoUpdate;
		public float Persistance => persistance;
		public float Lacunarity => lacunarity;
		public int MapChunkSize => mapChunkSize;
		public float NoiseScale => noiseScale;
		public int Octaves => octaves;
		public int Seed => seed;
		public Vector2 Offset => offset;
		
		// Constants
		private const int mapChunkSize = 47;
		
		// Fields
		[SerializeField] [Range(0, 1)] private float persistance;
		[SerializeField] private float noiseScale;
		[SerializeField] private int octaves;
		[SerializeField] private float lacunarity;
		[SerializeField] private int seed;
		[SerializeField] private Vector2 offset;

		[SerializeField] private bool autoUpdate;
		[SerializeField] private TerrainType[] regions;

		public DrawMode drawMode;
		public NormalizeMode normalizeMode;
		// Public methods

		public void DrawMapInEditor()
		{
			MapData mapData = GenerateMapData(Vector2.zero);

			MapDisplay display = FindObjectOfType<MapDisplay>();
			if (drawMode == DrawMode.NoiseMap)
			{
				display.DrawTexture(TextureGenerator.TextureFromHeightMap(mapData.heightMap));
			}
			else if (drawMode == DrawMode.ColourMap)
			{
				display.DrawTexture(
					TextureGenerator.TextureFromColourMap(mapData.colourMap, mapChunkSize, mapChunkSize));
			}
		}
		
		private MapData GenerateMapData(Vector2 centre)
		{
			float[,] noiseMap = Noise.GenerateNoiseMap(mapChunkSize + 2, mapChunkSize + 2, seed, noiseScale, octaves,
				persistance, lacunarity, centre + offset, normalizeMode);

			Color[] colourMap = new Color[mapChunkSize * mapChunkSize];
			for (int y = 0; y < mapChunkSize; y++)
			{
				for (int x = 0; x < mapChunkSize; x++)
				{
					float currentHeight = noiseMap[x, y];
					for (int i = 0; i < regions.Length; i++)
					{
						if (currentHeight >= regions[i].height)
						{
							colourMap[y * mapChunkSize + x] = regions[i].colour;
						}
						else
						{
							break;
						}
					}
				}
			}
			return new MapData(noiseMap, colourMap);
		}
		
		public float[,] GenerateDensityMap(Vector2 centre)
		{
			float[,] noiseMap = Noise.GenerateNoiseMap(mapChunkSize + 2, mapChunkSize + 2, seed, noiseScale, octaves,
				persistance, lacunarity, centre + offset, normalizeMode);

			return noiseMap;
		}

		private void OnValidate()
		{
			if (lacunarity < 1)
				lacunarity = 1;
			
			if (octaves < 0)
				octaves = 0;
		}
	}
}