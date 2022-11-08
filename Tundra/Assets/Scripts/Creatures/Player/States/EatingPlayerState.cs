using Creatures.Player.Behaviour;
using Creatures.Player.Inventory.ItemConfiguration;
using GUI.GameplayGUI;
using UnityEngine;

namespace Creatures.Player.States
{
    public class EatingPlayerState : BasicPlayerState
    {
        private Vector3 _velocity;
        private float _slowedCoefficient = 1;
        private const float EATING_SLOW_COEFFICIENT = .2f;
        
        public EatingPlayerState(PlayerMovement playerMovement, IPlayerStateSwitcher switcher,
            PlayerProperties playerProperties, PlayerInventory playerInventory, EscapeMenu escapeCanvas) 
            : base(playerMovement, switcher, playerProperties, playerInventory, escapeCanvas)
        { }

        protected override float StarvingConsumptionCoefficient => 2f;

        protected override float StaminaConsumption => 0f;

        protected override float SpeedCoefficient => 1 * (PlayerBehaviour.IsOverweight ? 0.5f : 1f);

        protected override float WarmConsumptionCoefficient => 2f;

        // We don't need hits in Eating state
        public override void PrepareForHit() { }

        public override void HandleUserInput()
        {
            if (PlayerEquipment.Book != null && Input.GetKeyDown(KeyCode.X))
            {
                PlayerStateSwitcher.SwitchState<MagicCastingPlayerState>();
            }

            if (Input.GetMouseButtonDown(0))
            {
                ConsumeCurrentFood();
            }
        }

        public override void MoveCharacter()
        {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            // Decrease player speed when LMB is pressed
            if (Input.GetMouseButton(0))
                _slowedCoefficient = EATING_SLOW_COEFFICIENT;
            else
                _slowedCoefficient = 1;
            
            
            Vector3 _rightMovement = PlayerMovement.Right * (PlayerMovement.Speed * _slowedCoefficient * 
                                                                 SpeedCoefficient * Time.deltaTime * h);
            Vector3 _forwardMovement = PlayerMovement.Forward * (PlayerMovement.Speed * _slowedCoefficient *
                                                                 SpeedCoefficient * Time.deltaTime * v);
            
            PlayerMovement.Heading = Vector3.Normalize(_rightMovement + _forwardMovement);

            var transform = PlayerMovement.transform;
            transform.forward += PlayerMovement.Heading;
            _velocity = Vector3.Lerp(_velocity, (_rightMovement + _forwardMovement) * 75f, 0.5f);//TODO: Multiplier is needed to increase force with which player can reach environment
            PlayerRigidBody.velocity = new Vector3(_velocity.x, PlayerRigidBody.velocity.y, _velocity.z);
        }

        /// <summary>
        /// Consumes currently equipped item, grants calories and checks if we need to switch state to idle 
        /// </summary>
        private void ConsumeCurrentFood()
        {
            // We are assured that the equipped item is a food. Otherwise, Eating state shouldn't be set
            FoodItemConfiguration food = PlayerInventory.SelectedItem as FoodItemConfiguration;
            
            /*if (_currentStarvation + value >= MaxStarve)
            {
                _currentStarvation = maxStarve;
                _currentSaturation += value / 2;
            }*/
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
                PlayerProperties.CurrentSaturationPoints += calories / 2;
            }
            
            // Gaining more than max is handled in properties
            PlayerProperties.CurrentStarvePoints += food.Calories;
        }
        
        public override void Start()
        {
            PlayerMovement.CanSprint = false;
        }


        public override void Stop()
        {
            PlayerMovement.CanSprint = true;
        }

        protected override void StaminaIsOver()
        { }
    }
}