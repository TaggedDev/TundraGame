using Creatures.Player.Behaviour;
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
            PlayerProperties playerProperties,
            PlayerInventory playerInventory, EscapeMenu escapeCanvas) : base(playerMovement, switcher, playerProperties,
            playerInventory, escapeCanvas)
        { }

        protected override float StarvingConsumptionCoefficient => 2f;

        protected override float StaminaConsumption => 0f;

        protected override float SpeedCoefficient => 1 * (PlayerBehaviour.IsOverweight ? 0.5f : 1f);

        protected override float WarmConsumptionCoefficient => 2f;

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