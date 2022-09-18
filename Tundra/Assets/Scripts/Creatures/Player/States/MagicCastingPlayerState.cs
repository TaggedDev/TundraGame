using Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Creatures.Player.Behaviour;

namespace Creatures.Player.States
{
    public class MagicCastingPlayerState : BasicPlayerState
    {
        private PlayerMagic _playerMagic;

        protected override float StarvingConsumptionCoefficient => throw new NotImplementedException();

        protected override float StaminaConsumption => throw new NotImplementedException();

        protected override float SpeedCoefficient => throw new NotImplementedException();

        protected override float WarmConsumptionCoefficient => throw new NotImplementedException();

        public MagicCastingPlayerState(PlayerMovement playerMovement, IPlayerStateSwitcher switcher, PlayerProperties playerProperties, PlayerMagic playerMagic)
            : base(playerMovement, switcher, playerProperties)
        {
            _playerMagic=playerMagic;
        }

        public override void MoveCharacter()
        {
            //Here's nothing. Player should'nt move in this state.
        }

        public override void ContinueFreeze()
        {
            //I guess it's like a pause, player should not spend temperature on it
        }

        public override void ContinueStarving()
        {
            //And so I can say for starving.
        }

        public override void LoadForThrow()
        {
            //Player cannot throw items while he's in magic spelling.
        }

        public override void SpendStamina()
        {
            //Player doesn't spend stamina while he creates magic.
        }

        public override void Start()
        {
            _playerMagic.IsEnabled = true;
        }

        public override void Stop()
        {
            _playerMagic.IsEnabled = false;
        }

        protected override void StaminaIsOver()
        {
        }
    }
}
