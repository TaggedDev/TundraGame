using UnityEditor;
using UnityEngine;

namespace Environment.Objects
{
    public class ObjectsGenerator : MonoBehaviour
    {
        private const int TERRAIN_MASK = 8; // layer mask of terrain chunks
        [SerializeField] private MapGenerator mapGenerator;
        [SerializeField] private GameObject treePrefab;
        [SerializeField] private int treeDensity;

        private void Start()
        {
            GenerateTrees(new Vector2(-183, -183));
        }

        private void GenerateTrees(Vector2 bottomRightCorner)
        {
            for (int i = (int)bottomRightCorner.x; i < mapGenerator.MapChunkSize * mapGenerator.NoiseScale; i += 47)
            {
                for (int j = (int)bottomRightCorner.y; j < mapGenerator.MapChunkSize * mapGenerator.NoiseScale; j += 47)
                {
                    /*float xSample = Random.Range(topLeftChunkCorner.x,
                        topLeftChunkCorner.x + mapGenerator.MapChunkSize);
                    float ySample = Random.Range(topLeftChunkCorner.y,
                        topLeftChunkCorner.y + mapGenerator.MapChunkSize);*/
                    print($"{i}, {j}");
                    Vector3 start = new Vector3(i, 500, j);

                    if (!Physics.Raycast(start, Vector3.down, out RaycastHit hit, Mathf.Infinity))
                        continue;

                    GameObject instantiated = (GameObject)PrefabUtility.InstantiatePrefab(treePrefab, transform);
                    instantiated.transform.position = hit.point;
                }
            }
        }
    }
}