using System;
using UnityEngine;

namespace Player.Behaviour
{
    public class PlayerStarvation : MonoBehaviour
    {
        // Properties
        public float MaxStarve => maxStarve;
        public float CurrentStarveCapacity => currentStarveCapacity;

        // Fields
        [SerializeField] private float maxStarve;
        [SerializeField] private float saturationTime;
        
        // Variables
        private float currentStarveCapacity;
        private float currentSaturationTime;
        
        // Public methods

        // Private methods
        private void Start()
        {
            currentStarveCapacity = maxStarve;
            currentSaturationTime = saturationTime;
        }

        private void Update()
        {
            ContinueStarving();
        }

        private void ContinueStarving()
        {
            if (currentSaturationTime > 0)
            {
                currentSaturationTime -= Time.deltaTime;
                return;
            }
            currentStarveCapacity -= 1;
            if (currentStarveCapacity < 0) currentStarveCapacity = 0;
        }

        private void ConsumeFood(float value)
        {
            if (currentStarveCapacity + value <= MaxStarve)
                currentStarveCapacity += value;
            else
                currentStarveCapacity = MaxStarve;
        }
    }
}