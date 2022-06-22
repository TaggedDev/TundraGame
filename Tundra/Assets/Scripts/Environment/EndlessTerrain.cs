﻿using System;
using System.Collections.Generic;
using UnityEngine;


namespace Environment
{
    public class EndlessTerrain : MonoBehaviour
    {
	    // Serialize variables
	    [SerializeField] private LODInfo[] detailLevels;
	    [SerializeField] private Transform viewer;
	    [SerializeField] private Material mapMaterial;
	    [SerializeField] private int colliderLODindex;

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
		
		// Fields
		private int _chunkSize;
		private readonly Dictionary<Vector2, TerrainChunk> _terrainChunkDictionary = new Dictionary<Vector2, TerrainChunk>();
		
		// Variables
		private Vector2 viewerPositionOld;
		private int chunksVisibleInViewDst;
		
		/// <summary>
		/// Function is called on object initialization
		/// </summary>
		private void Start() {
			_mapGenerator = FindObjectOfType<MapGenerator> ();

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
						_terrainChunkDictionary.Add(viewedChunkCoord,
							new TerrainChunk(viewedChunkCoord, _chunkSize, detailLevels, colliderLODindex, transform,
								mapMaterial));
					}
				}
			}
		}

		private class TerrainChunk
		{
			// Fields
			private readonly GameObject _meshObject;
			private readonly LODInfo[] _detailLevels;
			private readonly LODMesh[] _lodMeshes;
			private readonly int _colliderLODindex;
			private readonly MeshRenderer _meshRenderer;
			private readonly MeshFilter _meshFilter;
			private readonly MeshCollider _meshCollider;
			private bool _mapDataReceived;
			private bool _hasSetCollider;
			private Bounds _bounds;
			private MapData _mapData;
			
			// Variables
			private int previousLODIndex = -1;

			public TerrainChunk(Vector2 coord, int size, LODInfo[] detailLevels, int colliderLODindex, Transform parent, Material material) {
				_detailLevels = detailLevels;
				_colliderLODindex = colliderLODindex;

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
				SetVisible(false);

				_lodMeshes = new LODMesh[detailLevels.Length];
				for (int i = 0; i < detailLevels.Length; i++) {
					_lodMeshes[i] = new LODMesh(detailLevels[i].lod);
					_lodMeshes[i].UpdateCallback += UpdateTerrainChunk;
					if (i == colliderLODindex)
						_lodMeshes[i].UpdateCallback += UpdateCollisionMesh;
					
				}

				_mapGenerator.RequestMapData(position,OnMapDataReceived);
			}

			private void OnMapDataReceived(MapData mapDataParameter) {
				_mapData = mapDataParameter;
				_mapDataReceived = true;

				Texture2D texture = TextureGenerator.TextureFromColourMap(mapDataParameter.colourMap,
					MapGenerator.mapChunkSize, MapGenerator.mapChunkSize);
				_meshRenderer.material.mainTexture = texture;

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

				if (sqrDistanceViewerEdge < _detailLevels[_colliderLODindex].SqrVisibleDistanceThreshold)
					if (!_lodMeshes[_colliderLODindex].HasRequestedMesh)
						_lodMeshes[_colliderLODindex].RequestMesh(_mapData);
				
				if (sqrDistanceViewerEdge < ColliderGenerationDistanceThreshold * ColliderGenerationDistanceThreshold)
					if (_lodMeshes[_colliderLODindex].HasMesh)
						_meshCollider.sharedMesh = _lodMeshes[_colliderLODindex].ThisMesh;

			}

			public void SetVisible(bool visible) {
				_meshObject.SetActive (visible);
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
				UpdateCallback ();
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