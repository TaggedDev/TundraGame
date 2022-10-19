using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.AI;

namespace Creatures.Mobs.Wolf.States
{
    public class WolfPreparingState : MobBasicState
    {
        private const float ATTACK_DISTANCE_THRESHOLD = 1f;
        private const float MAX_ATTACK_DELAY = 5f; // seconds
        private const float MIN_ATTACK_DELAY = 3f; // seconds

        private Vector3 mobVelocity;
        private Vector3 mobAngularVelocity;
        
        private float attackTimer;

        public WolfPreparingState(Mob mob, IMobStateSwitcher switcher, NavMeshAgent agent) : base(mob, switcher, agent)
        {
            _mob.DeltaRotate = _mob.MaxDeltaRotate;
            _mob.MobRigidbody = _mob.GetComponent<Rigidbody>();
        }

        public override void Start()
        {
            Debug.Log("Preparing state");
            DisableWolfMovement();
            attackTimer = 0f;
        }

        public override void Stop()
        {
            _mob.Agent.isStopped = false;
            _mob.MobRigidbody.angularVelocity = mobAngularVelocity;
            _mob.MobRigidbody.velocity = mobVelocity;
        }

        public override void MoveMob()
        {
            LookAtPosition(_mob.Player.position);
            
            if (Vector3.Distance(_mob.Player.position, _mob.transform.position) > ATTACK_DISTANCE_THRESHOLD)
                _switcher.SwitchState<WolfHuntingState>();

            if (attackTimer <= 0)
            {
                AttackPlayer();
                attackTimer = Random.Range(MIN_ATTACK_DELAY, MAX_ATTACK_DELAY);
            }

            attackTimer -= Time.deltaTime;
            
        }

        private void LookAtPosition(Vector3 spot)
        {
            Vector3 direction = spot - _mob.transform.position;
            direction.y = 0;
            Quaternion rotation = Quaternion.LookRotation(direction);
            _mob.transform.rotation = Quaternion.Lerp(_mob.transform.rotation, rotation,
                Time.deltaTime * _mob.RotationSpeed);
        }
        
        /// <summary>
        /// Disables any movement (angular and regular velocity) of Rigidbody and sets isStopped = true on Agent
        /// and saves these values
        /// </summary>
        private void DisableWolfMovement()
        {
            SaveMovementValues();
            _mob.Agent.isStopped = true;
            _mob.MobRigidbody.angularVelocity = Vector3.zero;
            _mob.MobRigidbody.velocity = Vector3.zero;
        }

        /// <summary>
        /// Saves velocity values in private variables
        /// </summary>
        private void SaveMovementValues()
        {
            mobVelocity = _mob.MobRigidbody.angularVelocity;
            mobAngularVelocity = _mob.MobRigidbody.velocity;
        }
        
        public void AttackPlayer()
        {
            //_mob.MobRigidbody.velocity = Vector3.forward * 2f + new Vector3(0, 10, 0);
        }

        public override void SniffForTarget()
        { }
    }
}