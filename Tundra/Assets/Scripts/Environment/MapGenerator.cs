﻿using System;
using UnityEngine;
using System.Threading;
using System.Collections.Generic;

namespace Environment
{
	public class MapGenerator : MonoBehaviour
	{
		// Inner structs and enums
		public enum DrawMode
		{
			NoiseMap,
			ColourMap,
			Mesh
		}
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
		
		// Constants
		public const int mapChunkSize = 47;
		
		// Fields
		[SerializeField] [Range(0, 6)] private int editorPreviewLOD;
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
		
		// Variables
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
			else if (drawMode == DrawMode.Mesh)
			{
				display.DrawMesh(
					MeshGenerator.GenerateTerrainMesh(mapData.heightMap, meshHeightMultiplier, meshHeightCurve,
						editorPreviewLOD),
					TextureGenerator.TextureFromColourMap(mapData.colourMap, mapChunkSize, mapChunkSize));
			}
		}

		public void RequestMapData(Vector2 centre, Action<MapData> callback)
		{
			ThreadStart threadStart = delegate { MapDataThread(centre, callback); };

			new Thread(threadStart).Start();
		}
		
		public void RequestMeshData(MapData mapData, int lod, Action<MeshData> callback)
		{
			ThreadStart threadStart = delegate { MeshDataThread(mapData, lod, callback); };

			new Thread(threadStart).Start();
		}
		
		// Private methods
		private void MapDataThread(Vector2 centre, Action<MapData> callback)
		{
			MapData mapData = GenerateMapData(centre);
			lock (mapDataThreadInfoQueue)
			{
				mapDataThreadInfoQueue.Enqueue(new MapThreadInfo<MapData>(callback, mapData));
			}
		}

		private void MeshDataThread(MapData mapData, int lod, Action<MeshData> callback)
		{
			MeshData meshData =
				MeshGenerator.GenerateTerrainMesh(mapData.heightMap, meshHeightMultiplier, meshHeightCurve, lod);
			lock (meshDataThreadInfoQueue)
			{
				meshDataThreadInfoQueue.Enqueue(new MapThreadInfo<MeshData>(callback, meshData));
			}
		}

		private void Update()
		{
			if (mapDataThreadInfoQueue.Count > 0)
			{
				for (int i = 0; i < mapDataThreadInfoQueue.Count; i++)
				{
					MapThreadInfo<MapData> threadInfo = mapDataThreadInfoQueue.Dequeue();
					threadInfo.callback(threadInfo.parameter);
				}
			}

			if (meshDataThreadInfoQueue.Count > 0)
			{
				for (int i = 0; i < meshDataThreadInfoQueue.Count; i++)
				{
					MapThreadInfo<MeshData> threadInfo = meshDataThreadInfoQueue.Dequeue();
					threadInfo.callback(threadInfo.parameter);
				}
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

		private void OnValidate()
		{
			if (lacunarity < 1)
				lacunarity = 1;
			
			if (octaves < 0)
				octaves = 0;
		}
	}

	[Serializable]
	public struct TerrainType
	{
		public string name;
		public float height;
		public Color colour;
	}

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