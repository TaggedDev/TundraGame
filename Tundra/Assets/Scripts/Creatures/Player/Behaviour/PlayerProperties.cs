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
        /// <summary>
        /// Maximal starvation capacity.
        /// </summary>
        [SerializeField] private float maxStarvePoints;
        /// <summary>
        /// Time in seconds while which player won't spend saturation.
        /// </summary>
        [SerializeField] private float saturationPoints;
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
        [SerializeField] private float maxWarmthPoints;
        /// <summary>
        /// Maximal player character HP level.
        /// </summary>
        [SerializeField] private float maxHealthPoints;
        /// <summary>
        /// Maximal player character stamina.
        /// </summary>
        [SerializeField] private float maxStaminaPoints;
        /// <summary>
        /// Maximal player load capacity.
        /// </summary>
        [SerializeField] private float maxLoadCapacity;
        /// <summary>
        /// Preparation time before item throwing.
        /// </summary>
        [SerializeField] private float throwPrepareTime;
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
        [SerializeField] private float currentHealthPoints;

        /// <summary>
        /// Private field determines whether player is eating or not
        /// </summary>
        private bool _isHoldingFood;

        /// <summary>
        /// The time (sec) left to eat the equipped food item 
        /// </summary>
        private float _foodConsumingTimeLeft;

        /// <summary>
        /// The time (sec) left to eat the equipped food item 
        /// </summary>
        public float FoodConsumingTimeLeft
        {
            get => _foodConsumingTimeLeft;
            set => _foodConsumingTimeLeft = value;
        }

        /// <summary>
        /// The time takes to eat the equipped food item
        /// </summary>
        public const float FOOD_CONSUMING_MAX_TIME = 1f;

        /// <summary>
        /// Internal field for the current player speed coefficient.
        /// </summary>
        private float _currentSpeed;
        /// <summary>
        /// Internal field for the current player warm level.
        /// </summary>
        private float _currentWarmthPoints;
        /// <summary>
        /// Internal field for the current player starvation level.
        /// </summary>
        private float _currentStarvePoints;
        /// <summary>
        /// Internal field for the current player saturation.
        /// </summary>
        private float _currentSaturationPoints;
        /// <summary>
        /// Internal field for the current player stamina.
        /// </summary>
        private float _currentStaminaPoints;
        /// <summary>
        /// Time of preparing for the hit.
        /// </summary>
        private float _currentHitPreparingTime;
        /// <summary>
        /// Internal field for the progress of the item throwing.
        /// </summary>
        internal float _throwLoadingProgress;

        /// <summary>
        /// Ideal player temperature. 
        /// </summary>
        public float PerfectTemperature => perfectTemperature;
        /// <summary>
        /// Player character temperature amplitude.
        /// </summary>
        public float AbsoluteTemperatureAmplitude => absoluteTemperatureAmplitude;
        /// <summary>
        /// Throw preparation time.
        /// </summary>
        public float ThrowPrepareTime => throwPrepareTime;
        /// <summary>
        /// Maximal starvation level.
        /// </summary>
        public float MaxStarvePoints => maxStarvePoints;
        /// <summary>
        /// Maximal player health.
        /// </summary>
        public float MaxHealthPoints => maxHealthPoints;
        /// <summary>
        /// Maximal player stamina.
        /// </summary>
        public float MaxStaminaPoints => maxStaminaPoints;
        /// <summary>
        /// Maximal warm level. 
        /// </summary>
        public float MaxWarmthPoints => maxWarmthPoints;

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
        public float CurrentHealthPoints 
        {
            get
            {
                return currentHealthPoints;
            }
            internal set
            {
                currentHealthPoints = value;
            }
        }
        /// <summary>
        /// Current player stamina.
        /// </summary>
        public float CurrentStaminaPoints
        {
            get
            {
                return _currentStaminaPoints;
            }
            internal set
            {
                _currentStaminaPoints = value;
            }
        }

        /// <summary>
        /// Current speed coefficient.
        /// </summary>
        public float CurrentSpeed
        {
            get => _currentSpeed;
            internal set => _currentSpeed = value;
        }

        /// <summary>
        /// Current warm level. 
        /// </summary>
        public float CurrentWarmthPoints 
        {
            get => _currentWarmthPoints;
            internal set => _currentWarmthPoints = value;
        }
        
        /// <summary>
        /// Current starvation capacity.
        /// </summary>
        public float CurrentStarvePoints 
        {
            get => _currentStarvePoints;
            internal set
            {
                if (value > maxStarvePoints)
                    _currentStarvePoints = maxStarvePoints;
                else
                    _currentStarvePoints = value;
            }
        }
        /// <summary>
        /// Current saturation.
        /// </summary>
        public float CurrentSaturationPoints 
        {
            get => _currentSaturationPoints;
            internal set => _currentSaturationPoints = value;
        }
        /// <summary>
        /// Player eating status 
        /// </summary>
        public bool IsHoldingFood
        {
            get => _isHoldingFood;
            set => _isHoldingFood = value;
        }

        private void Start()
        {
            if (playerRace != null)
            {
                _foodConsumingTimeLeft = FOOD_CONSUMING_MAX_TIME;
                maxStarvePoints = playerRace.MaxStarve;
                maxHealthPoints = playerRace.MaxHealth;
                maxStaminaPoints = playerRace.MaxStamina;
                maxWarmthPoints = playerRace.MaxWarmLevel;
                saturationPoints = playerRace.SaturationTime;
                perfectTemperature = playerRace.PerfectTemperature;
                absoluteTemperatureAmplitude = playerRace.AbsoluteTemperatureAmplitude;
                throwPrepareTime = playerRace.ThrowPrepareTime;
                maxLoadCapacity = playerRace.MaxLoadCapacity;
                hitPreparationTime = playerRace.HitPrepareTime;
            }
            _currentStarvePoints = maxStarvePoints;
            currentHealthPoints = maxHealthPoints;
            _currentStaminaPoints = maxStaminaPoints;
            _throwLoadingProgress = throwPrepareTime;
        }
    }
}
