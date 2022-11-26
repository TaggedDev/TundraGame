using System;
using Creatures.Player.Behaviour;
using UnityEngine;
using Creatures.Player.Inventory.ItemConfiguration;
using Creatures.Player.Inventory;
using Creatures.Player.Magic;
using GUI.BestiaryGUI;
using GUI.GameplayGUI;

namespace Creatures.Player.States
{
    public class MagicCastingPlayerState : BasicPlayerState
    {
        private PlayerMagic _playerMagic;

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
            _playerMagic=playerMagic;
            _playerMagic.SpellCast += ExitState;
        }

        /// <summary>
        /// Exits from this state
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExitState(object sender, Spell e)
        {
            PlayerStateSwitcher.SwitchState<IdlePlayerState>();
            _playerMagic.Dispell();
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
            PlayerMovement.CanSprint = false;
            _playerMagic.Book = (BookEquipmentConfiguration)PlayerEquipment.Book;
            _playerMagic.StartSpelling();
            PlayerBehaviour.gameObject.GetComponent<PlayerInventory>().UnselectItem();
        }

        public override void Stop()
        {
            PlayerMovement.CanSprint = true;
        }

        protected override void StaminaIsOver()
        {
        }

        public override void HandleUserInput()
        {
            base.HandleUserInput();
            if (Input.GetKeyDown(KeyCode.Alpha1)) _playerMagic.AddElement(0);
            if (Input.GetKeyDown(KeyCode.Alpha2)) _playerMagic.AddElement(1);
            if (Input.GetKeyDown(KeyCode.Alpha3)) _playerMagic.AddElement(2);
            if (Input.GetKeyDown(KeyCode.Alpha4)) _playerMagic.AddElement(3);
            if (Input.GetKeyDown(KeyCode.Alpha5)) _playerMagic.AddElement(4);
            if (Input.GetKeyDown(KeyCode.F))
            {
                _playerMagic.PrepareForCasting();
                _playerMagic.CastSpell();
            }
        }

        internal void Dispell()
        {
            _playerMagic.Dispell();
        }
    }
}
