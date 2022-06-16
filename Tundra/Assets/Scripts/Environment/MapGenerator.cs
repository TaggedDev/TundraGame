using System;
using UnityEngine;
using System.Threading;
using System.Collections.Generic;

namespace Environment
{
    public class MapGenerator : MonoBehaviour
    {
        private enum DrawMode
        {
            NoiseMap, ColorMap, MeshMap
        }

        public const int MapChunkSize = 241;
        [Range(0, 6)]
        [SerializeField] private int editorLOD;
        private MapDisplay _display;

        [Range(0, 1)] 
        [SerializeField] private float persistance;
        [SerializeField] private float noiseScale;
        [SerializeField] private float lacunarity;
        [SerializeField] private float heightMultiplier;
        [SerializeField] private int octaves;
        [SerializeField] private int seed;
        [SerializeField] private DrawMode drawMode;
        [SerializeField] private Vector2 offset;
        [SerializeField] private TerrainType[] regions;
        [SerializeField] private AnimationCurve meshHeightCurve;
        private Queue<MapThreadInfo<MapData>> _mapDataThreadInfoQueue = new Queue<MapThreadInfo<MapData>>();
        private Queue<MapThreadInfo<MeshData>> _meshDataThreadInfoQueue = new Queue<MapThreadInfo<MeshData>>();

        public bool autoUpdate;

        private void Update()
        {
            if (_mapDataThreadInfoQueue.Count > 0)
            {
                for (int i = 0; i < _mapDataThreadInfoQueue.Count; i++)
                {
                    var threadInfo = _mapDataThreadInfoQueue.Dequeue();
                    threadInfo.callBack(threadInfo.parameter);
                }   
            }

            if (_meshDataThreadInfoQueue.Count > 0)
            {
                for (int i = 0; i < _meshDataThreadInfoQueue.Count; i++)
                {
                    var threadInfo = _meshDataThreadInfoQueue.Dequeue();
                    threadInfo.callBack(threadInfo.parameter);
                }
            }
        }

        /*
        /// <summary>
        /// 
        /// </summary>
        public void DrawMapInEditor()
        {
            MapData mapData = GenerateMapData();
            _display = GetComponent<MapDisplay>();
            
           switch (drawMode)
            {
                case DrawMode.NoiseMap:
                    _display.DrawTexture(TextureGenerator.TextureFromHeightMap(mapData.HeightMap));
                    break;
                case DrawMode.ColorMap:
                    _display.DrawTexture(TextureGenerator.TextureFromColorMap(mapData.ColorMap, MapChunkSize, MapChunkSize));
                    break;
                case DrawMode.MeshMap:
                    _display.DrawMesh(MeshGenerator.GenerateTerrainMesh(mapData.HeightMap, heightMultiplier, meshHeightCurve, levelOfDetail),
                        TextureGenerator.TextureFromColorMap(mapData.ColorMap, MapChunkSize, MapChunkSize));
                    break;
            }
        }*/

        /// <summary>
        /// 
        /// </summary>
        /// <param name="callback"></param>
        public void RequestMapData(Action<MapData> callback)
        {
            ThreadStart threadStart = delegate {
                MapDataThread (callback);
            };
            new Thread (threadStart).Start ();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="callback"></param>
        private void MapDataThread(Action<MapData> callback)
        {
            MapData mapData = GenerateMapData();
            lock (_mapDataThreadInfoQueue) {
                _mapDataThreadInfoQueue.Enqueue(new MapThreadInfo<MapData>(callback, mapData));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mapData"></param>
        /// <param name="callback"></param>
        public void RequestMeshData(MapData mapData, int lod, Action<MeshData> callback)
        {
            ThreadStart threadStart = delegate {
                MeshDataThread (mapData, lod, callback);
            };

            new Thread (threadStart).Start ();
        }

        private void MeshDataThread(MapData mapData, int lod, Action<MeshData> callback)
        {
            MeshData meshData =
                MeshGenerator.GenerateTerrainMesh(mapData.HeightMap, heightMultiplier, meshHeightCurve, lod);
            lock (_meshDataThreadInfoQueue)
            {
                _meshDataThreadInfoQueue.Enqueue(new MapThreadInfo<MeshData>(callback, meshData)); 
            }
        }

        /// <summary>
        /// Generates and paints map object in the edit mode window and play mode and draws it according to the DrawMode 
        /// </summary>
        public MapData GenerateMapData()
        {
            var noiseMap = Noise.GenerateNoiseMap(MapChunkSize, MapChunkSize, seed, noiseScale, 
                octaves, persistance, lacunarity, offset);
            Color[] colorMap = GenerateColorsForNoiseMap(noiseMap);
            return new MapData(noiseMap, colorMap);
        }

        /// <summary>
        /// Generates a color array for noise map 
        /// </summary>
        /// <param name="noiseMap">The map to made colors for</param>
        /// <returns>1D array of colors for noise map</returns>
        private Color[] GenerateColorsForNoiseMap(float[,] noiseMap)
        {
            var colorMap = new Color[MapChunkSize * MapChunkSize];
            
            for (int y = 0; y < MapChunkSize; y++)
            {
                for (int x = 0; x < MapChunkSize; x++)
                {
                    float currentHeight = noiseMap[x, y];
                    for (int i = 0; i < regions.Length; i++)
                    {
                        if (currentHeight <= regions[i].height)
                        {
                            colorMap[y * MapChunkSize + x] = regions[i].color;
                            break;
                        }
                    }
                }
            }

            return colorMap;
        }
        
        /// <summary>
        /// Calls when any variable is edited in Inspector 
        /// </summary>
        private void OnValidate()
        {
            if (noiseScale <= 0)
                noiseScale = 0.001f;
            if (lacunarity < 1)
                lacunarity = 1;
            if (octaves < 0)
                octaves = 0;
        }

        struct MapThreadInfo<T>
        {
            public readonly Action<T> callBack;
            public readonly T parameter;

            public MapThreadInfo(Action<T> callBack, T parameter)
            {
                this.callBack = callBack;
                this.parameter = parameter;
            }
        }
    }
    
    /// <summary>
    /// Struct to define Terrain (sand, grass, water etc)
    /// </summary>
    [Serializable]
    public struct TerrainType
    {
        public readonly string name;
        public readonly float height;
        public readonly Color color;
    }

    public struct MapData
    {
        public MapData(float[,] heightMap, Color[] colorMap)
        {
            HeightMap = heightMap;
            ColorMap = colorMap;
        }

        public float[,] HeightMap;
        public Color[] ColorMap;
        
    }

}

