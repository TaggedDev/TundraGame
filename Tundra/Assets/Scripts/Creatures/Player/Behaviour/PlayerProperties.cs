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
    /// Хранит состояние основных параметров персонажа игрока.
    /// </summary>
    public class PlayerProperties : Component
    {
        /// <summary>
        /// Максимальный запас голода у игрока.
        /// </summary>
        [SerializeField] private float maxStarve;
        /// <summary>
        /// Время в секундах, в течение которого игрок не будет терять голод.
        /// </summary>
        [SerializeField] private float saturationTime;
        /// <summary>
        /// Идеальная температура для персонажа.
        /// </summary>
        [SerializeField] private float perfectTemperature;
        /// <summary>
        /// Величина по модулю, которая определяет приемлемую мин/макс температуру здорового организма персонажа игрока
        /// </summary>
        [SerializeField] private float absoluteTemperatureAmplitude;
        /// <summary>
        /// Максимальный объём запаса тепла игрока.
        /// </summary>
        [SerializeField] private float maxWarmLevel;
        /// <summary>
        /// Максимальное здоровье персонажа игрока.
        /// </summary>
        [SerializeField] private float maxHealth;
        /// <summary>
        /// Максимальная величина выносливости персонажа игрока.
        /// </summary>
        [SerializeField] private float maxStamina;
        /// <summary>
        /// Максимальная приемлемая нагрузка для персонажа.
        /// </summary>
        [SerializeField] private float maxLoadCapacity;
        /// <summary>
        /// Время подготовки к броску предмета.
        /// </summary>
        [SerializeField] private float throwPrepareTime;
        /// <summary>
        /// Раса персонажа игрока.
        /// </summary>
        [SerializeField] private BasicPlayerRaceConfiguration playerRace;
        /// <summary>
        /// Текущее здоровье игрока (внутреннее поле).
        /// </summary>
        private float _currentHealth;
        /// <summary>
        /// Текущая скорость игрока (внутреннее поле).
        /// </summary>
        private float _currentSpeed;
        /// <summary>
        /// Запас тепла игрока (внутреннее поле).
        /// </summary>
        private float _currentWarm;
        /// <summary>
        /// Запас голода игрока (внутреннее поле).
        /// </summary>
        private float _currentStarvation;
        /// <summary>
        /// Запас насыщения игрока (внутреннее поле).
        /// </summary>
        private float _currentSaturation;
        /// <summary>
        /// Запас выносливости игрока (внутреннее поле).
        /// </summary>
        private float _currentStamina;
        /// <summary>
        /// Прогресс подготовки к броску предмета.
        /// </summary>
        internal float _throwLoadingProgress;
        /// <summary>
        /// Время, в течение которого игрок голодает (ничего уже не ел).
        /// </summary>
        internal float _currentStarvationTime;
        /// <summary>
        /// Идеальная температура для персонажа игрока. 
        /// </summary>
        public float PerfectTemperature => perfectTemperature;
        /// <summary>
        /// Амплитуда температуры персонажа.
        /// </summary>
        public float AbsoluteTemperatureAmplitude => absoluteTemperatureAmplitude;
        /// <summary>
        /// Время подготовки к броску. 
        /// </summary>
        public float ThrowPrepareTime => throwPrepareTime;
        /// <summary>
        /// Максимальное значение голода.
        /// </summary>
        public float MaxStarve => maxStarve;
        /// <summary>
        /// Максимальное здоровье.
        /// </summary>
        public float MaxHealth => maxHealth;
        /// <summary>
        /// Максимальная выносливость.
        /// </summary>
        public float MaxStamina => maxStamina;
        /// <summary>
        /// Максимальный запас тепла.
        /// </summary>
        public float MaxWarmLevel => maxWarmLevel;
        /// <summary>
        /// Время, в течение которого персонаж будет сыт.
        /// </summary>
        public float SaturationTime => saturationTime;
        /// <summary>
        /// Максимальная грузоподъёмность персонажа.
        /// </summary>
        public float MaxLoadCapacity => maxLoadCapacity;
        /// <summary>
        /// Текущее здоровье.
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
        /// Текущая выносливость.
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
        /// Текущая скорость.
        /// </summary>
        public float CurrentSpeed
        {
            get
            {
                return _currentSpeed;
            }
            internal set
            {
                _currentSpeed = value;
            }
        }

        /// <summary>
        /// Текущий запас тепла.
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
        /// Текущее значение голода.
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
        /// Текущее насыщение.
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

        public void Initialize(BasicPlayerRaceConfiguration race)
        {
            if (playerRace == null) playerRace = race;
            if (playerRace != null)
            {
                maxStarve = playerRace.MaxStarve;
                maxHealth = playerRace.MaxHealth;
                maxStamina = playerRace.MaxStamina;
                maxWarmLevel = playerRace.MaxWarmLevel;
                saturationTime = playerRace.SaturationTime;
                perfectTemperature = playerRace.PerfectTemperature;
                absoluteTemperatureAmplitude = playerRace.AbsoluteTemperatureAmplitude;
                throwPrepareTime = playerRace.ThrowPrepareTime;
                maxLoadCapacity = playerRace.MaxLoadCapacity;
            }
            _currentStarvation = maxStarve;
            _currentHealth = maxHealth;
            _currentStamina = maxStamina;
            _currentStarvationTime = saturationTime;
            _throwLoadingProgress = throwPrepareTime;
        }
    }
}
