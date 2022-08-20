using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace Environment.Terrain
{
	/// <summary>
	/// Class is *only once* attached to an object in scene and is responsible for generating terrain chunks as children
	/// objects.
	/// </summary>
    public class ChunksGenerator : MonoBehaviour
    {
	    // Serialize variables
	    [SerializeField] private LODInfo[] detailLevels;
	    [SerializeField] private EntityLevel[] entityInfo;
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
		private float sqrSpatial;
		
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
		/// Function is called every frame.
		/// </summary>
		private void Update() {
			_viewerPosition = new Vector2 (viewer.position.x, viewer.position.z) / WorldConstants.Scale;
			
			sqrSpatial = (viewerPositionOld - _viewerPosition).sqrMagnitude;
			
			if (sqrSpatial > WorldConstants.SqrChunkUpdateThreshold)
			{
				viewerPositionOld = _viewerPosition;
				
				// Get current chunk and bake the navmesh for it 
				int currentChunkCoordX = Mathf.RoundToInt (_viewerPosition.x / _chunkSize);
				int currentChunkCoordY = Mathf.RoundToInt (_viewerPosition.y / _chunkSize);
				_terrainChunkDictionary[new Vector2(currentChunkCoordX, currentChunkCoordY)].BakeNavMesh();
				
				// Update all chunks due level of distance
				UpdateVisibleChunks();
				foreach (TerrainChunk chunk in _terrainChunksVisibleLastUpdate)
					chunk.UpdateCollisionMesh();
			}
		}
		
		/// <summary>
		/// Is responsible for updating chunks that were active in the last update. The amount of such depends on
		/// 'chunksVisibleInViewDst' variable.
		/// In case there is a new chunks to be rendered, we add it in dictionary with it's V2 coords.
		/// </summary>
		private void UpdateVisibleChunks() {

			foreach (var chunk in _terrainChunksVisibleLastUpdate)
				chunk.SetVisibility (false);

			_terrainChunksVisibleLastUpdate.Clear();
				
			int currentChunkCoordX = Mathf.RoundToInt (_viewerPosition.x / _chunkSize);
			int currentChunkCoordY = Mathf.RoundToInt (_viewerPosition.y / _chunkSize);

			for (int yOffset = -chunksVisibleInViewDst; yOffset <= chunksVisibleInViewDst; yOffset++)
			{
				for (int xOffset = -chunksVisibleInViewDst; xOffset <= chunksVisibleInViewDst; xOffset++)
				{
					Vector2 viewedChunkCoord = new Vector2(currentChunkCoordX + xOffset, currentChunkCoordY + yOffset);

					if (_terrainChunkDictionary.ContainsKey(viewedChunkCoord))
					{
						_terrainChunkDictionary[viewedChunkCoord].UpdateTerrainChunk();
					}
					else
					{
						var chunk = new TerrainChunk(viewedChunkCoord, _chunkSize, detailLevels, colliderLODIndex,
							transform, mapMaterial, entityInfo, viewer.transform);
						_terrainChunkDictionary.Add(viewedChunkCoord, chunk);
					}
				}
			}
		}
		
		/// <summary>
		/// This class describes the terrain chunk and it's entities
		/// </summary>
		public class TerrainChunk
		{
			public EntityLevel[] EntitiesInfo => _entitiesInfo;

			private const int TERRAIN_LAYER_MASK = 8; 
			
			// Fields
			private readonly LODInfo[] _detailLevels;
			private readonly LODMesh[] _lodMeshes;
			private readonly Transform _player;
			private readonly int _chunkSize;
			private readonly int _colliderLODIndex;
			private GameObject _meshObject;
			private MeshRenderer _meshRenderer;
			private MeshFilter _meshFilter;
			private MeshCollider _meshCollider;
			private bool _mapDataReceived;
			private bool _hasGeneratedEntities;
			private bool _hasSetCollider;
			private Bounds _bounds;
			private MapData _mapData;
			private NavMeshSurface _navMeshSurface;

			//Entities
			private readonly EntityLevel[] _entitiesInfo;
			private readonly Dictionary<Vector2, Entity> _entities;

			// Variables
			private int previousLODIndex = -1;

			/// <summary>
			/// The constructor of Terrain Chunk.
			/// </summary>
			/// <param name="coord">The coordinates of terrain chunk game object.</param>
			/// <param name="size">The chunk size of the object.</param>
			/// <param name="detailLevels">The array of LODInfo from ChunksGenerator.</param>
			/// <param name="colliderLODIndex">Current index of LOD.</param>
			/// <param name="parent">The parent object.</param>
			/// <param name="material">Material to apply to chunk.</param>
			/// <param name="entities">Array of entities to choose from.</param>
			/// <param name="player">The transform of player</param>
			public TerrainChunk(Vector2 coord, int size, LODInfo[] detailLevels, int colliderLODIndex, Transform parent,
				Material material, EntityLevel[] entities, Transform player)
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

				InstantiateTerrainChunk();
				SetChunkTransform();

				_lodMeshes = new LODMesh[detailLevels.Length];
				for (int i = 0; i < detailLevels.Length; i++)
				{
					_lodMeshes[i] = new LODMesh(detailLevels[i].lod);
					_lodMeshes[i].UpdateCallback += UpdateTerrainChunk;
					if (i == colliderLODIndex)
						_lodMeshes[i].UpdateCallback += UpdateCollisionMesh;
				}
				SetVisibility(false);
				_mapGenerator.RequestMapData(position, OnMapDataReceived);

				// Instantiates and adds components to meshObject
				void InstantiateTerrainChunk()
				{
					_meshObject = new GameObject("Terrain Chunk")
					{
						layer = TERRAIN_LAYER_MASK
					};
					_meshRenderer = _meshObject.AddComponent<MeshRenderer>();
					_meshFilter = _meshObject.AddComponent<MeshFilter>();
					_meshCollider = _meshObject.AddComponent<MeshCollider>();
					_meshRenderer.material = material;
				}
				
				// Sets chunk's transform parameters  
				void SetChunkTransform()
				{
					_meshObject.transform.position = positionV3 * WorldConstants.Scale;
					_meshObject.transform.parent = parent;
					_meshObject.transform.localScale = Vector3.one * WorldConstants.Scale;
					_meshObject.isStatic = true;
				}
				
				_navMeshSurface = _meshObject.AddComponent<NavMeshSurface>();
			}

			/// <summary>
			/// Updates the level of detail of current terrain chunk based on its position relative to the viewer. 
			/// </summary>
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
				SetVisibility (visible);
			}

			/// <summary>
			/// Bakes the navmesh for current chunk map
			/// </summary>
			public void BakeNavMesh()
			{
				Debug.Log($"Baking with {_meshObject.name}");
				_navMeshSurface.BuildNavMesh();
			}

			/// <summary>
			/// Updates the collision mesh depending on the position relative to player's object.
			/// </summary>
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
						_hasSetCollider = true;
					}
			}

			/// <summary>
			/// Sets the current terrain chunk object visible depending on visible value
			/// </summary>
			/// <param name="visible">Visibility parameter, as is</param>
			public void SetVisibility(bool visible)
			{
				_meshObject.SetActive(visible);
			}

			/// <summary>
			/// Is called by MapGenerator in a new thread when TerrainChunk requests the MapData for _mapData field.
			/// Also calls the level of detail update on the chunk.
			/// We use new thread on generating MapData because calculating MapData is a costly process.
			/// </summary>
			/// <param name="mapData">MapData to be saved</param>
			private void OnMapDataReceived(MapData mapData) {
				_mapData = mapData;
				_mapDataReceived = true;
				
				Texture2D texture = TextureGenerator.TextureFromColourMap(mapData.colourMap,
					MapGenerator.mapChunkSize, MapGenerator.mapChunkSize);
				_meshRenderer.material.mainTexture = texture;
				_mapGenerator.mapDataCount++;
				UpdateTerrainChunk ();
			}
			
			/// <summary>
			/// If chunk has a collider set, we generate or render entities in _entities list of this chunk 
			/// </summary>
			public void UpdateChunkEntities()
			{
				if (!_hasSetCollider) return;

				Vector2 offset = new Vector2(_meshObject.transform.position.x, _meshObject.transform.position.z);

				int absHalfChunkSize = _chunkSize * WorldConstants.Scale / 2;
				for (int x = Mathf.FloorToInt(offset.x - absHalfChunkSize); x <= Mathf.FloorToInt(offset.x + absHalfChunkSize); x += 16)
				{
					for (int y = Mathf.FloorToInt(offset.y - absHalfChunkSize); y <= Mathf.FloorToInt(offset.y + absHalfChunkSize); y += 16)
					{
						Vector2 position = new Vector2(x, y);
						if (_hasGeneratedEntities)
						{
							if (_entities.ContainsKey(position))
							{
								_entities[position].UpdateSelf();
							}
						}
						else
						{
							if (!Physics.Raycast(new Vector3(x, 500, y), Vector3.down, out RaycastHit info,
								    Mathf.Infinity, 1 << TERRAIN_LAYER_MASK)) continue;

							var height = info.point.y;
							IterateThroughBiomes(x, height, y, position);
							
						}
						
					}
				}
				_hasGeneratedEntities = true;
			}

			private void IterateThroughBiomes(float x, float height, float z, Vector2 position)
			{
				for (int i = 0; i < _entitiesInfo.Length; i++)
				{
					// If blank chance has proceeded, ignore this X;Z position
					if (height < _entitiesInfo[i].topSpawnHeightThreshold)
					{
						SpawnEntityInPosition(x, height, z, _entitiesInfo[i].entities, position, _entitiesInfo[i].blankChance);
						return;
					}
				}
			}
			
			private void SpawnEntityInPosition(float x, float height, float z, IReadOnlyList<Entity> entities, 
				Vector2 position, float blankChance)
			{
				float rnd = Random.Range(0f, 1f);
				// If blank chance has proceeded - don't spawn anything
				if (blankChance >= rnd)
					return;
				
				// Else, choose an object from the list and spawn an entity
				var entityPosition = new Vector3(x, height, z);
				var tree = Instantiate(GetRandomEntity(entities), _meshObject.transform);
				tree.Initialise(entityPosition, _player);
				_entities.Add(position, tree);
			}

			private Entity GetRandomEntity(IReadOnlyList<Entity> entities)
			{
				Entity entity = null;
				float totalChance = entities.Sum(entityItem => entityItem.SpawnRateForLevel);

				float chanceNumber = Random.Range(0f, totalChance);

				totalChance = 0f;

				foreach (var entityItem in entities)
				{
					entity = entityItem;
					totalChance += entityItem.SpawnRateForLevel;
					if (totalChance >= chanceNumber)
						return entity;
				}
				
				return entity;
			}
		}
		
		/// <summary>
		/// This class describes the level of detail of the Mesh
		/// </summary>
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

		/// <summary>
		/// Contains some basic information about current level of detail
		/// </summary>
		[Serializable]
		public struct LODInfo {
			public int lod;
			public float visibleDstThreshold;
			public bool useForCollider;
			public float SqrVisibleDistanceThreshold => visibleDstThreshold * visibleDstThreshold;
		}

		/// <summary>
        /// Contains some basic information about this entity
        /// </summary>
		[Serializable]
		public struct EntityLevel
		{
			public Entity[] entities;
			[Range(0, 1)]
			public float blankChance;
			public float topSpawnHeightThreshold;
		} 
    }
}