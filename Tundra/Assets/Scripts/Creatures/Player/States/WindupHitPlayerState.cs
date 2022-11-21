using Creatures.Player.Behaviour;
using Creatures.Player.Inventory;
using GUI.GameplayGUI;
using JetBrains.Annotations;
using System.Collections;
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
            PlayerAnimator.speed = (PlayerInventory.SelectedItem as MeleeWeaponConfiguration).AnimationClipSpeed;
            PlayerAnimator.CrossFade((PlayerInventory.SelectedItem as MeleeWeaponConfiguration).AnimationWeaponName + " Windup", .1f);
        }

        /// <summary>
        /// Overrides basic player input <br/>
        /// Only reads MouseButton0
        /// </summary>
        public override void HandleUserInput()
        {

            if (Input.GetMouseButton(0))
            {
                
                PlayerProperties.CurrentHitProgress += Time.smoothDeltaTime;
                if (PlayerProperties.CurrentHitProgress > (PlayerInventory.SelectedItem as MeleeWeaponConfiguration).FullWindupTime)
                {
                    PlayerProperties.CurrentHitProgress = (PlayerInventory.SelectedItem as MeleeWeaponConfiguration).FullWindupTime;
                }
            }
            if (Input.GetMouseButtonUp(0))
            {
                PlayerAnimator.speed = 1;
                PlayerBehaviour.StartCoroutine(CoolDown((PlayerInventory.SelectedItem as MeleeWeaponConfiguration).ReleaseAnimationLenght));
            }
            
        }
        /// <summary>
        /// Blocks all movement
        /// </summary>
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
            
        }
        /// <summary>
        /// Starts a hit cooldown so animation could play properly <br/>
        /// And to avoid spamming
        /// </summary>
        /// <param name="delayTime"> Hit/CoolDown Duration in seconds </param> 
        private IEnumerator CoolDown(float delayTime)
        {
            
            PlayerAnimator.Play((PlayerInventory.SelectedItem as MeleeWeaponConfiguration).AnimationWeaponName + " Release");
            yield return new WaitForSeconds(delayTime);
            if (PlayerProperties.CurrentHitProgress != 0)
                PlayerBehaviour.Hit();
            yield return null;
            PlayerProperties.CurrentHitProgress = 0;
            PlayerStateSwitcher.SwitchState<IdlePlayerState>();
            yield return null;
            yield break;

        }
    }
}

