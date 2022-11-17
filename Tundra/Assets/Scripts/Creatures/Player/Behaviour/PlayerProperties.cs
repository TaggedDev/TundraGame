using Creatures.Player.Races;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Creatures.Player.Behaviour
{
    /// <summary>
    /// Keeps current player character properties
    /// </summary>
    public class PlayerProperties : MonoBehaviour
    {
        private PlayerMovement _movement;

        /// <summary>
        /// Maximal starvation capacity.
        /// </summary>
        [SerializeField] private float maxStarve;
        /// <summary>
        /// Time in seconds while which player won't spend saturation.
        /// </summary>
        [SerializeField] private float saturationTime;
        /// <summary>
        /// Ideal player character temperature.
        /// </summary>
        [SerializeField] private float perfectTemperature;
        /// <summary>
        /// Value which represents amplitude of comfort temperature for player.
        /// </summary>
        [SerializeField] private float absoluteTemperatureAmplitude;
        /// <summary>
        /// Maximal player warm level.
        /// </summary>
        [SerializeField] private float maxWarmLevel;
        /// <summary>
        /// Maximal player character HP level.
        /// </summary>
        [SerializeField] private float maxHealth;
        /// <summary>
        /// Maximal player character stamina.
        /// </summary>
        [SerializeField] private float maxStamina;
        /// <summary>
        /// Maximal player load capacity.
        /// </summary>
        [SerializeField] private float maxLoadCapacity;
        /// <summary>
        /// Total time to prepare for throw.
        /// </summary>
        [SerializeField] private float hitPreparationTime;
        /// <summary>
        /// Player character race.
        /// </summary>
        [SerializeField] private BasicPlayerRaceConfiguration playerRace;
        /// <summary>
        /// Internal field for the current player's HP.
        /// </summary>
        [SerializeField] private float _currentHealth;
        /// <summary>
        /// Internal field for the current player speed coefficient.
        /// </summary>
        private float _currentSpeed;
        /// <summary>
        /// Internal field for the current player warm level.
        /// </summary>
        private float _currentWarm;
        /// <summary>
        /// Internal field for the current player starvation level.
        /// </summary>
        private float _currentStarvation;
        /// <summary>
        /// Internal field for the current player saturation.
        /// </summary>
        private float _currentSaturation;
        /// <summary>
        /// Internal field for the current player stamina.
        /// </summary>
        private float _currentStamina;
        /// <summary>
        /// Time of preparing for the hit.
        /// </summary>
        private float _currentHitPreparingTime;
        /// <summary>
        /// Time while which player hasn't eaten anything.
        /// </summary>
        internal float _currentStarvationTime;
        /// <summary>
        /// Ideal player temperature. 
        /// </summary>
        public float PerfectTemperature => perfectTemperature;
        /// <summary>
        /// Player character temperature amplitude.
        /// </summary>
        public float AbsoluteTemperatureAmplitude => absoluteTemperatureAmplitude;
        /// <summary>
        /// Maximal starvation level.
        /// </summary>
        public float MaxStarve => maxStarve;
        /// <summary>
        /// Maximal player health.
        /// </summary>
        public float MaxHealth => maxHealth;
        /// <summary>
        /// Maximal player stamina.
        /// </summary>
        public float MaxStamina => maxStamina;
        /// <summary>
        /// Maximal warm level. 
        /// </summary>
        public float MaxWarmLevel => maxWarmLevel;
        /// <summary>
        /// Time while which player will be saturated.
        /// </summary>
        public float SaturationTime => saturationTime;
        /// <summary>
        /// Maximal player load capacity.
        /// </summary>
        public float MaxLoadCapacity => maxLoadCapacity;
        /// <summary>
        /// Total hit preparation time.
        /// </summary>
        public float HitPreparationTime => hitPreparationTime;
        /// <summary>
        /// Current hit preparation progress.
        /// </summary>
        public float CurrentHitProgress
        {
            get => _currentHitPreparingTime;
            internal set
            {
                _currentHitPreparingTime = value;
            }
        }
        /// <summary>
        /// Current player health.
        /// </summary>
        public float CurrentHealth 
        {
            get
            {
                return _currentHealth;
            }
            internal set
            {
                _currentHealth = value;
            }
        }
        /// <summary>
        /// Current player stamina.
        /// </summary>
        public float CurrentStamina
        {
            get
            {
                return _currentStamina;
            }
            internal set
            {
                _currentStamina = value;
            }
        }

        /// <summary>
        /// Current speed coefficient.
        /// </summary>
        public float CurrentSpeed
        {
            get
            {
                return _movement.Speed;
            }
            internal set
            {
                _movement.Speed = value;
            }
        }

        /// <summary>
        /// Current warm level. 
        /// </summary>
        public float CurrentWarmLevel 
        {
            get
            {
                return _currentWarm;
            }
            internal set
            {
                _currentWarm = value;
            }
        }
        /// <summary>
        /// Current starvation capacity.
        /// </summary>
        public float CurrentStarvationCapacity 
        {
            get
            {
                return _currentStarvation;
            }
            internal set
            {
                _currentStarvation = value;
            }
        }
        /// <summary>
        /// Current saturation.
        /// </summary>
        public float CurrentSaturation 
        {
            get
            {
                return _currentSaturation;
            }
            internal set
            {
                _currentSaturation = value;
            }
        }

        void Start()
        {
            _movement = gameObject.GetComponent<PlayerMovement>();
            if (playerRace != null)
            {
                maxStarve = playerRace.MaxStarve;
                maxHealth = playerRace.MaxHealth;
                maxStamina = playerRace.MaxStamina;
                maxWarmLevel = playerRace.MaxWarmLevel;
                saturationTime = playerRace.SaturationTime;
                perfectTemperature = playerRace.PerfectTemperature;
                absoluteTemperatureAmplitude = playerRace.AbsoluteTemperatureAmplitude;
                maxLoadCapacity = playerRace.MaxLoadCapacity;
                hitPreparationTime = playerRace.HitPrepareTime;
            }
            _currentStarvation = maxStarve;
            _currentHealth = maxHealth;
            _currentStamina = maxStamina;
            _currentStarvationTime = saturationTime;
        }
    }
}
