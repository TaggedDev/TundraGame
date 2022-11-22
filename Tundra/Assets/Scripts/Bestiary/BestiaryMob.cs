namespace Bestiary
{
    /// <summary>
    /// A model of a mob in bestiary list
    /// </summary>
    [System.Serializable]
    public class BestiaryMob
    {
        // Description that will be displayed in bestiary
        public string MobDescription;
        // Mob name - title of mob card
        public string MobName;
        public bool IsKilled;
    }
}