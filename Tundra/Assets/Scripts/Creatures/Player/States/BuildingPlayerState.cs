using Creatures.Player.Behaviour;
using Creatures.Player.Inventory;
using GUI.GameplayGUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Creatures.Player.States
{
    public class BuildingPlayerState : BasicPlayerState
    {
        private const float speed = .7f; //ain't it supposed to be set somwhere in 1 place for everyone?
        private PlayerBuild _playerBuild;
        
        public BuildingPlayerState(PlayerMovement playerMovement, IPlayerStateSwitcher switcher, PlayerProperties playerProperties, PlayerInventory inventory, EscapeMenu escapeCanvas, PlayerBuild playerBuild) 
            : base(playerMovement, switcher, playerProperties, inventory, escapeCanvas)
        {
            _playerBuild = playerBuild;
        }



        protected override float StarvingConsumptionCoefficient => 1f;

        protected override float StaminaConsumption => -.5f; //а за overweight мы стамину не едим? (в Valheim такое было)

        protected override float SpeedCoefficient => speed * (PlayerBehaviour.IsOverweight ? 0.5f : 1f);

        protected override float WarmConsumptionCoefficient => 1f;

        public override void Start()
        {
            _playerBuild.ObjectPlaced += ObjectPlaced;
            _playerBuild.PlacableObj = (PlayerInventory.SelectedItem as PlaceableItemConfiguration).RepresentedObject;
            _playerBuild.enabled = true;
        }

        private void ObjectPlaced(object source, System.EventArgs e)
        {
            PlayerInventory.Inventory[PlayerInventory.SelectedInventorySlot].Clear();
        }

        public override void Stop()
        {
            _playerBuild.enabled = false;
        }

        protected override void StaminaIsOver()
        {
            //nothing should happen
            return;
        }
    }
}