using Creatures.Player.Behaviour;
using UnityEngine;

namespace Creatures.Player.States
{
    public class IdlePlayerState : BasicPlayerState
    {
        public IdlePlayerState(PlayerMovement playerMovement, IPlayerStateSwitcher switcher, PlayerProperties playerProperties) 
            : base (playerMovement, switcher, playerProperties)
        { }

        private float _h = 0, _v = 0;

        protected override float StarvingConsumptionCoefficient => 1f;

        protected override float StaminaConsumption => -1f;

        protected override float SpeedCoefficient => 0;

        protected override float WarmConsumptionCoefficient => 2f;

        public override void MoveCharacter()
        {
            _h = Input.GetAxis("Horizontal");
            _v = Input.GetAxis("Vertical");

            if (Mathf.Abs(_h) > 0 || Mathf.Abs(_v) > 0)
                PlayerStateSwitcher.SwitchState<WalkPlayerState>();
        }

        public override void Start()
        {
            Debug.Log("Got Idle State");
        }

        public override void Stop()
        {
            Debug.Log("Lost Idle State");
        }

        public override void SpendStamina()
        {
            PlayerProperties.CurrentStamina -= (StaminaConsumption * Time.deltaTime);
            if (PlayerProperties.CurrentStamina > PlayerProperties.MaxStamina) PlayerProperties.CurrentStamina = PlayerProperties.MaxStamina;
        }

        protected override void StaminaIsOver()
        { }
    }
}
