using System.Collections.Generic;
using ScriptableObjects.Environment;
using ScriptableObjects.Environment.Nature;
using UnityEngine;

namespace Environment.Objects
{
    public class ObjectsGenerator : MonoBehaviour
    {
        // Serialize variables
        [SerializeField] private Transform viewer;
        [SerializeField] private GameObject tree;
        [SerializeField] private float spawnOffset;
        [SerializeField] private float fillPercent;

        // Fields
		private readonly Dictionary<Vector2, GameObject> _terrainChunkDictionary = new Dictionary<Vector2, GameObject>();

		// Variables
		private Vector2 viewerPositionOld;
		
		/// <summary>
		/// Function is called on object initialization
		/// </summary>
		private void Start()
		{
			FindObjectOfType<ObjectMapGenerator>();
			//UpdateVisibleObjects ();
			UpdateEntities(viewer.transform.position, 20);
		}
		
		/*private void SpawnEntities(Vector2 topLeftChunkCorner)
		{
			var map = _objectsGenerator.GenerateDensityMap(viewer.position);
			int i = 0;
			int xPos = (int)topLeftChunkCorner.x;
			for (int xIndex = 0; xIndex < map.GetLength(0); xIndex++)
			{
				int yPos = (int)topLeftChunkCorner.y;
				for (int yIndex = 0; yIndex < map.GetLength(1); yIndex++)
				{
					i++;
					//if (chance > map[xIndex, yIndex]) continue;
					Vector3 start = new Vector3(xPos, 500, yPos);
					

					if (!Physics.Raycast(start, Vector3.down, out RaycastHit hit, Mathf.Infinity))
					{
						Debug.Log($"{xPos}, {yPos} -> {i}");
						continue;
					}
					
					GameObject instantiated = (GameObject)PrefabUtility.InstantiatePrefab(tree, transform);
					instantiated.name = $"Tree [{i}]";
					instantiated.transform.position = hit.point;
					yPos += (int)Scale;
				}
				xPos += (int)Scale;
			}
		}*/

		private void UpdateEntities(Vector3 playerPos3D, int radius)
		{
			Vector2 startPos = new Vector2(playerPos3D.x - radius, playerPos3D.z - radius);
			float[,] noise = Noise.GenerateNoiseMap(radius * 2, radius * 2, startPos);
			for (float x = startPos.x; x < startPos.x + noise.GetLength(0); x += 12)
				for (float y = startPos.y; y < startPos.y + noise.GetLength(1); y += 12)
					SpawnEntity(x, y, radius);
			Debug.Log(_terrainChunkDictionary.Count);
		}

		private void SpawnEntity(float x, float y, int radius)
		{
			float noiseValue = Noise.GetNoiseValue(x, y, radius);
			if (!Physics.Raycast(new Vector3(x, 500, y), Vector3.down, out RaycastHit hit, Mathf.Infinity))
				return;

			if (noiseValue >= fillPercent)
				return;

			Vector2 pos = new Vector2(x + Random.Range(-spawnOffset, spawnOffset),
				y + Random.Range(-spawnOffset, spawnOffset));
			GameObject instantiated = Instantiate(tree, new Vector3(pos.x, hit.point.y, pos.y), Quaternion.identity);
			instantiated.AddComponent<TreeEntity>();
			_terrainChunkDictionary.Add(pos, instantiated);
		}
    }
}

/*
 private void SpawnEntities(Vector2 topLeftChunkCorner)
		{
			var map = _objectsGenerator.GenerateDensityMap(viewer.position);
			int i = 0;
			int xPos = (int)topLeftChunkCorner.x;
			for (int xIndex = 0; xIndex < map.GetLength(0); xIndex++)
			{
				int yPos = (int)topLeftChunkCorner.y;
				for (int yIndex = 0; yIndex < map.GetLength(1); yIndex++)
				{
					i++;
					if (chance <= map[xIndex, yIndex])
					{
						Vector3 start = new Vector3(xPos, 500, yPos);

						if (!Physics.Raycast(start, Vector3.down, out RaycastHit hit, Mathf.Infinity))
						{
							Debug.Log($"{xPos}, {yPos} -> {i}");
							continue;
						}
					
						GameObject instantiated = (GameObject)PrefabUtility.InstantiatePrefab(tree, transform);
						instantiated.name = $"Tree [{i}]";
						instantiated.transform.position = hit.point;
					}
					yPos += (int)Scale;
				}
				xPos += (int)Scale;
			}
		}
 * 
 */