using System;
using UnityEngine;

namespace Environment
{
    public class MapGenerator : MonoBehaviour
    {
        private enum DrawMode
        {
            NoiseMap, ColorMap
        }
        
        private MapDisplay _display;

        [SerializeField] private DrawMode drawMode;
        [SerializeField] private int mapWidth;
        [SerializeField] private int mapHeight;
        [SerializeField] private int octaves;
        [SerializeField] private int seed;
        [SerializeField] private float noiseScale;
        [SerializeField] private float lacunarity;
        [Range(0, 1)]
        [SerializeField] private float persistance;
        [SerializeField] private Vector2 offset;

        [SerializeField] private TerrainType[] regions;
        
        public bool autoUpdate;

        public void GenerateMap()
        {
            var noiseMap = Noise.GenerateNoiseMap(mapWidth, mapHeight, seed, noiseScale, octaves, persistance, lacunarity, offset);

            Color[] colorMap = new Color[mapWidth * mapHeight];
            
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
            
            _display = GetComponent<MapDisplay>();
            if (drawMode == DrawMode.NoiseMap)
                _display.DrawTexture(TextureGenerator.TextureFromHeightMap(noiseMap));
            else if (drawMode == DrawMode.ColorMap)
                _display.DrawTexture(TextureGenerator.TextureFromColorMap(colorMap, mapWidth, mapHeight));
        }

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
    
    [Serializable]
    public struct TerrainType
    {
        public string name;
        public float height;
        public Color color;
    }

}

