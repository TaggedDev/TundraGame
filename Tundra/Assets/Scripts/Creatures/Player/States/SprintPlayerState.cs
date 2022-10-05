using Creatures.Player.Behaviour;
using UnityEngine;

namespace Creatures.Player.States
{
    public class SprintPlayerState : BasicPlayerState
    {
        private const float speed = 2f;

        protected override float StarvingConsumptionCoefficient => 2f;

        protected override float StaminaConsumption => 1f;

        protected override float SpeedCoefficient => speed * (PlayerBehaviour.IsOverweight ? 0.5f : 1f);

        protected override float WarmConsumptionCoefficient => 1f;

        public SprintPlayerState(PlayerMovement playerMovement, IPlayerStateSwitcher switcher, PlayerProperties playerProperties)
            : base(playerMovement, switcher, playerProperties)
        { }

        public override void Start()
        {
            //Debug.Log("Got sprint State");
        }

        public override void Stop()
        {
            //Debug.Log("Lost sprint State");
        }

        public override void LoadForThrow()
        {
            PlayerBehaviour.SwitchState<WalkPlayerState>();
        }

        protected override void StaminaIsOver()
        {
            PlayerBehaviour.SwitchState<WalkPlayerState>();
        }
    }
}
