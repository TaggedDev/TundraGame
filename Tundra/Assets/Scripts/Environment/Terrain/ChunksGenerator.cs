using System;
using System.Collections.Generic;
using UnityEngine;

namespace Environment.Terrain
{
    public class ChunksGenerator : MonoBehaviour
    {
	    // Serialize variables
	    [SerializeField] private LODInfo[] detailLevels;
	    [SerializeField] private EntityInfo[] _entityInfo;
	    [SerializeField] private Transform viewer;
	    [SerializeField] private Material mapMaterial;
	    [SerializeField] private int colliderLODIndex;

	    // Constants variables
	    private const int Scale = 8;
	    private const float ChunkUpdateThreshold = 5f;
	    private const float ColliderGenerationDistanceThreshold = 5f;
	    private const float SqrChunkUpdateThreshold = ChunkUpdateThreshold * ChunkUpdateThreshold;
	    private const float EntityUpdateThreshold = .05f;
	    private const float SqrEntityUpdateThreshold = EntityUpdateThreshold * EntityUpdateThreshold;

	    // Static fields 
	    private static float _maxViewDst;
	    private static Vector2 _viewerPosition;
		private static MapGenerator _mapGenerator;
		private static readonly List<TerrainChunk> _terrainChunksVisibleLastUpdate = new List<TerrainChunk>();

		public static List<TerrainChunk> TerrainChunksVisibleLastUpdate => _terrainChunksVisibleLastUpdate;

		// Fields
		private int _chunkSize;
		private readonly Dictionary<Vector2, TerrainChunk> _terrainChunkDictionary = new Dictionary<Vector2, TerrainChunk>();
		
		// Variables
		private Vector2 viewerPositionOld;
		private int chunksVisibleInViewDst;
		
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
			_viewerPosition = new Vector2 (viewer.position.x, viewer.position.z) / Scale;

			if (_viewerPosition != viewerPositionOld)
				foreach (TerrainChunk chunk in _terrainChunksVisibleLastUpdate) 
					chunk.UpdateCollisionMesh();
			float sqrSpatial = (viewerPositionOld - _viewerPosition).sqrMagnitude;
			if (sqrSpatial > SqrEntityUpdateThreshold)
			{
				viewerPositionOld = _viewerPosition;
				foreach (var chunk in _terrainChunksVisibleLastUpdate)
					chunk.UpdateChunkEntities(_mapGenerator.MapChunkSize * Scale, chunk.EntitiesInfo);
			}
			
			if (sqrSpatial > SqrChunkUpdateThreshold)
			{
				viewerPositionOld = _viewerPosition;
				UpdateVisibleChunks();
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
							transform, mapMaterial, _entityInfo, viewer.transform);
						_terrainChunkDictionary.Add(viewedChunkCoord, chunk);
					}
				}
			}
		}

		public class TerrainChunk
		{
			public EntityInfo[] EntitiesInfo => _entitiesInfo;

			private const int TERRAIN_LAYERMASK = 8; 
			
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

				var position = coord * size;
				_bounds = new Bounds(position,Vector2.one * size);
				Vector3 positionV3 = new Vector3(position.x,0,position.y);

				_meshObject = new GameObject("Terrain Chunk")
				{
					layer = TERRAIN_LAYERMASK
				};
				_meshRenderer = _meshObject.AddComponent<MeshRenderer>();
				_meshFilter = _meshObject.AddComponent<MeshFilter>();
				_meshCollider = _meshObject.AddComponent<MeshCollider>();
				_meshRenderer.material = material;

				_meshObject.transform.position = positionV3 * Scale;
				_meshObject.transform.parent = parent;
				_meshObject.transform.localScale = Vector3.one * Scale;
				
				_lodMeshes = new LODMesh[detailLevels.Length];
				for (int i = 0; i < detailLevels.Length; i++)
				{
					_lodMeshes[i] = new LODMesh(detailLevels[i].lod);
					_lodMeshes[i].UpdateCallback += UpdateTerrainChunk;
					if (i == colliderLODIndex)
						_lodMeshes[i].UpdateCallback += UpdateCollisionMesh;
				}
				SetVisible(false);
				_mapGenerator.RequestMapData(position,OnMapDataReceived);
			}
			
			public void UpdateTerrainChunk()
			{
				if (!_mapDataReceived) return;
				
				float viewerDstFromNearestEdge = Mathf.Sqrt (_bounds.SqrDistance (_viewerPosition));
				bool visible = viewerDstFromNearestEdge <= _maxViewDst;

				if (visible) {
					int lodIndex = 0;

					for (int i = 0; i < _detailLevels.Length - 1; i++) {
						if (viewerDstFromNearestEdge > _detailLevels [i].visibleDstThreshold) {
							lodIndex = i + 1;
						} else {
							break;
						}
					}
						
					if (lodIndex != previousLODIndex) {
						LODMesh lodMesh = _lodMeshes [lodIndex];
						if (lodMesh.HasMesh) {
							previousLODIndex = lodIndex;
							_meshFilter.mesh = lodMesh.ThisMesh;
						} else if (!lodMesh.HasRequestedMesh) {
							lodMesh.RequestMesh (_mapData);
						}
					}
					_terrainChunksVisibleLastUpdate.Add (this);
					
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
				
				if (sqrDistanceViewerEdge < ColliderGenerationDistanceThreshold * ColliderGenerationDistanceThreshold)
					if (_lodMeshes[_colliderLODIndex].HasMesh)
					{
						_meshCollider.sharedMesh = _lodMeshes[_colliderLODIndex].ThisMesh;
						_hasSetCollider = true;
						UpdateChunkEntities(_mapGenerator.MapChunkSize * Scale, EntitiesInfo);
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
			
			public void UpdateChunkEntities(int mapChunkSize, EntityInfo[] entitiesInfo)
			{
				if (!_hasSetCollider) return;

				for (int x = 0; x <= 50; x += 3)
				{
					for (int y = 0; y <= 50; y += 3)
					{
						if (!Physics.Raycast(new Vector3(x, 500, y), Vector3.down, out RaycastHit info, Mathf.Infinity, 1 << TERRAIN_LAYERMASK))
							continue;

						Vector2 position = new Vector2(x, y);

						if (_entities.ContainsKey(position))
						{
							_entities[position].UpdateSelf();
						}
						else
						{
							var tree = Instantiate(_entitiesInfo[0].entity, new Vector3(x, info.point.y, y), Quaternion.identity);
							tree.Initialise(position, _player);
							_entities.Add(position, tree);
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
			[Range(1, 100)] public float density;
		} 
    }
}