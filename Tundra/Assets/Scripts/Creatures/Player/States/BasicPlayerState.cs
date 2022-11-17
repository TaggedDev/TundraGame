using System;
using Creatures.Player.Behaviour;
using Creatures.Player.Inventory;
using GUI.BestiaryGUI;
using GUI.GameplayGUI;
using UnityEngine;

namespace Creatures.Player.States
{
    public abstract class BasicPlayerState
    {
        protected readonly PlayerMovement PlayerMovement;
        protected readonly IPlayerStateSwitcher PlayerStateSwitcher;
        protected readonly PlayerBehaviour PlayerBehaviour;
        protected readonly PlayerProperties PlayerProperties;
        protected readonly Rigidbody PlayerRigidBody;
        protected readonly PlayerEquipment PlayerEquipment;
        protected readonly PlayerInventory PlayerInventory;
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

        private Vector3 velocity;
        private readonly EscapeMenu _escapeCanvas;
        private readonly BestiaryPanel _bestiaryPanel;

        protected BasicPlayerState(PlayerMovement playerMovement, IPlayerStateSwitcher switcher, 
            PlayerProperties playerProperties, PlayerInventory playerInventory, EscapeMenu escapeCanvas,
            BestiaryPanel bestiaryPanel)
        {
            PlayerBehaviour = (PlayerBehaviour)switcher;
            PlayerMovement = playerMovement;
            PlayerStateSwitcher = switcher;
            PlayerMovement = playerMovement;
            PlayerProperties = playerProperties;
            PlayerRigidBody = PlayerBehaviour.gameObject.GetComponent<Rigidbody>();
            PlayerEquipment = PlayerBehaviour.gameObject.GetComponent<PlayerEquipment>();
            PlayerInventory = playerInventory;
            PlayerInventory.SelectedItemChanged += InventorySelectedSlotChanged;
            _escapeCanvas = escapeCanvas;
            _bestiaryPanel = bestiaryPanel;
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
        /// Basic movement with sprint
        /// </summary>
        public virtual void MoveCharacter()
        {
            if (Input.GetKey(KeyCode.LeftShift) && !Input.GetMouseButton(2) && !(this is SprintPlayerState))
            {
                if (PlayerProperties.CurrentStamina > 0) PlayerStateSwitcher.SwitchState<SprintPlayerState>();
            }
            else if (this is SprintPlayerState && !Input.GetKey(KeyCode.LeftShift))
            {
                PlayerStateSwitcher.SwitchState<WalkPlayerState>();
            }

            float _h = Input.GetAxis("Horizontal");
            float _v = Input.GetAxis("Vertical");

            if (_h == 0 && _v == 0 && !(this is IdlePlayerState))
                PlayerStateSwitcher.SwitchState<IdlePlayerState>();

            Vector3 _rightMovement = PlayerMovement.Right * (PlayerMovement.Speed * SpeedCoefficient * Time.deltaTime * _h);
            Vector3 _forwardMovement = PlayerMovement.Forward * (PlayerMovement.Speed * SpeedCoefficient * Time.deltaTime * _v);

            PlayerMovement.Heading = Vector3.Normalize(_rightMovement + _forwardMovement);

            var transform = PlayerMovement.transform;
            transform.forward += PlayerMovement.Heading;
            velocity = Vector3.Lerp(velocity, (_rightMovement + _forwardMovement) * 75f, 0.5f);//TODO: Multiplier is needed to increase force with which player can reach environment

            PlayerRigidBody.velocity = new Vector3(velocity.x, PlayerRigidBody.velocity.y, velocity.z);
        }

        /// <summary>
        /// Handles the logic of pressing escape in different states
        /// </summary>
        public virtual void HandleEscapeButton()
        {
            _escapeCanvas.gameObject.SetActive(!_escapeCanvas.gameObject.activeSelf);
        }
        
        /// <summary>
        /// Updates starving state in <see cref="PlayerBehaviour"/>
        /// </summary>
        public virtual void ContinueStarving()
        {
            if (PlayerProperties._currentStarvationTime > 0)
            {
                PlayerProperties._currentStarvationTime -= Time.deltaTime;
                return;
            }
            PlayerProperties.CurrentStarvationCapacity -= StarvingConsumptionCoefficient;
            
            if (PlayerProperties.CurrentStarvationCapacity < 0)
            {
                PlayerProperties.CurrentStarvationCapacity = 0;
                PlayerProperties.CurrentHealth -= 1f * Time.deltaTime;
                if (PlayerProperties.CurrentHealth < 0) 
                    PlayerProperties.CurrentHealth = 0;
            }
        }
        
        protected virtual void InventorySelectedSlotChanged(object sender, EventArgs e)
        {
            if (PlayerInventory.SelectedItem is PlaceableItemConfiguration)
                PlayerStateSwitcher.SwitchState<BuildingPlayerState>();
        }

        /// <summary>
        /// Updates player warm with current state coefficient.
        /// </summary>
        public virtual void ContinueFreeze()
        {
            PlayerProperties.CurrentWarmLevel -= WarmConsumptionCoefficient * Time.deltaTime;
            if (PlayerProperties.CurrentWarmLevel < 0)
            {
                PlayerProperties.CurrentWarmLevel = 0;
                PlayerProperties.CurrentHealth -= 1f * Time.deltaTime;
                if (PlayerProperties.CurrentHealth < 0) PlayerProperties.CurrentHealth = 0;
            }
        }

        public virtual void SpendStamina()
        {
            if (PlayerProperties.CurrentStamina > 0)
                PlayerProperties.CurrentStamina -= StaminaConsumption * Time.deltaTime;
            if (PlayerProperties.CurrentStamina <= 0) 
                StaminaIsOver();
        }

        protected abstract void StaminaIsOver();
        
        /// <summary>
        /// Loads to hit.
        /// </summary>
        public virtual void PrepareForHit()
        {
            if (!(this is BusyPlayerState) && !(this is MagicCastingPlayerState))
            {
                if (Input.GetMouseButton(0))
                {
                    PlayerProperties.CurrentHitProgress += Time.smoothDeltaTime;
                }
                else PlayerProperties.CurrentHitProgress -= Time.deltaTime;
                if (PlayerProperties.CurrentHitProgress < 0) PlayerProperties.CurrentHitProgress = 0;
                if (PlayerProperties.CurrentHitProgress > PlayerProperties.HitPreparationTime)
                {
                    PlayerBehaviour.Hit();
                    PlayerProperties.CurrentHitProgress = 0;
                }
            }
            else PlayerProperties.CurrentHitProgress = 0;
        }
        /// <summary>
        /// Receives player input for changing states with opening related menus.
        /// </summary>
        public virtual void HandleUserInput()
        {
            if (this is BusyPlayerState && Input.GetKeyDown(KeyCode.Escape))
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
            
            // Open bestiary if player is not busy and presses B key
            if (!(this is BusyPlayerState) && Input.GetKeyDown(KeyCode.B))
            {
                HandleBestiaryOpen();
            }
        }
        
        public virtual void OnPlayerSelectedItemChanged(PlayerInventory inventory)
        {
            if (inventory.SelectedItem is PlaceableItemConfiguration)
            {
                PlayerStateSwitcher.SwitchState<BuildingPlayerState>();
            }
            else if (this is BuildingPlayerState)
                PlayerStateSwitcher.SwitchState<IdlePlayerState>();
        }

        /// <summary>
        /// Handles Bestiary hotkey
        /// </summary>
        public virtual void HandleBestiaryOpen()
        {
            _bestiaryPanel.gameObject.SetActive(!_bestiaryPanel.gameObject.activeSelf);
        }
    }

}