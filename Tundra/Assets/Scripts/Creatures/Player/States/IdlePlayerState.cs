using Creatures.Player.Behaviour;
using GUI.GameplayGUI;
using UnityEngine;

namespace Creatures.Player.States
{
    public class IdlePlayerState : BasicPlayerState
    {

        public IdlePlayerState(PlayerMovement playerMovement, IPlayerStateSwitcher switcher,
            PlayerProperties playerProperties, PlayerInventory inventory, EscapeMenu escapeCanvas)
            : base(playerMovement, switcher, playerProperties, inventory, escapeCanvas) { }

        private float _h, _v;

        protected override float StarvingConsumptionCoefficient => 10f;

        protected override float StaminaConsumption => -1f;

        protected override float SpeedCoefficient => 0;

        protected override float WarmConsumptionCoefficient => 2f;


        public override void MoveCharacter()
        {
            _h = Input.GetAxis("Horizontal");
            _v = Input.GetAxis("Vertical");

            if (Mathf.Abs(_h) > 0 || Mathf.Abs(_v) > 0)
                PlayerStateSwitcher.SwitchState<WalkPlayerState>();
        }

        public override void Start()
        {
            PlayerMovement.CanSprint = true;
        }

        public override void Stop()
        {

        }

        public override void SpendStamina()
        {
            PlayerProperties.CurrentStaminaPoints -= (StaminaConsumption * Time.deltaTime);
            if (PlayerProperties.CurrentStaminaPoints > PlayerProperties.MaxStaminaPoints) PlayerProperties.CurrentStaminaPoints = PlayerProperties.MaxStaminaPoints;
        }

        protected override void StaminaIsOver()
        { }

    }
}
