using UnityEngine;

namespace Creatures.Player.Races
{
    [CreateAssetMenu(fileName = "New Race Configuration", menuName = "Player Race Configuration")]
    public class BasicPlayerRaceConfiguration : ScriptableObject
    {
        [SerializeField]
        private float _maxStarve;
        [SerializeField]
        private float _saturationTime;
        [SerializeField]
        private float _perfectTemperature;        
        [SerializeField]
        private float _absoluteTemperatureAmplitude;        
        [SerializeField]
        private float _maxWarmLevel;        
        [SerializeField]
        private float _maxHealth;        
        [SerializeField]
        private float _maxStamina;        
        [SerializeField]
        private float _maxLoadCapacity;       
        [SerializeField]
        private float _throwPrepareTime;
        [SerializeField]
        private float _maxDamageModificator;
        [SerializeField]
        private float _minDamageModificator;
        /// <summary>
        /// Maximal player starvation capacity.
        /// </summary>
        public float MaxStarve => _maxStarve;
        /// <summary>
        /// Time in seconds while which player won't spend saturation.
        /// </summary>
        public float SaturationTime => _saturationTime;
        /// <summary>
        /// Ideal player character temperature.
        /// </summary>
        public float PerfectTemperature => _perfectTemperature;
        /// <summary>
        /// Value which represents amplitude of comfort temperature for player.
        /// </summary>
        public float AbsoluteTemperatureAmplitude => _absoluteTemperatureAmplitude;
        /// <summary>
        /// Maximal player warm level.
        /// </summary>
        public float MaxWarmLevel => _maxWarmLevel;
        /// <summary>
        /// Maximal player character HP level.
        /// </summary>
        public float MaxHealth => _maxHealth;
        /// <summary>
        /// Maximal player character stamina.
        /// </summary>
        public float MaxStamina => _maxStamina;
        /// <summary>
        /// Maximal player load capacity.
        /// </summary>
        public float MaxLoadCapacity => _maxLoadCapacity;
        /// <summary>
        /// Preparation time before item throwing.
        /// </summary>
        public float ThrowPrepareTime => _throwPrepareTime;
        /// <summary>
        /// Minmal damage modificator. Used in lerp to calculate damage
        /// </summary>
        public float MaxDamageModificator => _maxDamageModificator;
        /// <summary>
        /// Maximal damage modificator. Used in lerp to calculate damage
        /// </summary>
        public float MinDamageModificator  => _minDamageModificator;
    }
}
