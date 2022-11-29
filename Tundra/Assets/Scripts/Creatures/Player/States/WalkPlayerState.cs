using Creatures.Player.Behaviour;
using Creatures.Player.Inventory.ItemConfiguration;
using GUI.BestiaryGUI;
using GUI.GameplayGUI;
using UnityEngine;

namespace Creatures.Player.States
{
    public class WalkPlayerState : BasicPlayerState
    {

        private const float speed = 1f;

        protected override float StarvingConsumptionCoefficient => 2f;

        protected override float StaminaConsumption => 0f;

        protected override float SpeedCoefficient => speed * (PlayerBehaviour.IsOverweight ? 0.5f : 1f);

        protected override float WarmConsumptionCoefficient => 2f;

        public WalkPlayerState(PlayerMovement playerMovement, IPlayerStateSwitcher switcher,

            PlayerProperties playerProperties, PlayerInventory inventory, EscapeMenu escapeCanvas, BestiaryPanel bestiaryPanel)
            : base(playerMovement, switcher, playerProperties, inventory, escapeCanvas, bestiaryPanel)
        {
            if (Input.GetMouseButton(0))
            {
                Debug.Log(PlayerProperties.MaxCircleBarFillingTime);
                // Check if we are trying to eat something that is not food
                if (!PlayerProperties.IsHoldingFood)
                    PlayerProperties.IsHoldingFood = CheckWhetherSelectedItemIsFood();

                if (PlayerProperties.IsHoldingFood)
                {
                    PlayerProperties.MaxCircleBarFillingTime = PlayerProperties.MaxCircleFillingTime_EATING;
                    PlayerAnimation.SwitchAnimation("Eat");
                    PlayerProperties.FoodConsumingTimeLeft -= Time.deltaTime;
                    if (PlayerProperties.FoodConsumingTimeLeft <= 0)
                    {
                        ConsumeCurrentFood();
                        PlayerProperties.FoodConsumingTimeLeft = PlayerProperties.FOOD_CONSUMING_MAX_TIME;
                        PlayerAnimation.SwitchAnimation("Walk");
                        PlayerAnimation.SwitchAnimation("Not eating");
                    }
                }
                else
                {
                    PlayerProperties.MaxCircleBarFillingTime = PlayerProperties.MaxCircleFillingTime_ATTACK;
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                PlayerProperties.FoodConsumingTimeLeft = PlayerProperties.FOOD_CONSUMING_MAX_TIME;
                PlayerAnimation.SwitchAnimation("Not eating");
            }
            
            if (PlayerEquipment.Book != null && Input.GetKeyDown(KeyCode.X))
            {
                PlayerStateSwitcher.SwitchState<MagicCastingPlayerState>();
            }
            
        }

        /// <summary>
        /// Checks whether the selected inventory item is a type of food
        /// </summary>
        /// <returns>True if this is a food item</returns>
        private bool CheckWhetherSelectedItemIsFood()
        {
            return PlayerInventory.SelectedItem is FoodItemConfiguration;
        }

        protected override void OnStart()
        {
            PlayerAnimation.SwitchAnimation("Walk");
        }

        public override void Stop()
        { }
        
        protected override void StaminaIsOver()
        { }

    }
}
