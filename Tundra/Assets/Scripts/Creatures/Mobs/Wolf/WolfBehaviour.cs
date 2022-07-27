using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Creatures.Mobs.Wolf
{
    public class WolfBehaviour : Mob, IMobStateSwitcher
    {
        private const float _maxSniffingTime = 1.2f;
        
        private MobBasicState _currentMobState;
        private List<MobBasicState> _allMobStates;

        private float currentSniffingTime;
        
        private void Start()
        {
            currentSniffingTime = _maxSniffingTime;
            _allMobStates = new List<MobBasicState>
            {
                new WolfPatrollingState(this, this),
                new WolfHuntingState(this, this)
            };
            _currentMobState = _allMobStates[0];
        }

        private void FixedUpdate()
        {
            _currentMobState.MoveMob();
            
            currentSniffingTime -= Time.fixedDeltaTime;
            if (currentSniffingTime <= 0)
            {
                _currentMobState.SniffForTarget();
                currentSniffingTime = _maxSniffingTime;
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