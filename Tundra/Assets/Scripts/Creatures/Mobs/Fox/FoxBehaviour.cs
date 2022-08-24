using System;
using System.Collections.Generic;
using System.Linq;
using Creatures.Mobs.Fox.States;
using Creatures.Player.Behaviour;
using UnityEngine;
using UnityEngine.AI;

namespace Creatures.Mobs.Fox
{
    public class FoxBehaviour : Mob, IMobStateSwitcher
    {
        private const float MAX_SNIFFING_TIME = 1.2f;
        
        private MobBasicState _currentMobState;
        private List<MobBasicState> _allMobStates;

        private float currentSniffingTime;
        private float mobHeight;
        
        private void Start()
        {
            // Define Fear Health Threshold as 10% of max health
            FearHealthThreshold = MaxMobHealth * .1f;
            CurrentMobHealth = MaxMobHealth;
            
            currentSniffingTime = MAX_SNIFFING_TIME;
            mobHeight = GetComponent<Collider>().bounds.extents.y;
            Agent = gameObject.GetComponent<NavMeshAgent>();

            Player = FindObjectOfType<PlayerMovement>().transform;
            
            _allMobStates = new List<MobBasicState>
            {
                new FoxPatrollingState(this, this, Agent),
                new FoxHuntingState(this, this, Agent),
                new FoxEscapingState(this, this, Agent)
            };
            _currentMobState = _allMobStates[0];
        }

        private void FixedUpdate()
        {
            // Temporary solution to kill mob
            if (Input.GetKeyDown(KeyCode.X))
                CurrentMobHealth -= 5f;

            Debug.DrawRay(transform.position, Vector3.down * (mobHeight + 0.2f), Color.blue);
            IsGrounded = Physics.Raycast(transform.position, Vector3.down, mobHeight + 0.2f, 1 << TERRAIN_LAYER_INDEX);
            
            if (IsGrounded)
                MobRigidbody.useGravity = false;
            else
                MobRigidbody.useGravity = true;
                
            _currentMobState.MoveMob();
            
            currentSniffingTime -= Time.fixedDeltaTime;
            if (currentSniffingTime <= 0)
            {
                _currentMobState.SniffForTarget();
                currentSniffingTime = MAX_SNIFFING_TIME;
            }
        }

        public override void Initialise()
        {
            transform.gameObject.layer = MOB_LAYER_INDEX;
            SpawnPosition = transform.position;
        }

        public void SwitchState<T>() where T : MobBasicState
        {
            var state = _allMobStates.FirstOrDefault(s => s is T);
            _currentMobState.Stop();
            state.Start();
            _currentMobState = state;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(targetPoint, .1f);
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(transform.position, SniffingRadius);
        }
    }
}