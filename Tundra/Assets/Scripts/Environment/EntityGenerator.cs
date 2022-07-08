using Environment.Terrain;
using UnityEngine;

namespace Environment
{
    /// <summary>
    /// GameObject with this class must be only 1 in the scene. This class checks the current player position
    /// and updates entities' positions and visibility.
    /// </summary>
    public class EntityGenerator : MonoBehaviour
    {
        // Fields
        [SerializeField] private ChunksGenerator chunksGenerator;
        [SerializeField] private Transform player;
        [SerializeField] private MapGenerator _mapGenerator;
        
        private static ChunksGenerator.TerrainChunk _playerChunk;
        private Vector2 _playerPosition;
        private Vector2 _oldPlayerPosition;
        [SerializeField] private Vector2 _previousChunkPosition;
        [SerializeField] private Vector2 chunkPosition;
        private int _chunkSize;
        [SerializeField] private float sqrSpatial;

        /// <summary>
        /// Sets the start values.
        /// </summary>
        private void Start()
        {
            _chunkSize = MapGenerator.mapChunkSize - 1;
            _playerPosition = new Vector2(player.position.x, player.position.z);
            _previousChunkPosition = GetCurrentChunkCoordinates();
            _playerChunk = chunksGenerator.TerrainChunkDictionary[_previousChunkPosition];
            _playerChunk?.UpdateChunkEntities();
        }

        /// <summary>
        /// Updates chunk's entities based on player's movement.
        /// </summary>
        private void Update()
        {
            _playerPosition = new Vector2(player.position.x, player.position.z) / WorldConstants.Scale;

            chunkPosition = GetCurrentChunkCoordinates();
            
            // If chunks generator has the position of current chunk (already calculated this part)
            if (_previousChunkPosition == chunkPosition)
            {
                // If player has moved for a long distance -> update entities
                sqrSpatial = (_oldPlayerPosition - _playerPosition).sqrMagnitude;
                if (sqrSpatial <= WorldConstants.SqrEntityUpdateThreshold) return;
            
                _oldPlayerPosition = _playerPosition;
                foreach (var chunk in chunksGenerator.TerrainChunksLastUpdate)
                {
                    chunk.UpdateChunkEntities();
                }
                //_playerChunk?.UpdateChunkEntities();
            }
            else
            {
                if (!chunksGenerator.TerrainChunkDictionary.ContainsKey(chunkPosition)) return;
                _playerChunk = chunksGenerator.TerrainChunkDictionary[chunkPosition];
                _previousChunkPosition = chunkPosition;
            }
        }

        /// <summary>
        /// Gets the coordinates of a chunk, on which player stays.
        /// </summary>
        /// <returns>Vector2 coordinates of chunk</returns>
        private Vector2 GetCurrentChunkCoordinates()
        {
            int currentChunkCoordX = Mathf.RoundToInt (_playerPosition.x / _chunkSize);
            int currentChunkCoordY = Mathf.RoundToInt (_playerPosition.y / _chunkSize);
            return new Vector2(currentChunkCoordX, currentChunkCoordY);
        }
    }
}