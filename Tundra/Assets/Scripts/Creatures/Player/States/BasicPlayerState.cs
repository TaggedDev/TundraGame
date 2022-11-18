using Creatures.Player.Behaviour;
using UnityEngine;
using System;
using Creatures.Player.Inventory;
using Creatures.Player.Inventory.ItemConfiguration;
using GUI.GameplayGUI;

namespace Creatures.Player.States
{
    /// <summary>
    /// An abstract model to describe the behaviour of player state
    /// </summary>
    public abstract class BasicPlayerState
    {
        protected readonly PlayerAnimation PlayerAnimation;
        protected readonly PlayerMovement PlayerMovement;
        protected readonly IPlayerStateSwitcher PlayerStateSwitcher;
        protected readonly PlayerBehaviour PlayerBehaviour;
        protected readonly PlayerProperties PlayerProperties;
        protected readonly Rigidbody PlayerRigidBody;
        protected readonly PlayerEquipment PlayerEquipment;
        protected readonly PlayerInventory PlayerInventory;
        private Vector3 velocity;
        private EscapeMenu _escapeCanvas;
        
        /// <summary>
        /// The hunger consumption value of this state.
        /// </summary>
        protected abstract float StarvingConsumptionCoefficient { get; }
        /// <summary>
        /// The stamina consumption value of this state.
        /// </summary>
        protected abstract float StaminaConsumption { get; }
        /// <summary>
        /// Player speed multiplier.
        /// </summary>
        protected abstract float SpeedCoefficient { get; }
        /// <summary>
        /// The warm consumption coefficient of this state.
        /// </summary>
        protected abstract float WarmConsumptionCoefficient { get; }
        
        protected BasicPlayerState(PlayerMovement playerMovement, IPlayerStateSwitcher switcher, PlayerProperties playerProperties, PlayerInventory playerInventory, EscapeMenu escapeCanvas)
        {
            PlayerBehaviour = (PlayerBehaviour)switcher;
            PlayerMovement = playerMovement;
            PlayerStateSwitcher = switcher;
            PlayerMovement = playerMovement;
            PlayerProperties = playerProperties;
            PlayerRigidBody = PlayerBehaviour.gameObject.GetComponent<Rigidbody>();
            PlayerEquipment = PlayerBehaviour.gameObject.GetComponent<PlayerEquipment>();
            PlayerAnimation = PlayerBehaviour.GetComponent<PlayerAnimation>();
            PlayerInventory = playerInventory;
            PlayerInventory.SelectedItemChanged += InventorySelectedSlotChanged;
            _escapeCanvas = escapeCanvas;
        }

        /// <summary>
        /// On State changed | Start
        /// </summary>
        public abstract void Start();
        
        /// <summary>
        /// On State changed | Stop
        /// </summary>
        public abstract void Stop();

        /// <summary>
        /// Handles the logic of pressing escape in different states
        /// </summary>
        public virtual void HandleEscapeButton()
        {
            _escapeCanvas.gameObject.SetActive(!_escapeCanvas.gameObject.activeSelf);
        }

        /// <summary>
        /// Basic movement with sprint
        /// </summary>
        public virtual void MoveCharacter()
        {
            if (PlayerMovement.CanSprint && Input.GetKey(KeyCode.LeftShift) && !Input.GetMouseButton(2) && !(this is SprintPlayerState) && !(this is BuildingPlayerState))
            {
                if (PlayerProperties.CurrentStaminaPoints > 0) PlayerStateSwitcher.SwitchState<SprintPlayerState>();
            }
            else if (this is SprintPlayerState && !Input.GetKey(KeyCode.LeftShift))
            {
                PlayerStateSwitcher.SwitchState<WalkPlayerState>();
            }

            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            if (h == 0 && v == 0 && !(this is IdlePlayerState) && !(this is BuildingPlayerState))
                PlayerStateSwitcher.SwitchState<IdlePlayerState>();

            Vector3 _rightMovement = PlayerMovement.Right * (PlayerMovement.Speed * SpeedCoefficient * Time.deltaTime * h);
            Vector3 _forwardMovement = PlayerMovement.Forward * (PlayerMovement.Speed * SpeedCoefficient * Time.deltaTime * v);

            PlayerMovement.Heading = Vector3.Normalize(_rightMovement + _forwardMovement);

            var transform = PlayerMovement.transform;
            transform.forward += PlayerMovement.Heading;
            velocity = Vector3.Lerp(velocity, (_rightMovement + _forwardMovement) * 75f, 0.5f);//TODO: Multiplier is needed to increase force with which player can reach environment
            PlayerRigidBody.velocity = new Vector3(velocity.x, PlayerRigidBody.velocity.y, velocity.z);
        }

        /// <summary>
        /// Updates starving state in <see cref="PlayerBehaviour"/>
        /// </summary>
        public virtual void ContinueStarving()
        {
            // Use saturation instead of starve points if there are any;
            if (PlayerProperties.CurrentSaturationPoints >= 0)
            {
                PlayerProperties.CurrentSaturationPoints -= StarvingConsumptionCoefficient * Time.deltaTime;
                return;
            }

            // If there is any starvation points - consume them before the health points 
            if (PlayerProperties.CurrentStarvePoints >= 0)
            {
                PlayerProperties.CurrentStarvePoints -= StarvingConsumptionCoefficient * Time.deltaTime;
            }
            // Otherwise, start killing the player of hunger
            else
            {
                PlayerProperties.CurrentStarvePoints = 0;
                PlayerProperties.CurrentHealthPoints -= 1f * Time.deltaTime;
                
                if (PlayerProperties.CurrentHealthPoints < 0) 
                    PlayerProperties.CurrentHealthPoints = 0;
            }
        }
        
        /// <summary>
        /// An event called whenever user changed his item
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void InventorySelectedSlotChanged(object sender, EventArgs e)
        {
            if (PlayerInventory.SelectedItem is PlaceableItemConfiguration)
                PlayerStateSwitcher.SwitchState<BuildingPlayerState>();
        }

        /// <summary>
        /// Updates player warm with current state coefficient.
        /// </summary>
        public virtual void ContinueFreezing()
        {
            PlayerProperties.CurrentWarmthPoints -= WarmConsumptionCoefficient * Time.deltaTime;
            if (PlayerProperties.CurrentWarmthPoints < 0)
            {
                PlayerProperties.CurrentWarmthPoints = 0;
                PlayerProperties.CurrentHealthPoints -= 1f * Time.deltaTime;
                if (PlayerProperties.CurrentHealthPoints < 0) PlayerProperties.CurrentHealthPoints = 0;
            }
        }


        public virtual void SpendStamina()
        {
            if (PlayerProperties.CurrentStaminaPoints > 0) PlayerProperties.CurrentStaminaPoints -= (StaminaConsumption * Time.deltaTime);
            if (PlayerProperties.CurrentStaminaPoints <= 0) StaminaIsOver();
        }

        protected abstract void StaminaIsOver();
        
        /// <summary>
        /// Loads to hit.
        /// </summary>
        public virtual void PrepareForHit()
        {
            if (!(this is BusyPlayerState) && !(this is MagicCastingPlayerState))
            {
                // Handle LMB press, filling the charge circle 
                if (Input.GetMouseButton(0))
                {
                    PlayerProperties.CurrentCircleBarFillingTime += Time.smoothDeltaTime;
                }
                // In case the stop pressing LMB -> reset the circle to zero
                else
                {
                    PlayerProperties.CurrentCircleBarFillingTime = 0f;
                }
                
                // Hit in front of self when charge is 100%. To be reworked in Melee branch 
                if (PlayerProperties.CurrentCircleBarFillingTime > PlayerProperties.MaxCircleBarFillingTime)
                {
                    PlayerBehaviour.Hit();
                    PlayerProperties.CurrentCircleBarFillingTime = 0;
                }
            }
            else
            {
                PlayerProperties.CurrentCircleBarFillingTime = 0;
            }
        }

        /// <summary>
        /// Receives player input for changing states with opening related menus.
        /// </summary>
        public virtual void HandleUserInput()
        {
            if (!(this is BusyPlayerState) && !(this is MagicCastingPlayerState) && Input.GetKeyDown(KeyCode.B))
            {
                PlayerStateSwitcher.SwitchState<BusyPlayerState>();
            }
            else if (this is BusyPlayerState && Input.GetKeyDown(KeyCode.Escape))
            {
                PlayerStateSwitcher.SwitchState<IdlePlayerState>();
            }
            if (!(this is BusyPlayerState) && !(this is MagicCastingPlayerState)
                && PlayerEquipment.Book != null && Input.GetKeyDown(KeyCode.X))
            {
                PlayerStateSwitcher.SwitchState<MagicCastingPlayerState>();
            }
            else if (this is MagicCastingPlayerState && Input.GetKeyDown(KeyCode.X))
            {
                PlayerStateSwitcher.SwitchState<IdlePlayerState>();
                (this as MagicCastingPlayerState).Dispell();
            }
        }

        public virtual void OnPlayerSelectedItemChanged(PlayerInventory inventory)
        {
            // If the selected item is a building item -> switch to building state 
            if (inventory.SelectedItem is PlaceableItemConfiguration)
            {
                PlayerStateSwitcher.SwitchState<BuildingPlayerState>();
            }
            // Otherwise, if this not a building item and we are in a building state -> switch to idle
            else if (this is BuildingPlayerState)
            {
                PlayerStateSwitcher.SwitchState<IdlePlayerState>();
            }
            
            // If selected item is null -> switch to idle state and show empty hands
            if (inventory.SelectedItem is null)
            {
                PlayerStateSwitcher.SwitchState<IdlePlayerState>();
                PlayerInventory.ItemHolder.ResetMesh();
            }
            // Selected item is not null -> show it in hands 
            else
            {
                var item = inventory.SelectedItem.ItemInWorldPrefab.GetComponent<DroppedItemBehaviour>();
                PlayerInventory.ItemHolder.SetNewMesh(item.HandedScale,
                    Quaternion.Euler(item.HandedRotation),
                    item.Model, 
                    item.Materials);
                //PlayerInventory.ItemHolder.SetNewMesh(inventory.SelectedItem);
            }
        }

        /// <summary>
        /// Consumes the equipped food and applies effects
        /// </summary>
        protected void ConsumeCurrentFood()
        {
            PlayerProperties.FoodConsumingTimeLeft = PlayerProperties.FOOD_CONSUMING_MAX_TIME;
            // Food won't be null because there is validation before getting in the function
            
            var food = PlayerInventory.SelectedItem as FoodItemConfiguration;
            var calories = food.Calories;

            // If current food is on limit and current saturation is twice bigger than maxstarve, cause player to vomit
            if (PlayerProperties.CurrentStarvePoints == PlayerProperties.MaxStarvePoints &&
                PlayerProperties.CurrentSaturationPoints + calories / 2f >= PlayerProperties.MaxStarvePoints * 2f)
            {
                // Apply vomit de buffs
                PlayerProperties.CurrentSaturationPoints = 0f;
                PlayerProperties.CurrentStarvePoints = PlayerProperties.MaxStarvePoints / 2f;
                PlayerProperties.CurrentHealthPoints -= 10f;
                PlayerProperties.CurrentWarmthPoints -= 50f;
            }
            // If player is just overeating, he gains a half of calories as a saturation effect
            else if (PlayerProperties.CurrentStarvePoints + calories >= PlayerProperties.MaxStarvePoints)
            {
                // is a coef. of how many calories will go to saturation points
                PlayerProperties.CurrentSaturationPoints += calories * .3f; 
            }
            
            // Gaining more than max is handled in properties
            PlayerProperties.CurrentStarvePoints += food.Calories;
            PlayerInventory.Inventory.Slots[PlayerInventory.SelectedInventorySlot].RemoveItems(1);
            PlayerProperties.IsHoldingFood = false;
            PlayerInventory.ItemHolder.ResetMesh();
        }
    }
}