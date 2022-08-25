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

        /// <summary>
        /// Максимальный запас голода у игрока.
        /// </summary>
        public float MaxStarve => maxStarve;
        /// <summary>
        /// Время в секундах, в течение которого игрок не будет терять голод.
        /// </summary>
        public float SaturationTime => saturationTime;
        /// <summary>
        /// Идеальная температура для персонажа.
        /// </summary>
        public float PerfectTemperature => perfectTemperature;
        /// <summary>
        /// Величина по модулю, которая определяет приемлемую мин/макс температуру здорового организма персонажа игрока
        /// </summary>
        public float AbsoluteTemperatureAmplitude => absoluteTemperatureAmplitude;
        /// <summary>
        /// Максимальный объём запаса тепла игрока.
        /// </summary>
        public float MaxWarmLevel => maxWarmLevel;
        /// <summary>
        /// Максимальное здоровье персонажа игрока.
        /// </summary>
        public float MaxHealth => maxHealth;
        /// <summary>
        /// Максимальная величина выносливости персонажа игрока.
        /// </summary>
        public float MaxStamina => maxStamina;
        /// <summary>
        /// Максимальная приемлемая нагрузка для персонажа.
        /// </summary>
        public float MaxLoadCapacity => maxLoadCapacity;
        /// <summary>
        /// Время подготовки к броску предмета.
        /// </summary>
        public float ThrowPrepareTime => throwPrepareTime;
    }
}
