using Creatures.Player.Behaviour;
using GUI.BestiaryGUI;
using Creatures.Player.Inventory.ItemConfiguration;
using GUI.GameplayGUI;
using UnityEngine;

namespace Creatures.Player.States
{
    public class IdlePlayerState : BasicPlayerState
    {
        public IdlePlayerState(PlayerMovement playerMovement, IPlayerStateSwitcher switcher,
            PlayerProperties playerProperties, PlayerInventory inventory, EscapeMenu escapeCanvas,
            BestiaryPanel bestiaryPanel)
            : base(playerMovement, switcher, playerProperties, inventory, escapeCanvas, bestiaryPanel)
        {
            
        }

        private float _h, _v;

        protected override float StarvingConsumptionCoefficient => 10f;

        protected override float StaminaConsumption => -1f;

        protected override float SpeedCoefficient => 0;

        protected override float WarmConsumptionCoefficient => 2f;

        public override void HandleUserInput()
        {
            base.HandleUserInput();
            if (Input.GetMouseButton(0) && !(PlayerInventory.SelectedItem is MeleeWeaponConfiguration))
            {

                
                // Check if we are trying to eat something that is not food
                if (!PlayerProperties.IsHoldingFood)
                    PlayerProperties.IsHoldingFood = CheckWhetherSelectedItemIsFood();

                if (PlayerProperties.IsHoldingFood)
                {
                    PlayerProperties.MaxCircleBarFillingTime = PlayerProperties.MaxCircleFillingTime_EATING;
                    PlayerAnimation.SwitchAnimation("Eat");
                    PlayerProperties.FoodConsumingTimeLeft -= Time.deltaTime;
                    PlayerProperties.CurrentCircleBarFillingTime += Time.deltaTime;
                    if (PlayerProperties.FoodConsumingTimeLeft <= 0)
                    {
                        ConsumeCurrentFood();
                        PlayerProperties.CurrentCircleBarFillingTime = 0;
                        PlayerProperties.FoodConsumingTimeLeft = PlayerProperties.FOOD_CONSUMING_MAX_TIME;
                        PlayerAnimation.SwitchAnimation("Idle");
                        PlayerAnimation.SwitchAnimation("Not eating");
                    }
                }
                else
                {
                    PlayerProperties.MaxCircleBarFillingTime = PlayerProperties.MaxCircleFillingTime_ATTACK;
                }
            }

            if (Input.GetMouseButtonUp(0) && !(PlayerInventory.SelectedItem is MeleeWeaponConfiguration))
            {
                PlayerProperties.FoodConsumingTimeLeft = PlayerProperties.FOOD_CONSUMING_MAX_TIME;
                PlayerAnimation.SwitchAnimation("Not eating");
            }
            
            if (PlayerEquipment.Book != null && Input.GetKeyDown(KeyCode.X))
            {
                PlayerStateSwitcher.SwitchState<MagicCastingPlayerState>();
            }
            
        }

        public override void MoveCharacter()
        {
            _h = Input.GetAxis("Horizontal");
            _v = Input.GetAxis("Vertical");

            if (Mathf.Abs(_h) > 0 || Mathf.Abs(_v) > 0)
                PlayerStateSwitcher.SwitchState<WalkPlayerState>();
        }

        public override void Start()
        {
            PlayerProperties.CurrentCircleBarFillingTime = 0f;
            PlayerMovement.CanSprint = true;
            PlayerAnimation.SwitchAnimation("Idle");
            
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

        /// <summary>
        /// Checks whether the selected inventory item is a type of food
        /// </summary>
        /// <returns>True if this is a food item</returns>
        private bool CheckWhetherSelectedItemIsFood()
        {
            return PlayerInventory.SelectedItem is FoodItemConfiguration;
        }
    }
}
