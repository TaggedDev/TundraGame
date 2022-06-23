using System;
using UnityEngine;

namespace Player.Behaviour
{
    public class PlayerTemperature : MonoBehaviour
    {
        // Properties
        public float PerfectTemperature => perfectTemperature;
        public float CurrentTemperature => currentTemperature;

        // Fields
        [SerializeField] private float perfectTemperature;
        [SerializeField] private float absoluteTemperatureAmplitude;
        [SerializeField] private float hotTemperature;

        // Variables
        private float currentTemperature;
        
        // Public methods

        // Private methods
        private void Start()
        {
            currentTemperature = PerfectTemperature;
        }

        private void Update()
        {
            /*
             * Check if current temperature is below the perfect + absolute amplitude
             * If so, start decreasing the temperature of player
             * If player is in comfy place, keep him warm
             * If the current temperature is above the perfect + absolute amplitude - start increasing the temperature
             * If temperature is greater then 'hot' temperature -> burning. Hit player
             */

            throw new NotImplementedException();
        }
    }
}