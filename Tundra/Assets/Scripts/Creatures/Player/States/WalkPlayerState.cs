using Creatures.Player.Behaviour;
using UnityEngine;

namespace Creatures.Player.States
{
    public class WalkPlayerState : BasicPlayerState
    {
        public WalkPlayerState(PlayerMovement playerMovement, IPlayerStateSwitcher switcher, PlayerProperties playerProperties)
            : base(playerMovement, switcher, playerProperties)
        { }

        private float _h = 0, _v = 0;
        private Vector3 _rightMovement = Vector3.zero, _forwardMovement = Vector3.zero;

        public override void MoveCharacter()
        {
            if (Input.GetKey(KeyCode.LeftShift) && !Input.GetMouseButton(2))
            {
                if (PlayerProperties.CurrentStamina > 0) PlayerStateSwitcher.SwitchState<SprintPlayerState>();
            }

            _h = Input.GetAxis("Horizontal");
            _v = Input.GetAxis("Vertical");

            if (_h == 0 && _v == 0)
                PlayerStateSwitcher.SwitchState<IdlePlayerState>();

            _rightMovement = PlayerMovement.Right * (PlayerMovement.Speed * Time.deltaTime * _h);
            _forwardMovement = PlayerMovement.Forward * (PlayerMovement.Speed * Time.deltaTime * _v);

            PlayerMovement.Heading = Vector3.Normalize(_rightMovement + _forwardMovement);
            
            var transform = PlayerMovement.transform;
            transform.forward += PlayerMovement.Heading;
            
            var position = transform.position;
            position += _rightMovement;
            position += _forwardMovement;
            transform.position = position;
        }

        public override void Start()
        {
            //_playerMovement.Animator.SetFloat("Speed", .5f);
            PlayerMovement.Speed = PlayerBehaviour.IsOverweight ? 2f : 3f;
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
