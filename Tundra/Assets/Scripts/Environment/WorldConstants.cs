namespace Environment
{
    /// <summary>
    /// This class contains all constants of this World.
    /// </summary>
    public static class WorldConstants
    {
        public const int Scale = 1;
        private const float ChunkUpdateThreshold = 1f;
        public const float SqrChunkUpdateThreshold = ChunkUpdateThreshold * ChunkUpdateThreshold;
        private const float ColliderGenerationDistanceThreshold = 5f;
        public const float SqrColliderGenerationDistanceThreshold =
            ColliderGenerationDistanceThreshold * ColliderGenerationDistanceThreshold; 
        private const float EntityUpdateThreshold = .5f;
        public const float SqrEntityUpdateThreshold = EntityUpdateThreshold * EntityUpdateThreshold;
    }
}