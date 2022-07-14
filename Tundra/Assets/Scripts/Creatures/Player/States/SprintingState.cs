using Creatures.Player.Behaviour;
using UnityEngine;

namespace Creatures.Player.States
{
    public class SprintingState : BasicState
    {
        public SprintingState(PlayerMovement playerMovement, IStateSwitcher switcher)
            : base(playerMovement, switcher)
        { }

        private float _h = 0, _v = 0;
        private Vector3 _rightMovement = Vector3.zero, _forwardMovement = Vector3.zero;

        public override void MoveCharacter()
        {
            if (Input.GetKeyUp(KeyCode.LeftShift))
                StateSwitcher.SwitchState<WalkingState>();

            _h = Input.GetAxis("Horizontal");
            _v = Input.GetAxis("Vertical");
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
            //_playerMovement.Animator.SetFloat("Speed", 1f);
            //PlayerMovement.Speed = 3.5f;
        }

        public override void Stop()
        {
            PlayerMovement.Speed = 3f;
        }
    }
}
