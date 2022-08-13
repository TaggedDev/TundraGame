using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Creatures.Player.Races
{
    public class BasicPlayerRaceConfiguration : ScriptableObject
    {
        /// <summary>
        /// Максимальный запас голода у игрока.
        /// </summary>
        public float MaxStarve { get; }
        /// <summary>
        /// Время в секундах, в течение которого игрок не будет терять голод.
        /// </summary>
        public float SaturationTime { get; }
        /// <summary>
        /// Идеальная температура для персонажа.
        /// </summary>
        public float PerfectTemperature { get; }
        /// <summary>
        /// Величина по модулю, которая определяет приемлемую мин/макс температуру здорового организма персонажа игрока
        /// </summary>
        public float AbsoluteTemperatureAmplitude { get; }
        /// <summary>
        /// Максимальный объём запаса тепла игрока.
        /// </summary>
        public float MaxWarmLevel { get; }
        /// <summary>
        /// Максимальное здоровье персонажа игрока.
        /// </summary>
        public float MaxHealth { get; }
        /// <summary>
        /// Максимальная величина выносливости персонажа игрока.
        /// </summary>
        public float MaxStamina { get; }
        /// <summary>
        /// Максимальная приемлемая нагрузка для персонажа.
        /// </summary>
        public float MaxLoadCapacity { get; }
        /// <summary>
        /// Время подготовки к броску предмета.
        /// </summary>
        public float ThrowPrepareTime { get; }
    }
}
