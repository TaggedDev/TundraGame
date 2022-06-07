using AssemblyCSharp.Assets.Scripts.Behaviour;
using AssemblyCSharp.Assets.Scripts.States;
using UnityEngine;

namespace AssemblyCSharp.Assets.Scripts.Player.States
{
    public class IdleState : BasicState
    {
        public IdleState(PlayerMovement playerMovement, IPlayerStateSwitcher switcher) 
            : base (playerMovement, switcher)
        { }

        float h = 0, v = 0;

        public override void MoveCharacter()
        {
            h = Input.GetAxis("Horizontal");
            v = Input.GetAxis("Vertical");

            if (Mathf.Abs(h) > 0 || Mathf.Abs(v) > 0)
                _stateSwitcher.SwitchState<WalkingState>();

            return;
        }

        public override void Start()
        {
            //_playerMovement.Animator.SetFloat("Speed", 0f);
        }

        public override void Stop()
        { }
    }
}
