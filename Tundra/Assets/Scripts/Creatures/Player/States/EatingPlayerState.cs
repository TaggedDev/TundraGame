using Creatures.Player.Behaviour;
using GUI.GameplayGUI;
using UnityEngine;

namespace Creatures.Player.States
{
    public class EatingPlayerState : BasicPlayerState
    {
        public EatingPlayerState(PlayerMovement playerMovement, IPlayerStateSwitcher switcher,
            PlayerProperties playerProperties,
            PlayerInventory playerInventory, EscapeMenu escapeCanvas) : base(playerMovement, switcher, playerProperties,
            playerInventory, escapeCanvas)
        {
        }

        protected override float StarvingConsumptionCoefficient => 2f;

        protected override float StaminaConsumption => 0f;

        protected override float SpeedCoefficient => 1 * (PlayerBehaviour.IsOverweight ? 0.5f : 1f);

        protected override float WarmConsumptionCoefficient => 2f;


        public override void Start()
        {
            PlayerMovement.CanSprint = false;
            Debug.Log("Entered the eating state");
        }


        public override void Stop()
        {
            PlayerMovement.CanSprint = true;
            Debug.Log("Left the eating state");
        }

        protected override void StaminaIsOver()
        {
        }
    }
}