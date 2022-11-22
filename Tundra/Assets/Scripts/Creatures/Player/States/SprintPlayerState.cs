using Creatures.Player.Behaviour;
using GUI.BestiaryGUI;
using GUI.GameplayGUI;

namespace Creatures.Player.States
{
    public class SprintPlayerState : BasicPlayerState
    {
        private const float speed = 2f;

        protected override float StarvingConsumptionCoefficient => 2f;

        protected override float StaminaConsumption => 1f;

        protected override float SpeedCoefficient => speed * (PlayerBehaviour.IsOverweight ? 0.5f : 1f);

        protected override float WarmConsumptionCoefficient => 1f;

        public SprintPlayerState(PlayerMovement playerMovement, IPlayerStateSwitcher switcher,
            PlayerProperties playerProperties, PlayerInventory inventory, EscapeMenu escapeCanvas,
            BestiaryPanel bestiaryPanel)
            : base(playerMovement, switcher, playerProperties, inventory, escapeCanvas, bestiaryPanel)
        { }

        public override void Start()
        {
            PlayerAnimation.SwitchAnimation("Run");
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
