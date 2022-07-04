using Environment.Terrain;
using UnityEngine;

namespace Environment
{
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

        private void Start()
        {
            _chunkSize = MapGenerator.mapChunkSize - 1;
            _playerPosition = new Vector2(player.position.x, player.position.z);
            _previousChunkPosition = GetCurrentChunkCoordinates();
            _playerChunk = chunksGenerator.TerrainChunkDictionary[_previousChunkPosition];
            _playerChunk?.UpdateChunkEntities(_mapGenerator.MapChunkSize * WorldConstants.Scale, _playerChunk.EntitiesInfo[0]);
        }

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
                _playerChunk?.UpdateChunkEntities(_mapGenerator.MapChunkSize * WorldConstants.Scale, _playerChunk.EntitiesInfo[0]);

            }
            else
            {
                if (!chunksGenerator.TerrainChunkDictionary.ContainsKey(chunkPosition)) return;
                _playerChunk = chunksGenerator.TerrainChunkDictionary[chunkPosition];
                _previousChunkPosition = chunkPosition;
            }
        }

        private Vector2 GetCurrentChunkCoordinates()
        {
            int currentChunkCoordX = Mathf.RoundToInt (_playerPosition.x / _chunkSize);
            int currentChunkCoordY = Mathf.RoundToInt (_playerPosition.y / _chunkSize);
            return new Vector2(currentChunkCoordX, currentChunkCoordY);
        }
    }
}