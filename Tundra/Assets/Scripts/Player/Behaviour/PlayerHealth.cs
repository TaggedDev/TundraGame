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
            throw new NotImplementedException();
        }
    }
}