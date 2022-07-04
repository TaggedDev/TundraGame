using System;
using UnityEngine;

namespace Player.Behaviour
{
    public class PlayerHealth : MonoBehaviour
    {
        // Properties
        public float MaxHealth
        {
            get => _maxHealth;
            set => _maxHealth = value;
        }
        
        public float CurrentHealth => currentHealth;
        // Fields
        [SerializeField] private float _maxHealth;
        // Variables
        private float currentHealth;

        
        
        // Public methods

        // Private methods
        private void Start()
        {
            currentHealth = _maxHealth;
        }

        private void Update()
        {
            //Debug.Log($"Current hp: {currentHealth}");
            if (Input.GetKey(KeyCode.H))
            {
                if (Input.GetKeyDown(KeyCode.Minus)) currentHealth -= 5;
                if (Input.GetKeyDown(KeyCode.Equals)) currentHealth += 5;
            }
        }
    }
}