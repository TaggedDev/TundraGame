using Creatures.Player.Behaviour;
using GUI.GameplayGUI;
using UnityEngine;

namespace Creatures.Player.States
{
    public class SprintPlayerState : BasicPlayerState
    {
        private const float speed = 2f;
        private EscapeMenu _escapeCanvas;

        protected override float StarvingConsumptionCoefficient => 2f;

        protected override float StaminaConsumption => 1f;

        protected override float SpeedCoefficient => speed * (PlayerBehaviour.IsOverweight ? 0.5f : 1f);

        protected override float WarmConsumptionCoefficient => 1f;

        public SprintPlayerState(PlayerMovement playerMovement, IPlayerStateSwitcher switcher,

            PlayerProperties playerProperties, PlayerInventory inventory, EscapeMenu escapeCanvas)
            : base(playerMovement, switcher, playerProperties,  inventory, escapeCanvas)
        { }

        public override void Start()
        {

        }

        public override void Stop()
        {

        }

        protected override void StaminaIsOver()
        {
            PlayerBehaviour.SwitchState<WalkPlayerState>();
        }

    }
}
