using Creatures.Player.Behaviour;
using UnityEngine;

namespace Creatures.Player.States
{
    public class IdleState : BasicState
    {
        public IdleState(PlayerMovement playerMovement, IStateSwitcher switcher) 
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
    }
}
