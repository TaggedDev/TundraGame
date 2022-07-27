using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Creatures.Mobs.Wolf
{
    public class WolfBehaviour : Mob, IMobStateSwitcher
    {
        private const float MAX_SNIFFING_TIME = 1.2f;
        
        private MobBasicState _currentMobState;
        private List<MobBasicState> _allMobStates;

        private float currentSniffingTime;
        
        private void Start()
        {
            // Define Fear Health Threshold as 10% of max health
            FearHealthThreshold = MaxMobHealth * .1f;
            CurrentMobHealth = MaxMobHealth;
            
            currentSniffingTime = MAX_SNIFFING_TIME;
            
            _allMobStates = new List<MobBasicState>
            {
                new WolfPatrollingState(this, this),
                new WolfHuntingState(this, this),
                new WolfEscapingState(this, this)
            };
            _currentMobState = _allMobStates[0];
            
        }

        private void FixedUpdate()
        {
            if (Input.GetKeyDown(KeyCode.X))
                CurrentMobHealth -= 5f;

            _currentMobState.MoveMob();
            
            currentSniffingTime -= Time.fixedDeltaTime;
            if (currentSniffingTime <= 0)
            {
                _currentMobState.SniffForTarget();
                currentSniffingTime = MAX_SNIFFING_TIME;
            }
        }
        
        public void SwitchState<T>() where T : MobBasicState
        {
            var state = _allMobStates.FirstOrDefault(s => s is T);
            _currentMobState.Stop();
            state.Start();
            _currentMobState = state;
        }

        public override void Initialise()
        {
            transform.gameObject.layer = MOB_LAYER_INDEX;
            SpawnPosition = transform.position;
        }
        
        private void OnDrawGizmosSelected()
        {
            // Draw a yellow sphere at the transform's position
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(Sensor.TargetPosition, 1f);
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(transform.position, SniffingRadius);
        }
    }
}