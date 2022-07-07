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
                if (Input.GetKeyDown(KeyCode.Equals))
                {
                    PlayerBehaviour._currentTemperature += 0.1f;
                }
                if (Input.GetKeyDown(KeyCode.Minus))
                {
                    PlayerBehaviour._currentTemperature -= 0.1f;
                }
            }
        }
    }
}
