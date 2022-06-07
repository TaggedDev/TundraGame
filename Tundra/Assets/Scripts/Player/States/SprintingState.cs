using AssemblyCSharp.Assets.Scripts.Behaviour;
using AssemblyCSharp.Assets.Scripts.States;
using UnityEngine;

namespace AssemblyCSharp.Assets.Scripts.Player.States
{
    public class SprintingState : BasicState
    {
        public SprintingState(PlayerMovement playerMovement, IPlayerStateSwitcher switcher)
            : base(playerMovement, switcher)
        { }

        float h = 0, v = 0;
        Vector3 rightMovement = Vector3.zero, forwardMovement = Vector3.zero;

        public override void MoveCharacter()
        {
            if (Input.GetKeyUp(KeyCode.LeftShift))
                _stateSwitcher.SwitchState<WalkingState>();

            h = Input.GetAxis("Horizontal");
            v = Input.GetAxis("Vertical");
            rightMovement = _playerMovement.Right * _playerMovement.Speed * Time.deltaTime * h;
            forwardMovement = _playerMovement.Forward * _playerMovement.Speed * Time.deltaTime * v;

            _playerMovement.Heading = Vector3.Normalize(rightMovement + forwardMovement);
            _playerMovement.transform.forward += _playerMovement.Heading;
            _playerMovement.transform.position += rightMovement;
            _playerMovement.transform.position += forwardMovement;
        }

        public override void Start()
        {
            //_playerMovement.Animator.SetFloat("Speed", 1f);
            _playerMovement.Speed = 3.5f;
        }

        public override void Stop()
        {
            _playerMovement.Speed = 3f;
        }
    }
}
