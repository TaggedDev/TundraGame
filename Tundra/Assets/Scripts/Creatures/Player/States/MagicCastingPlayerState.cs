using System;
using Creatures.Player.Behaviour;
using UnityEngine;
using Creatures.Player.Inventory;
using GUI.BestiaryGUI;
using GUI.GameplayGUI;

namespace Creatures.Player.States
{
    public class MagicCastingPlayerState : BasicPlayerState
    {
        private PlayerMagic _playerMagic;
        private EscapeMenu _escapeCanvas;

        private const float speed = 1f;

        protected override float StarvingConsumptionCoefficient => 2f;

        protected override float StaminaConsumption => 0f;

        protected override float SpeedCoefficient => speed * (PlayerBehaviour.IsOverweight ? 0.5f : 1f);

        protected override float WarmConsumptionCoefficient => 2f;

        private Vector3 velocity;

        public MagicCastingPlayerState(PlayerMovement playerMovement, IPlayerStateSwitcher switcher,
            PlayerProperties playerProperties, PlayerMagic playerMagic, PlayerInventory inventory, 
            EscapeMenu escapeCanvas, BestiaryPanel bestiaryPanel)
            : base(playerMovement, switcher, playerProperties, inventory, escapeCanvas, bestiaryPanel)
        {
            _playerMagic = playerMagic;
            _playerMagic.SpellCast += ExitState;
        }

        private void ExitState(object sender, EventArgs e)
        {
            PlayerStateSwitcher.SwitchState<IdlePlayerState>();
        }

        public override void HandleEscapeButton()
        {
            //I think I've accidentally deleted something from here
            //woopsie
            throw new NotImplementedException();
        }

        public override void MoveCharacter()
        {
            float _h = Input.GetAxis("Horizontal");
            float _v = Input.GetAxis("Vertical");

            Vector3 _rightMovement = PlayerMovement.Right * (PlayerMovement.Speed * SpeedCoefficient * Time.deltaTime * _h);
            Vector3 _forwardMovement = PlayerMovement.Forward * (PlayerMovement.Speed * SpeedCoefficient * Time.deltaTime * _v);

            PlayerMovement.Heading = Vector3.Normalize(_rightMovement + _forwardMovement);

            var transform = PlayerMovement.transform;
            transform.forward += PlayerMovement.Heading;
            velocity = Vector3.Lerp(velocity, (_rightMovement + _forwardMovement) * 75f, 0.5f);

            PlayerRigidBody.velocity = new Vector3(velocity.x, PlayerRigidBody.velocity.y, velocity.z);
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
