using Creatures.Player.Behaviour;
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
            if (Input.GetKey(KeyCode.LeftShift))
                PlayerStateSwitcher.SwitchState<SprintingPlayerState>();

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
        }

        public override void Stop()
        { }
    }
}
