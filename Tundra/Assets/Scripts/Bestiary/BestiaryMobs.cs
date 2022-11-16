namespace Bestiary
{
    /// <summary>
    /// A model to represent a list of mobs in JSON deserialization
    /// </summary>
    [System.Serializable]
    public class BestiaryMobs
    {
        // The array of all mobs serialized
        public BestiaryMob[] mobs;
    }
}