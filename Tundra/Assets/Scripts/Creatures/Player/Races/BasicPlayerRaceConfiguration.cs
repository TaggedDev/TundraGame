using UnityEngine;

namespace Creatures.Player.Races
{
    [CreateAssetMenu(fileName = "New Race Configuration", menuName = "Player Race Configuration")]
    public class BasicPlayerRaceConfiguration : ScriptableObject
    {
        [SerializeField]
        private float maxStarve;
        [SerializeField]
        private float saturationTime;
        [SerializeField]
        private float perfectTemperature;        
        [SerializeField]
        private float absoluteTemperatureAmplitude;        
        [SerializeField]
        private float maxWarmLevel;        
        [SerializeField]
        private float maxHealth;        
        [SerializeField]
        private float maxStamina;        
        [SerializeField]
        private float maxLoadCapacity;       
        [SerializeField]
        private float throwPrepareTime;
        [SerializeField]
        private float hitPrepareTime;
        /// <summary>
        /// Maximal player starvation capacity.
        /// </summary>
        public float MaxStarve => maxStarve;
        /// <summary>
        /// Time in seconds while which player won't spend saturation.
        /// </summary>
        public float SaturationTime => saturationTime;
        /// <summary>
        /// Ideal player character temperature.
        /// </summary>
        public float PerfectTemperature => perfectTemperature;
        /// <summary>
        /// Value which represents amplitude of comfort temperature for player.
        /// </summary>
        public float AbsoluteTemperatureAmplitude => absoluteTemperatureAmplitude;
        /// <summary>
        /// Maximal player warm level.
        /// </summary>
        public float MaxWarmLevel => maxWarmLevel;
        /// <summary>
        /// Maximal player character HP level.
        /// </summary>
        public float MaxHealth => maxHealth;
        /// <summary>
        /// Maximal player character stamina.
        /// </summary>
        public float MaxStamina => maxStamina;
        /// <summary>
        /// Maximal player load capacity.
        /// </summary>
        public float MaxLoadCapacity => maxLoadCapacity;
        /// <summary>
        /// Preparation time before item throwing.
        /// </summary>
        public float ThrowPrepareTime => throwPrepareTime;
        /// <summary>
        /// Total hit preparation time.
        /// </summary>
        public float HitPrepareTime => hitPrepareTime; 
    }
}
