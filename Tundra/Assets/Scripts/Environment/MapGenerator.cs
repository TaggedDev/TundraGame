using System;
using UnityEngine;

namespace Environment
{
    public class MapGenerator : MonoBehaviour
    {
        private MapDisplay _display;
    
        [SerializeField] private int mapWidth;
        [SerializeField] private int mapHeight;
        [SerializeField] private int octaves;
        [SerializeField] private int seed;
        [SerializeField] private float noiseScale;
        [SerializeField] private float lacunarity;
        [Range(0, 1)]
        [SerializeField] private float persistance;
        [SerializeField] private Vector2 offset;

        public bool autoUpdate;

        public void GenerateMap()
        {
            var noiseMap = Noise.GenerateNoiseMap(mapWidth, mapHeight, seed, noiseScale, octaves, persistance, lacunarity, offset);
            _display = GetComponent<MapDisplay>();
            _display.DrawNoiseMap(noiseMap);
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
}
