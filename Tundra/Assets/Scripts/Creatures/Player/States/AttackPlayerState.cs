using Creatures.Player.Behaviour;
using GUI.GameplayGUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Creatures.Player.States
{
    public class AttackPlayerState : BasicPlayerState
    {
        private Animator PlayerAnimator;
        public AttackPlayerState(PlayerMovement playerMovement, IPlayerStateSwitcher switcher, PlayerProperties playerProperties, PlayerInventory playerInventory, EscapeMenu escapeCanvas, Animator playerAnimator) 
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
            //TODO:

            PlayerAnimator.SetTrigger("Windup Right");
            
            //exit on hit/mouseUp
            //fixPlayerPosition
            
            //add hitboxes n' shit...
        }
        //no moving on Release
        //moving on windup resets it
        //thats it
        public override void HandleLMB()
        {
            if (Input.GetMouseButton(0))
            {
                PlayerProperties.CurrentHitProgress += Time.smoothDeltaTime;
            }
            else //NEEDS to be fixed
            {
                PlayerProperties.CurrentHitProgress -= Time.deltaTime;
                if (PlayerProperties.CurrentHitProgress <= 0)
                {
                    PlayerProperties.CurrentHitProgress = 0;
                    PlayerStateSwitcher.SwitchState<IdlePlayerState>();
                }
                    
            }
            if (PlayerProperties.CurrentHitProgress > PlayerProperties.HitPreparationTime)
            {
                PlayerBehaviour.Hit();
                PlayerProperties.CurrentHitProgress = 0;
            }
        }


        public override void Stop()
        {
            
        }

        protected override void StaminaIsOver()
        {
            PlayerStateSwitcher.SwitchState<IdlePlayerState>();
        }
    }
}

