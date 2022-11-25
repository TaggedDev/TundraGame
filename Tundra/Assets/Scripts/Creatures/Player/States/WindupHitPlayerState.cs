using Creatures.Player.Behaviour;
using Creatures.Player.Inventory;
using GUI.GameplayGUI;
using Creatures.Player.Inventory.ItemConfiguration;
using System.Collections;
using UnityEngine;
using GUI.BestiaryGUI;

namespace Creatures.Player.States
{
    public class WindupHitPlayerState : BasicPlayerState
    {
        private bool IsOnCoolDown;
        public WindupHitPlayerState(PlayerMovement playerMovement, IPlayerStateSwitcher switcher, PlayerProperties playerProperties, PlayerInventory playerInventory, EscapeMenu escapeCanvas, 
             BestiaryPanel bestiaryPanel) 
            : base(playerMovement, switcher, playerProperties, playerInventory, escapeCanvas, bestiaryPanel)
        {

        }

        protected override float StarvingConsumptionCoefficient => 12f;

        protected override float StaminaConsumption => 2f;

        protected override float SpeedCoefficient => 0f;

        protected override float WarmConsumptionCoefficient => 0.5f;

        public override void Start()
        {
            IsOnCoolDown = false;
            PlayerAnimation.SwitchAnimation((PlayerInventory.SelectedItem as MeleeWeaponConfiguration).AnimationWeaponName + " Windup", (PlayerInventory.SelectedItem as MeleeWeaponConfiguration).AnimationClipSpeed);
            PlayerProperties.MaxCircleBarFillingTime = (PlayerInventory.SelectedItem as MeleeWeaponConfiguration).FullWindupTime;
            
        }

        /// <summary>
        /// Overrides basic player input <br/>
        /// Only reads MouseButton0
        /// </summary>
        public override void HandleUserInput()
        {
            if (IsOnCoolDown)
                return;

            if (Input.GetMouseButton(0))
            {
                
                PlayerProperties.CurrentCircleBarFillingTime += Time.smoothDeltaTime;
                if (PlayerProperties.CurrentCircleBarFillingTime > (PlayerInventory.SelectedItem as MeleeWeaponConfiguration).FullWindupTime)
                {
                    PlayerProperties.CurrentCircleBarFillingTime = (PlayerInventory.SelectedItem as MeleeWeaponConfiguration).FullWindupTime;
                }
            }
            if (Input.GetMouseButtonUp(0))
            {
                PlayerBehaviour.StartCoroutine(CoolDown((PlayerInventory.SelectedItem as MeleeWeaponConfiguration).ReleaseAnimationLenght));
            }
            
        }
        /// <summary>
        /// Blocks all movement
        /// </summary>
        public override void MoveCharacter()
        {

        }

        protected override void InventorySelectedSlotChanged(object sender, int e)
        {
            PlayerStateSwitcher.SwitchState<IdlePlayerState>();
        }

        public override void Stop()
        {
            PlayerProperties.CurrentCircleBarFillingTime = 0;
        }

        protected override void StaminaIsOver()
        {
            PlayerStateSwitcher.SwitchState<IdlePlayerState>();
        }
        /// <summary>
        /// Starts a hit cooldown so animation could play properly <br/>
        /// And to avoid spamming
        /// </summary>
        /// <param name="delayTime"> Hit/CoolDown Duration in seconds </param> 
        private IEnumerator CoolDown(float delayTime)
        {
            IsOnCoolDown = true;
            PlayerAnimation.SwitchAnimation((PlayerInventory.SelectedItem as MeleeWeaponConfiguration).AnimationWeaponName + " Release");
            yield return new WaitForSeconds(delayTime);
            if (PlayerProperties.CurrentCircleBarFillingTime != 0)
                PlayerBehaviour.Hit();
            yield return null;
            PlayerProperties.CurrentCircleBarFillingTime = 0;
            PlayerStateSwitcher.SwitchState<IdlePlayerState>();
            yield return null;
            yield break;

        }
    }
}

