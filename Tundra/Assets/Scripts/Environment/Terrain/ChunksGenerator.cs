using System;
using System.Collections.Generic;
using UnityEngine;

namespace Environment.Terrain
{
    public class ChunksGenerator : MonoBehaviour
    {
	    // Serialize variables
	    [SerializeField] private LODInfo[] detailLevels;
	    [SerializeField] private EntityInfo[] entityInfo;
	    [SerializeField] private Transform viewer;
	    [SerializeField] private Material mapMaterial;
	    [SerializeField] private int colliderLODIndex;

	    // Static fields 
	    private static float _maxViewDst;
	    private static Vector2 _viewerPosition;
		private static MapGenerator _mapGenerator;
		private static readonly List<TerrainChunk> _terrainChunksVisibleLastUpdate = new List<TerrainChunk>();

		// Fields
		private int _chunkSize;
		private readonly Dictionary<Vector2, TerrainChunk> _terrainChunkDictionary = new Dictionary<Vector2, TerrainChunk>();

		public Dictionary<Vector2, TerrainChunk> TerrainChunkDictionary => _terrainChunkDictionary;
		public List<TerrainChunk> TerrainChunksLastUpdate => _terrainChunksVisibleLastUpdate;
		
		// Variables
		private Vector2 viewerPositionOld;
		private int chunksVisibleInViewDst;
		
		[SerializeField] private float sqrSpatial;
		
		/// <summary>
		/// Function is called on object initialization
		/// </summary>
		private void Start()
		{
			_mapGenerator = FindObjectOfType<MapGenerator>();

			_maxViewDst = detailLevels [detailLevels.Length - 1].visibleDstThreshold;
			_chunkSize = MapGenerator.mapChunkSize - 1;
			chunksVisibleInViewDst = Mathf.RoundToInt(_maxViewDst / _chunkSize);

			UpdateVisibleChunks();
		}

		/// <summary>
		/// Function is called every frame
		/// </summary>
		private void Update() {
			_viewerPosition = new Vector2 (viewer.position.x, viewer.position.z) / WorldConstants.Scale;
			
			sqrSpatial = (viewerPositionOld - _viewerPosition).sqrMagnitude;
			
			if (sqrSpatial > WorldConstants.SqrChunkUpdateThreshold)
			{
				viewerPositionOld = _viewerPosition;
				UpdateVisibleChunks();
				foreach (TerrainChunk chunk in _terrainChunksVisibleLastUpdate)
					chunk.UpdateCollisionMesh();
			}
		}
		
		private void UpdateVisibleChunks() {

			foreach (var chunk in _terrainChunksVisibleLastUpdate)
				chunk.SetVisible (false);
			
			_terrainChunksVisibleLastUpdate.Clear ();
				
			int currentChunkCoordX = Mathf.RoundToInt (_viewerPosition.x / _chunkSize);
			int currentChunkCoordY = Mathf.RoundToInt (_viewerPosition.y / _chunkSize);
			
			for (int yOffset = -chunksVisibleInViewDst; yOffset <= chunksVisibleInViewDst; yOffset++) {
				for (int xOffset = -chunksVisibleInViewDst; xOffset <= chunksVisibleInViewDst; xOffset++) {
					Vector2 viewedChunkCoord = new Vector2 (currentChunkCoordX + xOffset, currentChunkCoordY + yOffset);

					if (_terrainChunkDictionary.ContainsKey (viewedChunkCoord)) {
						_terrainChunkDictionary [viewedChunkCoord].UpdateTerrainChunk ();
					} else
					{
						var chunk = new TerrainChunk(viewedChunkCoord, _chunkSize, detailLevels, colliderLODIndex,
							transform, mapMaterial, entityInfo, viewer.transform);
						_terrainChunkDictionary.Add(viewedChunkCoord, chunk);
					}
				}
			}
		}

		public class TerrainChunk
		{
			public EntityInfo[] EntitiesInfo => _entitiesInfo;

			private const int TERRAIN_LAYER_MASK = 8; 
			
			// Fields
			private readonly GameObject _meshObject;
			private readonly LODInfo[] _detailLevels;
			private readonly LODMesh[] _lodMeshes;
			private readonly int _colliderLODIndex;
			private readonly MeshRenderer _meshRenderer;
			private readonly MeshFilter _meshFilter;
			private readonly MeshCollider _meshCollider;
			private readonly Transform _player;
			private bool _mapDataReceived;
			private bool _hasSetCollider;
			private Bounds _bounds;
			private MapData _mapData;
			private int _chunkSize;
			
			//Entities
			private readonly EntityInfo[] _entitiesInfo;
			private readonly Dictionary<Vector2, Entity> _entities;

			// Variables
			private int previousLODIndex = -1;

			public TerrainChunk(Vector2 coord, int size, LODInfo[] detailLevels, int colliderLODIndex, Transform parent,
				Material material, EntityInfo[] entities, Transform player)
			{
				_player = player;
				_entities = new Dictionary<Vector2, Entity>();
				_entitiesInfo = entities;
				_detailLevels = detailLevels;
				_colliderLODIndex = colliderLODIndex;
				_chunkSize = size;
				
				var position = coord * size;
				_bounds = new Bounds(position,Vector2.one * size);
				Vector3 positionV3 = new Vector3(position.x,0,position.y);

				_meshObject = new GameObject("Terrain Chunk")
				{
					layer = TERRAIN_LAYER_MASK
				};
				_meshRenderer = _meshObject.AddComponent<MeshRenderer>();
				_meshFilter = _meshObject.AddComponent<MeshFilter>();
				_meshCollider = _meshObject.AddComponent<MeshCollider>();
				_meshRenderer.material = material;

				_meshObject.transform.position = positionV3 * WorldConstants.Scale;
				_meshObject.transform.parent = parent;
				_meshObject.transform.localScale = Vector3.one * WorldConstants.Scale;
				
				_lodMeshes = new LODMesh[detailLevels.Length];
				for (int i = 0; i < detailLevels.Length; i++)
				{
					_lodMeshes[i] = new LODMesh(detailLevels[i].lod);
					_lodMeshes[i].UpdateCallback += UpdateTerrainChunk;
					if (i == colliderLODIndex)
						_lodMeshes[i].UpdateCallback += UpdateCollisionMesh;
				}
				SetVisible(false);
				_mapGenerator.RequestMapData(position, OnMapDataReceived);
			}
			
			public void UpdateTerrainChunk()
			{
				if (!_mapDataReceived) return;
				
				float viewerDstFromNearestEdge = Mathf.Sqrt (_bounds.SqrDistance (_viewerPosition));
				bool visible = viewerDstFromNearestEdge <= _maxViewDst;

				if (visible)
				{
					int lodIndex = 0;

					for (int i = 0; i < _detailLevels.Length - 1; i++)
					{
						if (viewerDstFromNearestEdge > _detailLevels[i].visibleDstThreshold)
						{
							lodIndex = i + 1;
						}
						else
						{
							break;
						}
					}

					if (lodIndex != previousLODIndex)
					{
						LODMesh lodMesh = _lodMeshes[lodIndex];
						if (lodMesh.HasMesh)
						{
							previousLODIndex = lodIndex;
							_meshFilter.mesh = lodMesh.ThisMesh;
						}
						else if (!lodMesh.HasRequestedMesh)
						{
							lodMesh.RequestMesh(_mapData);
						}
					}

					_terrainChunksVisibleLastUpdate.Add(this);

				}

				SetVisible (visible);
			}

			public void UpdateCollisionMesh()
			{
				if (_hasSetCollider) return;
				
				float sqrDistanceViewerEdge = _bounds.SqrDistance(_viewerPosition);

				if (sqrDistanceViewerEdge < _detailLevels[_colliderLODIndex].SqrVisibleDistanceThreshold)
					if (!_lodMeshes[_colliderLODIndex].HasRequestedMesh)
						_lodMeshes[_colliderLODIndex].RequestMesh(_mapData);
				
				if (sqrDistanceViewerEdge < WorldConstants.SqrColliderGenerationDistanceThreshold)
					if (_lodMeshes[_colliderLODIndex].HasMesh)
					{
						_meshCollider.sharedMesh = _lodMeshes[_colliderLODIndex].ThisMesh;
						Debug.Log(_meshObject.name);
						_hasSetCollider = true;
					}

			}

			public void SetVisible(bool visible)
			{
				_meshObject.SetActive(visible);
			}
			
			private void OnMapDataReceived(MapData mapDataParameter) {
				_mapData = mapDataParameter;
				_mapDataReceived = true;
				
				Texture2D texture = TextureGenerator.TextureFromColourMap(mapDataParameter.colourMap,
					MapGenerator.mapChunkSize, MapGenerator.mapChunkSize);
				_meshRenderer.material.mainTexture = texture;
				_mapGenerator.mapDataCount++;
				UpdateTerrainChunk ();
			}
			
			public void UpdateChunkEntities()
			{
				if (!_hasSetCollider) return;

				Vector2 offset = new Vector2(_meshObject.transform.position.x, _meshObject.transform.position.z);

				int absHalfChunkSize = _chunkSize * WorldConstants.Scale / 2;
				/*float[,] noiseMap = Noise.GenerateNoiseMap(mapChunkSize, mapChunkSize, _mapGenerator.Seed,
					_mapGenerator.NoiseScale, _mapGenerator.Octaves, _mapGenerator.Persistance, _mapGenerator.Lacunarity,
					new Vector2(_player.position.x, _player.position.z), _mapGenerator.normalizeMode);*/
				for (int x = Mathf.FloorToInt(offset.x - absHalfChunkSize); x <= Mathf.FloorToInt(offset.x + absHalfChunkSize); x += 16)
				{
					for (int y = Mathf.FloorToInt(offset.y - absHalfChunkSize); y <= Mathf.FloorToInt(offset.y + absHalfChunkSize); y += 16)
					{
						Vector2 position = new Vector2(x, y);
						if (_entities.ContainsKey(position))
						{
							_entities[position].UpdateSelf();
						}
						else
						{
							if (Physics.Raycast(new Vector3(x, 500, y), Vector3.down, out RaycastHit info,
								    Mathf.Infinity, 1 << TERRAIN_LAYER_MASK))
							{
								var entityPosition = new Vector3(x, info.point.y, y);
								var tree = Instantiate(_entitiesInfo[0].entity, _meshObject.transform);
								tree.Initialise(entityPosition, _player);
								_entities.Add(position, tree);
							}
						}
					}
				}
			}
		}

		private class LODMesh {
			// Properties
			public Mesh ThisMesh { get; private set; }
			public bool HasRequestedMesh { get; private set; }
			public bool HasMesh { get; private set; }

			public event Action UpdateCallback;

			// Fields
			private readonly int _lod;

			public LODMesh(int lod) {
				_lod = lod;
			}
			
			public void RequestMesh(MapData mapData) {
				HasRequestedMesh = true;
				_mapGenerator.RequestMeshData(mapData, _lod, OnMeshDataReceived);
			}

			private void OnMeshDataReceived(MeshData meshData) {
				ThisMesh = meshData.CreateMesh ();
				HasMesh = true;
				_mapGenerator.meshDataCount++;
				UpdateCallback();
			}
		}

		[Serializable]
		public struct LODInfo {
			public int lod;
			public float visibleDstThreshold;
			public bool useForCollider;
			public float SqrVisibleDistanceThreshold => visibleDstThreshold * visibleDstThreshold;
		}

		[Serializable]
		public struct EntityInfo
		{
			public Entity entity;
			[Range(0, 1)] public float density;
		} 
    }
}