﻿using Creatures.Player.Behaviour;
using UnityEngine;

namespace Creatures.Player.States
{
    public class WalkingPlayerState : BasicPlayerState
    {
        public WalkingPlayerState(PlayerMovement playerMovement, IPlayerStateSwitcher switcher)
            : base(playerMovement, switcher)
        { }

        private float _h = 0, _v = 0;
        private Vector3 _rightMovement = Vector3.zero, _forwardMovement = Vector3.zero;

        public override void MoveCharacter()
        {
            if (Input.GetKey(KeyCode.LeftShift) && !Input.GetMouseButton(2))
            {
                if (PlayerStateSwitcher is PlayerBehaviour behaviour)
                {
                    if (behaviour._currentStamina > 0) PlayerStateSwitcher.SwitchState<SprintingPlayerState>();
                }
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
        }

        public override void LoadForThrow()
        {
            if (Input.GetMouseButton(2))
            {
                PlayerBehaviour._throwLoadingProgress -= Time.deltaTime;
                if (PlayerBehaviour._throwLoadingProgress <= 0) PlayerBehaviour._throwLoadingProgress = 0;
            }
            else
            {
                if (PlayerBehaviour._throwLoadingProgress <= 0) PlayerBehaviour.ThrowItem();
                PlayerBehaviour._throwLoadingProgress = PlayerBehaviour.ThrowPrepareTime;
            }
        }
    }
}