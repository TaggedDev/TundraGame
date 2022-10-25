using UnityEngine;
using UnityEngine.AI;

namespace Creatures.Mobs.Wolf.States
{
    public class WolfPreparingState : MobBasicState
    {
        private const float ATTACK_DISTANCE_THRESHOLD = 1.6f;
        private const float MAX_ATTACK_DELAY = 5f; // seconds
        private const float MIN_ATTACK_DELAY = 3f; // seconds

        private readonly WolfMaw _maw;
        
        private float attackTimer;

        public WolfPreparingState(Mob mob, IMobStateSwitcher switcher, NavMeshAgent agent, WolfMaw maw) : base(mob, switcher, agent)
        {
            _mob.DeltaRotate = _mob.MaxDeltaRotate;
            _mob.MobRigidbody = _mob.GetComponent<Rigidbody>();
            _maw = maw;
        }

        public override void Start()
        {
            Debug.Log("Preparing state");
            DisableWolfMovement();
            _mob.Agent.isStopped = true;
            attackTimer = 0f;
        }

        public override void Stop()
        {
            _mob.Agent.isStopped = false;
        }

        public override void MoveMob()
        {
            _mob.LookAtPosition(_mob.Player.position);
            float distance = Vector3.Distance(_mob.Player.position, _mob.transform.position);
            
            // If player has ran too far from the wolf, it chases the player
            if (distance > ATTACK_DISTANCE_THRESHOLD)
            {
                _switcher.SwitchState<WolfHuntingState>();
                return;
            }

            // If player approaches the wolf, he attacks immediately
            if (distance < ATTACK_DISTANCE_THRESHOLD / 2 && attackTimer <= 0)
            {
                Debug.Log("Jumping!");
                //_mob.MobRigidbody.AddForce((_mob.transform.forward + Vector3.up * 1000f) * (50 * _mob.MobRigidbody.mass));
                _mob.MobRigidbody.AddForce(_mob.transform.forward.x * 50, 1000f, _mob.transform.forward.z * 50, ForceMode.Impulse);
                _maw.gameObject.SetActive(true);
                attackTimer = Random.Range(MIN_ATTACK_DELAY, MAX_ATTACK_DELAY);
            }

            if (attackTimer > 0)
            {
                attackTimer -= Time.deltaTime;
                
            }
        }

        /// <summary>
        /// Disables any movement (angular and regular velocity) of Rigidbody and sets isStopped = true on Agent
        /// and saves these values
        /// </summary>
        private void DisableWolfMovement()
        {
            _mob.Agent.isStopped = true;
            _mob.Agent.SetDestination(_mob.transform.position);
            _mob.MobRigidbody.angularVelocity = Vector3.zero;
            _mob.MobRigidbody.velocity = Vector3.zero;
        }
        
        public void AttackPlayer()
        {
            //_mob.MobRigidbody.velocity = Vector3.forward * 2f + new Vector3(0, 10, 0);
        }

        public override void SniffForTarget()
        { }
    }
}