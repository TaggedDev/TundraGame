using UnityEngine;

namespace Environment
{
    public class MapGenerator : MonoBehaviour
    {
        private MapDisplay _display;
    
        [SerializeField] private int mapWidth;
        [SerializeField] private int mapHeight;
        [SerializeField] private float noiseScale;

        public bool autoUpdate;

        public void GenerateMap()
        {
            var noiseMap = Noise.GenerateNoiseMap(mapWidth, mapHeight, noiseScale);
            _display = GetComponent<MapDisplay>();
            _display.DrawNoiseMap(noiseMap);
        }
    
    }
}
