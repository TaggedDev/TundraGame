using System.Collections.Generic;
using System.Linq;
using Creatures.Player.Behaviour;
using UnityEngine;

namespace Creatures.Mobs.Wolf
{
    public class WolfBehaviour : Mob, IMobStateSwitcher
    {
        private const float _maxSniffingTime = 2f;

        public Transform player;
        private MobBasicState _currentMobState;
        private List<MobBasicState> _allMobStates;

        private float currentSniffingTime;
        
        private void Start()
        {
            player = FindObjectOfType<PlayerMovement>().transform;
            currentSniffingTime = _maxSniffingTime;
            _allMobStates = new List<MobBasicState>
            {
                new WolfPatrollingState(this, this),
                new WolfHuntingState(this, this, player)
            };
            _currentMobState = _allMobStates[0];
        }

        private void FixedUpdate()
        {
            _currentMobState.MoveMob();
            
            currentSniffingTime -= Time.fixedDeltaTime;
            if (currentSniffingTime <= _maxSniffingTime)
                _currentMobState.SniffForTarget();
        }
        
        public void SwitchState<T>() where T : MobBasicState
        {
            var state = _allMobStates.FirstOrDefault(s => s is T);
            _currentMobState = state;
        }

        public override void Initialise(Transform playerParameter)
        {
            transform.gameObject.layer = MOB_LAYER_INDEX;
            SpawnPosition = transform.position;
        }

        private void OnDrawGizmosSelected()
        {
            // Draw a yellow sphere at the transform's position
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(_currentMobState._targetPosition, 1f);
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(transform.position, SniffingRadius);
        }
    }
}