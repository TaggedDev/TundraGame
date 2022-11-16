using Creatures.Player.Behaviour;
using System;
using GUI.BestiaryGUI;
using GUI.GameplayGUI;

namespace Creatures.Player.States
{
    public class BusyPlayerState : BasicPlayerState
    {

        private EscapeMenu _escapeCanvas;

        protected override float StarvingConsumptionCoefficient => throw new NotImplementedException();

        protected override float StaminaConsumption => throw new NotImplementedException();

        protected override float SpeedCoefficient => throw new NotImplementedException();

        protected override float WarmConsumptionCoefficient => throw new NotImplementedException();

        public BusyPlayerState(PlayerMovement playerMovement, IPlayerStateSwitcher switcher,

            PlayerProperties playerProperties, PlayerInventory inventory, EscapeMenu escapeCanvas, BestiaryPanel bestiaryPanel)
            : base(playerMovement, switcher, playerProperties, inventory, escapeCanvas, bestiaryPanel)
        {
        }

        public override void HandleEscapeButton()
        {
            throw new NotImplementedException();
        }

        public override void MoveCharacter()
        {
            //Here's nothing. Player Should'nt move in this state.
        }

        public override void ContinueFreeze()
        {
            //I guess it's like a pause, player should not spend temperature on it
        }

        public override void ContinueStarving()
        {
            //And so I can say for starving.
        }

        public override void LoadForThrow()
        {
            //Player cannot throw items while he's in inventory.
        }

        public override void SpendStamina()
        {
            //Player doesn't spend stamina in inventory.
        }

        public override void Start()
        {
        }

        public override void Stop()
        {
        }

        protected override void StaminaIsOver()
        {
        }

    }
}
