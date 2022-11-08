using Creatures.Player.Behaviour;
using GUI.GameplayGUI;
using UnityEngine;

namespace Creatures.Player.States
{
    public class EatingPlayerState : BasicPlayerState
    {
        public EatingPlayerState(PlayerMovement playerMovement, IPlayerStateSwitcher switcher, PlayerProperties playerProperties,
            PlayerInventory playerInventory, EscapeMenu escapeCanvas) : base(playerMovement, switcher, playerProperties,
            playerInventory, escapeCanvas) { }

        protected override float StarvingConsumptionCoefficient { get; }
        protected override float StaminaConsumption { get; }
        protected override float SpeedCoefficient { get; }
        protected override float WarmConsumptionCoefficient { get; }
        public override void Start()
        { Debug.Log("Entered the eating state"); }

        public override void Stop()
        { Debug.Log("Left the eating state"); }

        protected override void StaminaIsOver()
        { }
    }
}