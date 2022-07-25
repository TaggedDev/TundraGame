using Player.Behaviour;
using UnityEngine;

namespace Player.States
{
    public class IdleState : BasicState
    {
        public IdleState(PlayerMovement playerMovement, IPlayerStateSwitcher switcher) 
            : base (playerMovement, switcher)
        { }

        private float _h = 0, _v = 0;

        public override void MoveCharacter()
        {
            _h = Input.GetAxis("Horizontal");
            _v = Input.GetAxis("Vertical");

            if (Mathf.Abs(_h) > 0 || Mathf.Abs(_v) > 0)
                StateSwitcher.SwitchState<WalkingState>();

            if (StateSwitcher is PlayerBehaviour behaviour)
            {
                if (behaviour._currentStamina < behaviour.MaxStamina)
                {
                    behaviour._currentStamina += (3 * Time.deltaTime);
                }
            }
        }

        public override void Start()
        {
            //_playerMovement.Animator.SetFloat("Speed", 0f);
        }

        public override void Stop()
        { }

        public override void ContinueStarving()
        {
            if (PlayerBehaviour._currentSaturationTime > 0)
            {
                PlayerBehaviour._currentSaturationTime -= Time.deltaTime;
                return;
            }
            PlayerBehaviour._currentStarveCapacity -= 1;
            if (PlayerBehaviour._currentStarveCapacity < 0) PlayerBehaviour._currentStarveCapacity = 0;
        }

        public override void UpdateTemperature()
        {
            //TODO: make temperature logic

            /*
             * Check if current temperature is below the perfect + absolute amplitude
             * If so, start decreasing the temperature of player
             * If player is in comfy place, keep him warm
             * If the current temperature is above the perfect + absolute amplitude - start increasing the temperature
             * If temperature is greater then 'hot' temperature -> burning. Hit player
             */

            //debug
            if (Input.GetKey(KeyCode.T))
            {
                if (Input.GetKey(KeyCode.Equals))
                {
                    PlayerBehaviour._currentTemperature += 0.02f;
                }
                if (Input.GetKey(KeyCode.Minus))
                {
                    PlayerBehaviour._currentTemperature -= 0.02f;
                }
            }
            if (Input.GetKey(KeyCode.H))
            {
                if (Input.GetKey(KeyCode.Equals))
                {
                    PlayerBehaviour._currentHealth += 1f;
                }
                if (Input.GetKey(KeyCode.Minus))
                {
                    PlayerBehaviour._currentHealth -= 1f;
                }
            }
        }
    }
}
