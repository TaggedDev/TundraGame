using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;


namespace Environment
{
    public class EndlessTerrain : MonoBehaviour
    {
        public Transform viewer;

        private const float MaxViewDistance = 300;
        private static Vector2 _viewerPosition;
        private int _chunkSize;
        private int _chunksVisibleInViewDistance;

        private Dictionary<Vector2, TerrainChunk> _terrainChunks = new Dictionary<Vector2, TerrainChunk>();
        private List<TerrainChunk> _lastUpdatedTerrainChunks = new List<TerrainChunk>();
        
        private void Start()
        {
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
                    _terrainChunks.Add(viewedChunkCoord, new TerrainChunk(viewedChunkCoord, _chunkSize, transform));
                }
            }
        }

        private class TerrainChunk
        {
            private readonly GameObject _meshObject;
            private Bounds _bounds;
        
            public TerrainChunk(Vector2 coords, int size, Transform parent)
            {
                var position = coords * size;
                _bounds = new Bounds(position, Vector2.one * size);
                Vector3 positionV3 = new Vector3(position.x, 0, position.y);
            

                _meshObject = GameObject.CreatePrimitive(PrimitiveType.Plane);
                _meshObject.transform.position = positionV3;
                _meshObject.transform.localScale = Vector3.one * size / 10f;
                _meshObject.transform.parent = parent;
                SetVisible(false);
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