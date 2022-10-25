﻿using System.Collections.Generic;
using System.Linq;
using Creatures.Mobs.Fox.States;
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
        
        private void FixedUpdate()
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                CurrentMobHealth -= 5f;
                if (CurrentMobHealth <= 0)
                {
                    Fabric.ReturnToPool(this);
                    return;
                }
            }
            
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

        public override void Initialise(MobFabric fabric, Transform player)
        {
            Player = player;
            Fabric = fabric;
            transform.gameObject.layer = MOB_LAYER_INDEX;
        }

        public override void SpawnSelf(Vector3 position)
        {
            FearHealthThreshold = MaxMobHealth * .1f;
            CurrentMobHealth = MaxMobHealth;
            currentSniffingTime = MAX_SNIFFING_TIME;
            
            SpawnPosition = Player.position;
            transform.position = SpawnPosition;
            
            
            mobHeight = GetComponent<Collider>().bounds.extents.y;
            Agent = gameObject.GetComponent<NavMeshAgent>();
            
            gameObject.SetActive(true);

            _allMobStates = new List<MobBasicState>
            {
                new FoxPatrollingState(this, this, Agent),
                new FoxHuntingState(this, this, Agent),
                new FoxEscapingState(this, this, Agent)
            };
            _currentMobState = _allMobStates[0];
            
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