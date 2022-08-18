using Creatures.Player.Behaviour;
using UnityEngine;

namespace Creatures.Player.States
{
    public class IdlePlayerState : BasicPlayerState
    {
        public IdlePlayerState(PlayerMovement playerMovement, IPlayerStateSwitcher switcher, PlayerProperties playerProperties) 
            : base (playerMovement, switcher, playerProperties)
        { }

        private float _h = 0, _v = 0;

        public override void MoveCharacter()
        {
            _h = Input.GetAxis("Horizontal");
            _v = Input.GetAxis("Vertical");

            if (Mathf.Abs(_h) > 0 || Mathf.Abs(_v) > 0)
                PlayerStateSwitcher.SwitchState<WalkPlayerState>();

            if (PlayerStateSwitcher is PlayerBehaviour behaviour)
            {
                if (PlayerProperties.CurrentStamina < PlayerProperties.MaxStamina)
                {
                    PlayerProperties.CurrentStamina += (3 * Time.deltaTime);
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
            if (PlayerProperties._currentStarvationTime > 0)
            {
                PlayerProperties._currentStarvationTime -= Time.deltaTime;
                return;
            }
            PlayerProperties.CurrentStarvationCapacity -= 1;
            if (PlayerProperties.CurrentStarvationCapacity < 0) PlayerProperties.CurrentStarvationCapacity = 0;
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
                    PlayerProperties.CurrentWarmLevel += 0.02f;
                }
                if (Input.GetKey(KeyCode.Minus))
                {
                    PlayerProperties.CurrentWarmLevel -= 0.02f;
                }
            }
            if (Input.GetKey(KeyCode.H))
            {
                if (Input.GetKey(KeyCode.Equals))
                {
                    PlayerProperties.CurrentHealth += 1f;
                }
                if (Input.GetKey(KeyCode.Minus))
                {
                    PlayerProperties.CurrentHealth -= 1f;
                }
            }
        }

        public override void LoadForThrow()
        {
            if (Input.GetMouseButton(2))
            {
                PlayerProperties._throwLoadingProgress -= Time.deltaTime;
                if (PlayerProperties._throwLoadingProgress <= 0) PlayerProperties._throwLoadingProgress = 0;
            }
            else
            {
                if (PlayerProperties._throwLoadingProgress <= 0) PlayerBehaviour.ThrowItem();
                PlayerProperties._throwLoadingProgress = PlayerProperties.ThrowPrepareTime;
            }
        }
    }
}
