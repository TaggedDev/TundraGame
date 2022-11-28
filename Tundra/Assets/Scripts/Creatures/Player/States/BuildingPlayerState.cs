using Creatures.Player.Behaviour;
using Creatures.Player.Inventory;
using GUI.GameplayGUI;
using System.Collections;
using System.Collections.Generic;
using GUI.BestiaryGUI;
using Creatures.Player.Inventory.ItemConfiguration;
using UnityEngine;

namespace Creatures.Player.States
{
    public class BuildingPlayerState : BasicPlayerState
    {
        private const float speed = .7f; //ain't it supposed to be set somwhere in 1 place for everyone?
        private PlayerBuild _playerBuild;
        
        public BuildingPlayerState(PlayerMovement playerMovement, IPlayerStateSwitcher switcher,
            PlayerProperties playerProperties, PlayerInventory inventory, EscapeMenu escapeCanvas, 
            PlayerBuild playerBuild, BestiaryPanel bestiaryPanel) 
            : base(playerMovement, switcher, playerProperties, inventory, escapeCanvas, bestiaryPanel)
        {
            _playerBuild = playerBuild;
        }
        
        protected override float StarvingConsumptionCoefficient => 1f;

        protected override float StaminaConsumption => -.5f; //а за overweight мы стамину не едим? (в Valheim такое было)

        protected override float SpeedCoefficient => speed * (PlayerBehaviour.IsOverweight ? 0.5f : 1f);

        protected override float WarmConsumptionCoefficient => 1f;

        public override void Start()
        {
            PlayerMovement.CanSprint = false;
            _playerBuild.ObjectPlaced += ObjectPlaced;
            var item = PlayerInventory.SelectedItem as PlaceableItemConfiguration;
            if (!(item is null))
            {
                _playerBuild.PlacableObj = item.RepresentedObject;
                _playerBuild.enabled = true;    
            }
        }

        private void ObjectPlaced(object source, System.EventArgs e)
        {
            PlayerInventory.InventoryContainer[PlayerInventory.SelectedInventorySlot].Clear();
        }

        public override void Stop()
        {
            PlayerMovement.CanSprint = true;
            _playerBuild.enabled = false;
        }

        protected override void StaminaIsOver()
        {
            //nothing should happen
            return;
        }
    }
}