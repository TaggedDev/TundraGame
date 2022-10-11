using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace Environment.Terrain
{
	/// <summary>
	/// Only once set class describes the generation of meshes, mapData and noise for the whole world.
	/// </summary>
	public class MapGenerator : MonoBehaviour
	{
		/// <summary>
		/// Overall information about the current Thread
		/// </summary>
		/// <typeparam name="T">Class that returns the callback</typeparam>
		private struct MapThreadInfo<T>
		{
			public readonly Action<T> callback;
			public readonly T parameter;

			public MapThreadInfo(Action<T> callback, T parameter)
			{
				this.callback = callback;
				this.parameter = parameter;
			}
		}
		
		// Properties
		public bool AutoUpdate => autoUpdate;
		public float MeshHeightMultiplier => meshHeightMultiplier;
		public AnimationCurve MeshHeightCurve => meshHeightCurve;
		public float Persistance => persistance;
		public float Lacunarity => lacunarity;
		public int MapChunkSize => mapChunkSize;
		public float HeightMultiplier => meshHeightMultiplier;
		public float NoiseScale => noiseScale;
		public int Octaves => octaves;
		public int Seed => seed;
		public Vector2 Offset => offset;
		

		// Constants
		public const int mapChunkSize = 11;
		
		// Fields
		[SerializeField] [Range(0, 1)] private float persistance;
		[SerializeField] private float noiseScale;
		[SerializeField] private int octaves;
		[SerializeField] private float lacunarity;
		[SerializeField] private int seed;
		[SerializeField] private Vector2 offset;
		[SerializeField] private float meshHeightMultiplier;
		[SerializeField] private AnimationCurve meshHeightCurve;

		[SerializeField] private bool autoUpdate;
		[SerializeField] private TerrainType[] regions;
		private readonly Queue<MapThreadInfo<MapData>> mapDataThreadInfoQueue = new Queue<MapThreadInfo<MapData>>();
		private readonly Queue<MapThreadInfo<MeshData>> meshDataThreadInfoQueue = new Queue<MapThreadInfo<MeshData>>();

		/*[HideInInspector]*/ public int mapDataCount;
		/*[HideInInspector]*/ public int meshDataCount;
		
		// Variables
		public NormalizeMode normalizeMode;
		
		/// <summary>
		/// Creates the thread to generate mapData
		/// </summary>
		/// <param name="centre">The centre of the chunk</param>
		/// <param name="callback">The function to call when thread will be processed</param>
		public void RequestMapData(Vector2 centre, Action<MapData> callback)
		{
			ThreadStart threadStart = delegate { ProcessMapDataThread(centre, callback); };

			new Thread(threadStart).Start();
		}
		
		/// <summary>
		/// Creates the thread to generate meshData
		/// </summary>
		/// <param name="mapData"></param>
		/// <param name="lod"></param>
		/// <param name="callback"></param>
		public void RequestMeshData(MapData mapData, int lod, Action<MeshData> callback)
		{
			ThreadStart threadStart = delegate { ProcessMeshDataThread(mapData, lod, callback); };

			new Thread(threadStart).Start();
		}
		
		// Private methods
		
		/// <summary>
		/// This function is called object initializing 
		/// </summary>
		private void Start()
		{
			mapDataCount = 0;
			meshDataCount = 0;
		}

		/// <summary>
		/// This function is called on every frame. Gets all threads from queues and process them with callback
		/// </summary>
		private void Update()
		{
			// Iterate through queued threads to calculate MapData
			if (mapDataThreadInfoQueue.Count > 0)
			{
				for (int i = 0; i < mapDataThreadInfoQueue.Count; i++)
				{
					MapThreadInfo<MapData> threadInfo = mapDataThreadInfoQueue.Dequeue();
					threadInfo.callback(threadInfo.parameter);
				}
				
			}

			// Iterate through queued threads to calculate MeshData
			if (meshDataThreadInfoQueue.Count > 0)
			{
				for (int i = 0; i < meshDataThreadInfoQueue.Count; i++)
				{
					MapThreadInfo<MeshData> threadInfo = meshDataThreadInfoQueue.Dequeue();
					threadInfo.callback(threadInfo.parameter);
				}
			}
		}

		/// <summary>
		/// Generates MapData and passes it in queue
		/// </summary>
		/// <param name="centre">The chunk centre</param>
		/// <param name="callback">The function to be passed in thread</param>
		private void ProcessMapDataThread(Vector2 centre, Action<MapData> callback)
		{
			MapData mapData = GenerateMapData(centre);
			lock (mapDataThreadInfoQueue)
			{
				mapDataThreadInfoQueue.Enqueue(new MapThreadInfo<MapData>(callback, mapData));
			}
		}

		/// <summary>
		/// Generates MeshData and passes it in queue
		/// </summary>
		/// <param name="mapData">MapData to generate mesh from</param>
		/// <param name="lod">Level of index for this mesh</param>
		/// <param name="callback">Callback function to be passed in thread</param>
		private void ProcessMeshDataThread(MapData mapData, int lod, Action<MeshData> callback)
		{
			MeshData meshData =
				MeshGenerator.GenerateTerrainMesh(mapData.heightMap, meshHeightMultiplier, meshHeightCurve);
			lock (meshDataThreadInfoQueue)
			{
				meshDataThreadInfoQueue.Enqueue(new MapThreadInfo<MeshData>(callback, meshData));
			}
		}

		/// <summary>
		/// This function is used to generate Map Data
		/// </summary>
		/// <param name="centre">Map Centre</param>
		/// <returns>Object with generated mapData</returns>
		private MapData GenerateMapData(Vector2 centre)
		{
			float[,] noiseMap = Noise.GenerateNoiseMap(mapChunkSize + 2, mapChunkSize + 2, 
				WorldConstants.WorldSeed, noiseScale, octaves, persistance, lacunarity, centre + offset, normalizeMode);

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

		/// <summary>
		/// Is called when values are changed in Inspector
		/// </summary>
		private void OnValidate()
		{
			if (lacunarity < 1)
				lacunarity = 1;
			
			if (octaves < 0)
				octaves = 0;
		}
	}

	/// <summary>
	/// This class describes the settings of Terrain levels, which are based on height of the map
	/// </summary>
	[Serializable]
	public struct TerrainType
	{
		public string name;
		public float height;
		public Color colour;
	}

	/// <summary>
	/// This class represents the heightMap and colourMap combined
	/// </summary>
	public struct MapData
	{
		public readonly float[,] heightMap;
		public readonly Color[] colourMap;

		public MapData(float[,] heightMap, Color[] colourMap)
		{
			this.heightMap = heightMap;
			this.colourMap = colourMap;
		}
	}
}