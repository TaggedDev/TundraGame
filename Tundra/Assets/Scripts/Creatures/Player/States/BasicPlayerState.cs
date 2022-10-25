using Creatures.Player.Behaviour;
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

        protected BasicPlayerState(PlayerMovement playerMovement, IPlayerStateSwitcher switcher, PlayerProperties playerProperties)
        {
            PlayerBehaviour = (PlayerBehaviour)switcher;
            PlayerMovement = playerMovement;
            PlayerStateSwitcher = switcher;
            PlayerMovement = playerMovement;
            PlayerProperties = playerProperties;
            PlayerRigidBody = PlayerBehaviour.gameObject.GetComponent<Rigidbody>();
            PlayerEquipment = PlayerBehaviour.gameObject.GetComponent<PlayerEquipment>();
        }

        /// <summary>
        /// On State changed | Start
        /// </summary>
        public abstract void Start();

        /// <summary>
        /// When PlacableObbject is Chosen
        /// </summary>
        

        /// <summary>
        /// On State changed | Stop
        /// </summary>
        public abstract void Stop();

        /// <summary>
        /// Handles the logic of pressing escape in different states
        /// </summary>
        public abstract void HandleEscapeButton();

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
            //Debug.Log(velocity);
            //PlayerRigidBody.AddForce(force);

            PlayerRigidBody.velocity = new Vector3(velocity.x, PlayerRigidBody.velocity.y, velocity.z);

            //var position = transform.position;
            //position += _rightMovement;
            //position += _forwardMovement;
            //transform.position = position;
        }

        /// <summary>
        /// Updates starving state in <see cref="PlayerBehaviour"/>
        /// </summary>
        public virtual void ContinueStarving()
        {
            //TODO: Maybe this algorithm is not as good as I think
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
                if (PlayerProperties.CurrentHealth < 0) PlayerProperties.CurrentHealth = 0;
            }
        }

        /// <summary>
        /// Updates player warm with current state coefficient.
        /// </summary>
        public virtual void ContinueFreeze()
        {
            //TODO: Do something with it.
            PlayerProperties.CurrentWarmLevel -= WarmConsumptionCoefficient * Time.deltaTime;
            if (PlayerProperties.CurrentWarmLevel < 0)
            {
                PlayerProperties.CurrentWarmLevel = 0;
                PlayerProperties.CurrentHealth -= 1f * Time.deltaTime;
                if (PlayerProperties.CurrentHealth < 0) PlayerProperties.CurrentHealth = 0;
            }
        }

        public virtual void PickUpBuildingBlock()
        {
            PlayerStateSwitcher.SwitchState<BuildingPlayerState>();
        }

        public virtual void SpendStamina()
        {
            if (PlayerProperties.CurrentStamina > 0) PlayerProperties.CurrentStamina -= (StaminaConsumption * Time.deltaTime);
            if (PlayerProperties.CurrentStamina <= 0) StaminaIsOver();
        }

        protected abstract void StaminaIsOver();

        /// <summary>
        /// Loads weapon for throwing.
        /// </summary>
        public virtual void LoadForThrow()
        {
            if (Input.GetMouseButton(2))
            {
                PlayerProperties._throwLoadingProgress -= Time.deltaTime;
                if (PlayerProperties._throwLoadingProgress <= 0) PlayerProperties._throwLoadingProgress = 0;
            }
            else
            {
                if (PlayerProperties._throwLoadingProgress <= 0) PlayerBehaviour.ThrowItem();
                PlayerProperties._throwLoadingProgress = PlayerProperties.ThrowPrepareTime;
            }
        }
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
        /// Recievs player input for changing states with opening related menus.
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
    }
}