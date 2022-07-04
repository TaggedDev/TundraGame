namespace Environment
{
    public static class WorldConstants
    {
        public const int Scale = 8;
        private const float ChunkUpdateThreshold = 1f;
        public const float SqrChunkUpdateThreshold = ChunkUpdateThreshold * ChunkUpdateThreshold;
        private const float ColliderGenerationDistanceThreshold = 1f;
        public const float SqrColliderGenerationDistanceThreshold =
            ColliderGenerationDistanceThreshold * ColliderGenerationDistanceThreshold; 
        private const float EntityUpdateThreshold = .5f;
        public const float SqrEntityUpdateThreshold = EntityUpdateThreshold * EntityUpdateThreshold;
    }
}