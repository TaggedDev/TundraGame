using System;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;


namespace Environment
{
    public class ChunksGenerator : MonoBehaviour
    {
	    // Serialize variables
	    [SerializeField] private LODInfo[] detailLevels;
	    [SerializeField] private Transform viewer;
	    [SerializeField] private Material mapMaterial;
	    [SerializeField] private int colliderLODIndex;

	    // Constants variables
	    private const float Scale = 8f;
	    private const float ChunkUpdateThreshold = 5f;
	    private const float ColliderGenerationDistanceThreshold = 5f;
	    private const float SqrChunkUpdateThreshold = ChunkUpdateThreshold * ChunkUpdateThreshold;

	    // Static fields 
	    private static float _maxViewDst;
	    private static Vector2 _viewerPosition;
		private static MapGenerator _mapGenerator;
		private static readonly List<TerrainChunk> TerrainChunksVisibleLastUpdate = new List<TerrainChunk>();

		public Dictionary<Vector2, TerrainChunk> TerrainChunkDictionary => _terrainChunkDictionary;

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

			UpdateVisibleChunks ();
		}

		/// <summary>
		/// Function is called every frame
		/// </summary>
		private void Update() {
			_viewerPosition = new Vector2 (viewer.position.x, viewer.position.z) / Scale;

			if (_viewerPosition != viewerPositionOld)
			{
				foreach (TerrainChunk chunk in TerrainChunksVisibleLastUpdate)
				{
					chunk.UpdateCollisionMesh();
				}
			}
			
			if ((viewerPositionOld - _viewerPosition).sqrMagnitude <= SqrChunkUpdateThreshold) return;
			
			viewerPositionOld = _viewerPosition;
			UpdateVisibleChunks();
		}
		
		private void UpdateVisibleChunks() {

			foreach (var chunk in TerrainChunksVisibleLastUpdate)
				chunk.SetVisible (false);
			
			TerrainChunksVisibleLastUpdate.Clear ();
				
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
							transform, mapMaterial);
						_terrainChunkDictionary.Add(viewedChunkCoord, chunk);
					}
				}
			}
		}

		public class TerrainChunk
		{
			// Fields
			private readonly GameObject _meshObject;
			private readonly LODInfo[] _detailLevels;
			private readonly LODMesh[] _lodMeshes;
			private readonly int _colliderLODIndex;
			private readonly MeshRenderer _meshRenderer;
			private readonly MeshFilter _meshFilter;
			private readonly MeshCollider _meshCollider;
			private bool _mapDataReceived;
			private bool _hasSetCollider;
			private Bounds _bounds;
			private MapData _mapData;

			// Variables
			private int previousLODIndex = -1;

			public TerrainChunk(Vector2 coord, int size, LODInfo[] detailLevels, int colliderLODIndex, Transform parent, Material material)
			{
				_detailLevels = detailLevels;
				_colliderLODIndex = colliderLODIndex;

				var position = coord * size;
				_bounds = new Bounds(position,Vector2.one * size);
				Vector3 positionV3 = new Vector3(position.x,0,position.y);

				_meshObject = new GameObject("Terrain Chunk");
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

			private void OnMapDataReceived(MapData mapDataParameter) {
				_mapData = mapDataParameter;
				_mapDataReceived = true;
				
				Texture2D texture = TextureGenerator.TextureFromColourMap(mapDataParameter.colourMap,
					MapGenerator.mapChunkSize, MapGenerator.mapChunkSize);
				_meshRenderer.material.mainTexture = texture;
				_mapGenerator.mapDataCount++;
				UpdateTerrainChunk ();
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

					TerrainChunksVisibleLastUpdate.Add (this);
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
						_meshCollider.sharedMesh = _lodMeshes[_colliderLODIndex].ThisMesh;

			}

			public void SetVisible(bool visible)
			{
				_meshObject.SetActive(visible);
			}
		}

		private class LODMesh {
			// Properties
			public Mesh ThisMesh { get; set; }
			public bool HasRequestedMesh { get; set; }
			public bool HasMesh { get; set; }

			public event Action UpdateCallback;

			// Fields
			private readonly int _lod;

			public LODMesh(int lod) {
				_lod = lod;
			}
			
			public void RequestMesh(MapData mapData) {
				HasRequestedMesh = true;
				_mapGenerator.RequestMeshData (mapData, _lod, OnMeshDataReceived);
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
    }
}