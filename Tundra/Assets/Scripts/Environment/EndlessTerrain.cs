using System.Collections.Generic;
using UnityEngine;


namespace Environment
{
    public class EndlessTerrain : MonoBehaviour
    {
        [SerializeField] private Material mapMaterial;
        private const float MaxViewDistance = 450;
        private static Vector2 _viewerPosition;
        private static MapGenerator _mapGenerator;
        private int _chunkSize;
        private int _chunksVisibleInViewDistance;

        private Dictionary<Vector2, TerrainChunk> _terrainChunks = new Dictionary<Vector2, TerrainChunk>();
        private List<TerrainChunk> _lastUpdatedTerrainChunks = new List<TerrainChunk>();
        
        public Transform viewer;
        
        private void Start()
        {
            _mapGenerator = GetComponent<MapGenerator>();
            _chunkSize = MapGenerator.MapChunkSize - 1;
            _chunksVisibleInViewDistance = Mathf.RoundToInt(MaxViewDistance / _chunkSize);
        }

        private void Update()
        {
            _viewerPosition = new Vector2(viewer.position.x,  viewer.position.z);
            UpdateVisibleChunks();
        }
        
        /// <summary>
        /// Updates visible/invisible parameter of chunks that are in 'visible' area  
        /// </summary>
        private void UpdateVisibleChunks()
        {
            // Disable unseen chunks and clear the list of previous update
            for (int i = 0; i < _lastUpdatedTerrainChunks.Count; i++)
                _lastUpdatedTerrainChunks[i].SetVisible(false);
            
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
                {
                    var chunk = _terrainChunks[viewedChunkCoord];
                    chunk.UpdateChunk();
                    if (chunk.IsVisible())
                        _lastUpdatedTerrainChunks.Add(chunk);
                }
                else
                {
                    _terrainChunks.Add(viewedChunkCoord, new TerrainChunk(viewedChunkCoord, _chunkSize, transform, mapMaterial));
                }
            }
        }

        private class TerrainChunk
        {
            private readonly GameObject _meshObject;
            private MeshRenderer _meshRenderer;
            private MeshFilter _meshFilter;
            private Bounds _bounds;
        
            public TerrainChunk(Vector2 coords, int size, Transform parent, Material material)
            {
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
                
                _mapGenerator.RequestMapData(OnMapDataReceived);
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="mapData"></param>
            private void OnMapDataReceived(MapData mapData)
            {
                _mapGenerator.RequestMeshData(mapData, OnMeshDataReceived);
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="meshData"></param>
            private void OnMeshDataReceived(MeshData meshData)
            {
                _meshFilter.mesh = meshData.CreateMesh();
            }
            
            public void UpdateChunk()
            {
                float distanceFromViewerToEdge = Mathf.Sqrt(_bounds.SqrDistance(_viewerPosition));
                bool visible = distanceFromViewerToEdge <= MaxViewDistance;
                SetVisible(visible);
            }

            public void SetVisible(bool visible)
            {
                _meshObject.SetActive(visible);
            }


            public bool IsVisible() => _meshObject.activeSelf;
        }
    }
}