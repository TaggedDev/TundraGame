using System;
using UnityEngine;

namespace Environment
{
    public class MapGenerator : MonoBehaviour
    {
        private enum DrawMode
        {
            NoiseMap, ColorMap, MeshMap
        }
        
        private MapDisplay _display;

        [Range(0, 1)] 
        [SerializeField] private float persistance;
        [SerializeField] private float noiseScale;
        [SerializeField] private float lacunarity;
        [SerializeField] private int mapWidth;
        [SerializeField] private int mapHeight;
        [SerializeField] private int octaves;
        [SerializeField] private int seed;
        [SerializeField] private DrawMode drawMode;
        [SerializeField] private Vector2 offset;
        [SerializeField] private TerrainType[] regions;
        
        public bool autoUpdate;

        /// <summary>
        /// Generates and paints map object in the edit mode window and play mode and draws it according to the DrawMode 
        /// </summary>
        public void GenerateMap()
        {
            var noiseMap = Noise.GenerateNoiseMap(mapWidth, mapHeight, seed, noiseScale, octaves, persistance, lacunarity, offset);

            Color[] colorMap = GenerateColorsForNoiseMap(noiseMap);
            
            _display = GetComponent<MapDisplay>();
            
            switch (drawMode)
            {
                case DrawMode.NoiseMap:
                    _display.DrawTexture(TextureGenerator.TextureFromHeightMap(noiseMap));
                    break;
                case DrawMode.ColorMap:
                    _display.DrawTexture(TextureGenerator.TextureFromColorMap(colorMap, mapWidth, mapHeight));
                    break;
                case DrawMode.MeshMap:
                    _display.DrawMesh(MeshGenerator.GenerateTerrainMesh(noiseMap), TextureGenerator.TextureFromColorMap(colorMap, mapWidth, mapHeight));
                    break;
            }
        }

        /// <summary>
        /// Generates a color array for noise map 
        /// </summary>
        /// <param name="noiseMap">The map to made colors for</param>
        /// <returns>1D array of colors for noise map</returns>
        private Color[] GenerateColorsForNoiseMap(float[,] noiseMap)
        {
            var colorMap = new Color[mapWidth * mapHeight];
            
            for (int y = 0; y < mapHeight; y++)
            {
                for (int x = 0; x < mapWidth; x++)
                {
                    float currentHeight = noiseMap[x, y];
                    for (int i = 0; i < regions.Length; i++)
                    {
                        if (currentHeight <= regions[i].height)
                        {
                            colorMap[y * mapWidth + x] = regions[i].color;
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
            if (mapWidth < 1)
                mapWidth = 1;
            if (mapHeight < 1)
                mapHeight = 1;
            if (noiseScale <= 0)
                noiseScale = 0.001f;
            if (lacunarity < 1)
                lacunarity = 1;
            if (octaves < 0)
                octaves = 0;
        }
    }
    
    /// <summary>
    /// Struct to define Terrain (sand, grass, water etc)
    /// </summary>
    [Serializable]
    public struct TerrainType
    {
        public string name;
        public float height;
        public Color color;
    }

}

