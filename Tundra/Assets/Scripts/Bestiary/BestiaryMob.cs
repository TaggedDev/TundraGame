namespace Bestiary
{
    /// <summary>
    /// A model of a mob in bestiary list
    /// </summary>
    [System.Serializable]
    public class BestiaryMob
    {
        // Description that will be displayed in bestiary
        public string MobDescription { get; set; }
        
        // Mob name - title of mob card
        public string MobName { get; set; }

        // Represents if the mob has already once been killed in bestiary 
        public bool IsKilled { get; set; }
    }
}