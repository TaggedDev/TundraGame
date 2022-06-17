using System.Collections.Generic;
using UnityEngine;


namespace Environment
{
    public class EndlessTerrain : MonoBehaviour
    {
        private const float viewerChunkUpdateThreshold = 25f;
        private const float sqrViewerChunkUpdateThreshold = viewerChunkUpdateThreshold * viewerChunkUpdateThreshold;
        
        
        [SerializeField] private Material mapMaterial;
        [SerializeField] private LODInfo[] detailLevels;
        [SerializeField] private Transform viewer;
        
        private static float _maxViewDistance;
        private static Vector2 _viewerPosition;
        private Vector2 _viewerPositionOld;
        private static MapGenerator _mapGenerator;
        private int _chunkSize;
        private int _chunksVisibleInViewDistance;
        
        private static List<TerrainChunk> _lastUpdatedTerrainChunks = new List<TerrainChunk>();
        private Dictionary<Vector2, TerrainChunk> _terrainChunks = new Dictionary<Vector2, TerrainChunk>();
        
        
        private void Start()
        {
            _maxViewDistance = detailLevels[detailLevels.Length - 1].visibleDistanceTreshold;
            _mapGenerator = GetComponent<MapGenerator>();
            _chunkSize = MapGenerator.MapChunkSize - 1;
            _chunksVisibleInViewDistance = Mathf.RoundToInt(_maxViewDistance / _chunkSize);
            UpdateVisibleChunks();
        }

        private void Update()
        {
            _viewerPosition = new Vector2(viewer.position.x,  viewer.position.z);
            if ((_viewerPosition - _viewerPositionOld).sqrMagnitude > sqrViewerChunkUpdateThreshold)
            {
                _viewerPositionOld = _viewerPosition;
                UpdateVisibleChunks();
            }
                
        }
        
        /// <summary>
        /// Updates visible/invisible parameter of chunks that are in 'visible' area  
        /// </summary>
        private void UpdateVisibleChunks()
        {
            // Disable unseen chunks and clear the list of previous update
            foreach (var chunk in _lastUpdatedTerrainChunks)
                chunk.SetVisible(false);

            _lastUpdatedTerrainChunks.Clear();
            
            /*foreach (var chunk in _lastUpdateChunks) 
                chunk.UpdateChunk();*/
            
            int currentChunkCoordX = Mathf.RoundToInt(_viewerPosition.x / _chunkSize);
            int currentChunkCoordY = Mathf.RoundToInt(_viewerPosition.y / _chunkSize);

            // Loop through chunks are in visible area and fill the Dictionary
            for (int offsetY = -_chunksVisibleInViewDistance; offsetY <= _chunksVisibleInViewDistance; offsetY++)
            for (int offsetX = -_chunksVisibleInViewDistance; offsetX <= _chunksVisibleInViewDistance; offsetX++)
            {
                Vector2 viewedChunkCoord = new Vector2(currentChunkCoordX + offsetX, currentChunkCoordY + offsetY);
                if (_terrainChunks.ContainsKey(viewedChunkCoord))
                    _terrainChunks[viewedChunkCoord].UpdateChunk();
                else
                    _terrainChunks.Add(viewedChunkCoord, new TerrainChunk(viewedChunkCoord, _chunkSize, detailLevels, transform, mapMaterial));
            }
        }

        private class TerrainChunk
        {
            private LODInfo[] _detailLevels;
            private LODMesh[] _lodMeshes;

            private MapData _mapData;
            private bool _mapDataReceived;
            
            
            private readonly GameObject _meshObject;
            private MeshRenderer _meshRenderer;
            private MeshFilter _meshFilter;
            private Bounds _bounds;
            private int _previousDetailsIndex;
            
        
            public TerrainChunk(Vector2 coords, int size, LODInfo[] detailLevels, Transform parent, Material material)
            {
                _previousDetailsIndex = -1;
                _detailLevels = detailLevels;
                var position = coords * size;
                _bounds = new Bounds(position, Vector2.one * size);
                Vector3 positionV3 = new Vector3(position.x, 0, position.y);


                _meshObject = new GameObject();
                _meshRenderer = _meshObject.AddComponent<MeshRenderer>();
                _meshFilter = _meshObject.AddComponent<MeshFilter>();
                _meshRenderer.material = material;
                
                _meshObject.transform.position = positionV3;
                _meshObject.transform.parent = parent;
                SetVisible(false);

                _lodMeshes = new LODMesh[detailLevels.Length];
                for (int i = 0; i < detailLevels.Length; i++)
                {
                    _lodMeshes[i] = new LODMesh(_detailLevels[i].lod, UpdateChunk);
                }
                
                _mapGenerator.RequestMapData(position, OnMapDataReceived);
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="mapData"></param>
            private void OnMapDataReceived(MapData mapData)
            {
                _mapData = mapData;
                _mapDataReceived = true;

                Texture2D texture = TextureGenerator.TextureFromColorMap(mapData.ColorMap, MapGenerator.MapChunkSize,
                    MapGenerator.MapChunkSize);
                _meshRenderer.material.mainTexture = texture;
                
                UpdateChunk();
            }

            public void UpdateChunk()
            {
                if (!_mapDataReceived) return;
                
                float distanceFromViewerToEdge = Mathf.Sqrt(_bounds.SqrDistance(_viewerPosition));
                bool visible = distanceFromViewerToEdge <= _maxViewDistance;

                if (visible)
                {
                    int lodIndex = 0;
                    for (int i = 0; i < _detailLevels.Length -1; i++)
                    {
                        if (distanceFromViewerToEdge > _detailLevels[i].visibleDistanceTreshold)
                        {
                            lodIndex = i + 1;
                        }
                        else
                        {
                            break;
                        }
                    }
                    
                    if (lodIndex != _previousDetailsIndex)
                    {
                        LODMesh lodMesh = _lodMeshes[lodIndex];
                        if (lodMesh.hasMesh)
                        {
                            _previousDetailsIndex = lodIndex;
                            _meshFilter.mesh = lodMesh.mesh;
                        }
                        else if (!lodMesh.hasRequestedMesh)
                        {
                            lodMesh.RequestMesh(_mapData);
                        }
                    }
                    
                    _lastUpdatedTerrainChunks.Add(this);
                }
                
                SetVisible(visible);
            }

            public void SetVisible(bool visible)
            {
                _meshObject.SetActive(visible);
            }


            public bool IsVisible() => _meshObject.activeSelf;
        }

        private class LODMesh
        {
            public Mesh mesh;
            public bool hasRequestedMesh;
            public bool hasMesh;
            private int _lod;
            private System.Action _updateCallback;

            public LODMesh(int lod, System.Action updateCallback)
            {
                _lod = lod;
                _updateCallback = updateCallback;
            }

            private void OnMeshDataReceived(MeshData meshData)
            {
                mesh = meshData.CreateMesh();
                hasMesh = true;

                _updateCallback();
            }

            public void RequestMesh(MapData mapData)
            {
                hasRequestedMesh = true;
                _mapGenerator.RequestMeshData(mapData, _lod, OnMeshDataReceived);
            }
        }
    }

    [System.Serializable]
    public struct LODInfo
    {
        public int lod;
        public float visibleDistanceTreshold;

    }
}