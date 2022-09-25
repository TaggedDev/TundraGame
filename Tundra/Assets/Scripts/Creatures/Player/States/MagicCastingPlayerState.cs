using Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Creatures.Player.Behaviour;
using UnityEngine;
using Creatures.Player.Inventory;

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
            _playerMagic.SpellCast += ExitState;
        }

        private void ExitState(object sender, EventArgs e)
        {
            PlayerStateSwitcher.SwitchState<IdlePlayerState>();
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
            _playerMagic._config = (BookEquipmentConfiguration)PlayerEquipment.Book;
            _playerMagic.StartSpelling();
        }

        public override void Stop()
        {
            
        }

        protected override void StaminaIsOver()
        {
        }

        public override void HandleUserInput()
        {
            base.HandleUserInput();
            _playerMagic.MaxSpellElementCount += (int)(Input.GetAxis("Mouse ScrollWheel") * 10);
            if (_playerMagic.MaxSpellElementCount > _playerMagic._config.FreeSheets) _playerMagic.MaxSpellElementCount = _playerMagic._config.FreeSheets;
            else if (_playerMagic.MaxSpellElementCount < 1) _playerMagic.MaxSpellElementCount = 1;
            if (Input.GetKeyDown(KeyCode.Alpha1)) _playerMagic.AddElement(0);
            if (Input.GetKeyDown(KeyCode.Alpha2)) _playerMagic.AddElement(1);
            if (Input.GetKeyDown(KeyCode.Alpha3)) _playerMagic.AddElement(2);
            if (Input.GetKeyDown(KeyCode.Alpha4)) _playerMagic.AddElement(3);
            if (Input.GetKeyDown(KeyCode.Alpha5)) _playerMagic.AddElement(4);
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (!_playerMagic.IsReadyForCasting) _playerMagic.PrepareForCasting();
                else _playerMagic.CastSpell();
            }
        }

        internal void Dispell()
        {
            _playerMagic.Dispell();
        }
    }
}
