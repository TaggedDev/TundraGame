using Creatures.Player.Behaviour;
using Creatures.Player.Inventory.ItemConfiguration;
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

        public override void HandleUserInput()
        {
            Debug.Log($"{PlayerProperties.FoodConsumingTimeLeft}, {PlayerProperties.IsHoldingFood}");
            if (Input.GetMouseButton(0))
            {
                // Check if we are trying to eat something that is not food
                if (!PlayerProperties.IsHoldingFood)
                    PlayerProperties.IsHoldingFood = CheckWhetherSelectedItemIsFood();

                if (PlayerProperties.IsHoldingFood)
                {
                    PlayerAnimation.SwitchAnimation("Eat");
                    PlayerProperties.FoodConsumingTimeLeft -= Time.deltaTime;
                    if (PlayerProperties.FoodConsumingTimeLeft <= 0)
                        ConsumeCurrentFood();    
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                PlayerProperties.FoodConsumingTimeLeft = PlayerProperties.FOOD_CONSUMING_MAX_TIME;
                PlayerAnimation.SwitchAnimation("Idle");
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
