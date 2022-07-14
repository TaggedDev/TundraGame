using Creatures.Player.Behaviour;
using UnityEngine;

namespace Creatures.Player.States
{
    public class IdlePlayerState : BasicPlayerState
    {
        public IdlePlayerState(PlayerMovement playerMovement, IPlayerStateSwitcher switcher) 
            : base (playerMovement, switcher)
        { }

        private float _h = 0, _v = 0;

        public override void MoveCharacter()
        {
            _h = Input.GetAxis("Horizontal");
            _v = Input.GetAxis("Vertical");

            if (Mathf.Abs(_h) > 0 || Mathf.Abs(_v) > 0)
                PlayerStateSwitcher.SwitchState<WalkingPlayerState>();
        }

        public override void Start()
        {
            //_playerMovement.Animator.SetFloat("Speed", 0f);
        }

        public override void Stop()
        { }
    }
}
