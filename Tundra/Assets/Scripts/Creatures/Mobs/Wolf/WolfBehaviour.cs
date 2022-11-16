using System;
using System.Collections.Generic;
using System.Linq;
using Creatures.Mobs.Wolf.States;
using Creatures.Player.Behaviour;
using GUI.BestiaryGUI;
using UnityEngine;
using UnityEngine.AI;

namespace Creatures.Mobs.Wolf
{
    public class WolfBehaviour : Mob, IMobStateSwitcher
    {
        [SerializeField] private WolfMaw wolfMaw;
        private const float MAX_SNIFFING_TIME = 1.2f;
        private const float MAX_ATTACK_TIME = .175f;
        
        private List<MobBasicState> _allMobStates;
        private MobBasicState _currentMobState;
        private BestiaryPanel _bestiaryPanel;

        private float currentSniffingTime;
        private float lastAttackTimePassed;

        public override void Initialise(MobFabric fabric, Transform player, BestiaryPanel panel)
        {
            if (wolfMaw is null)
                throw new Exception("Wolf maw object wasn't assigned");
            
            wolfMaw.Initialise(player.GetComponent<PlayerProperties>(),
                player.GetComponent<Rigidbody>());

            _bestiaryPanel = panel;
            lastAttackTimePassed = MAX_ATTACK_TIME;
            Player = player;
            Fabric = fabric;
            transform.gameObject.layer = MOB_LAYER_INDEX;
            SpawnPosition = player.position;
        }
        
        private void FixedUpdate()
        {
            // If wolf has low HP, he runs away
            if (CurrentMobHealth <= FearHealthThreshold)
                SwitchState<WolfEscapingState>();
            
            // Temporary solution to kill mob
            if (Input.GetKeyDown(KeyCode.X))
            {
                CurrentMobHealth -= 5f;
                if (CurrentMobHealth <= 0)
                {
                    HandleDeath();
                    return;
                }
            }
            _currentMobState.MoveMob();

            // We cant perform the calculations on wolf maw in preparing state because there is a chance for the wolf
            // to switch states between preparing and hunting states during the attack.
            
            // If the wolf's maw is active -> it was activated to attack the player and we check the time
            // since the attack started
            if (wolfMaw.gameObject.activeSelf)
            {
                lastAttackTimePassed += Time.deltaTime;
            }

            // If enough attack time has passed we disable the maw hitbox and reset the timer
            if (lastAttackTimePassed >= MAX_ATTACK_TIME)
            {
                wolfMaw.gameObject.SetActive(false);
                lastAttackTimePassed = 0;
            }
            
            currentSniffingTime -= Time.fixedDeltaTime;
            if (currentSniffingTime <= 0)
            {
                _currentMobState.SniffForTarget();
                currentSniffingTime = MAX_SNIFFING_TIME;
            }
        }

        /// <summary>
        /// Handles all stuff about death: returning to pool, filling the bestiary 
        /// </summary>
        private void HandleDeath()
        {
            _bestiaryPanel.Mobs[MobIndices.WolfIndex].IsKilled = true;
            Fabric.ReturnToPool(this);
        }

        public override void SpawnSelf(Vector3 position)
        {
            // Define Fear Health Threshold as 10% of max health
            FearHealthThreshold = MaxMobHealth * .1f;
            CurrentMobHealth = MaxMobHealth;
            currentSniffingTime = MAX_SNIFFING_TIME;
            
            SpawnPosition = Player.position;
            transform.position = SpawnPosition;

            Agent = gameObject.GetComponent<NavMeshAgent>();

            transform.position = position;
            gameObject.SetActive(true);
            _allMobStates = new List<MobBasicState>
            {
                new WolfPatrollingState(this, this, Agent),
                new WolfHuntingState(this, this, Agent),
                new WolfEscapingState(this, this, Agent),
                new WolfPreparingState(this, this, Agent, wolfMaw)
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