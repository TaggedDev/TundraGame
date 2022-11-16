using Creatures.Player.Behaviour;
using Creatures.Player.Inventory;
using GUI.GameplayGUI;
using JetBrains.Annotations;
using UnityEngine;


namespace Creatures.Player.States
{
    public class WindupHitPlayerState : BasicPlayerState
    {
        private Animator PlayerAnimator;
        public WindupHitPlayerState(PlayerMovement playerMovement, IPlayerStateSwitcher switcher, PlayerProperties playerProperties, PlayerInventory playerInventory, EscapeMenu escapeCanvas, Animator playerAnimator) 
            : base(playerMovement, switcher, playerProperties, playerInventory, escapeCanvas)
        {
            PlayerAnimator = playerAnimator;
        }

        protected override float StarvingConsumptionCoefficient => 1.2f;

        protected override float StaminaConsumption => 5f;

        protected override float SpeedCoefficient => 0f;

        protected override float WarmConsumptionCoefficient => 0f;

        public override void Start()
        {
            PlayerAnimator.speed = 0.867f / (PlayerInventory.SelectedItem as MeleeWeaponConfiguration).FullWindupTime;
            PlayerAnimator.Play("Windup Right");
            //PlayerAnimator.SetTrigger("Windup");
        }


        public override void HandleUserInput()
        {
            if (Input.GetMouseButton(0))
            {
                
                PlayerProperties.CurrentHitProgress += Time.smoothDeltaTime;
                if (PlayerProperties.CurrentHitProgress >= (PlayerInventory.SelectedItem as MeleeWeaponConfiguration).FullWindupTime)
                {
                    PlayerBehaviour.Hit();
                    PlayerStateSwitcher.SwitchState<IdlePlayerState>();
                    PlayerProperties.CurrentHitProgress = 0;
                }
            }
            else
            {
                if(PlayerProperties.CurrentHitProgress != 0)
                    PlayerBehaviour.Hit();
                PlayerStateSwitcher.SwitchState<IdlePlayerState>();
            }
            
        }

        public override void MoveCharacter()
        {

        }

        public override void Stop()
        {
            PlayerProperties.CurrentHitProgress = 0;
            PlayerAnimator.speed = 1;
        }

        protected override void StaminaIsOver()
        {
            //PlayerStateSwitcher.SwitchState<IdlePlayerState>();
        }
    }
}

