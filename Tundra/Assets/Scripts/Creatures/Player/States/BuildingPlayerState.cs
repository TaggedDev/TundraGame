using Creatures.Player.Behaviour;
using Creatures.Player.Inventory;
using GUI.GameplayGUI;
using System.Collections;
using System.Collections.Generic;
using GUI.BestiaryGUI;
using Creatures.Player.Inventory.ItemConfiguration;
using UnityEngine;
using UnityEngine.PlayerLoop;
using System.Drawing;

namespace Creatures.Player.States
{
    public class BuildingPlayerState : BasicPlayerState
    {
        private const float speed = .7f; //ain't it supposed to be set somwhere in 1 place for everyone?
        private PlaceableObject _placeableObject;
        private Vector3 _point;
        public BuildingPlayerState(PlayerMovement playerMovement, IPlayerStateSwitcher switcher,
            PlayerProperties playerProperties, PlayerInventory inventory, EscapeMenu escapeCanvas, 
            BestiaryPanel bestiaryPanel) 
            : base(playerMovement, switcher, playerProperties, inventory, escapeCanvas, bestiaryPanel)
        {
            
        }



        protected override float StarvingConsumptionCoefficient => 1f;

        protected override float StaminaConsumption => -.5f; 

        protected override float SpeedCoefficient => speed * (PlayerBehaviour.IsOverweight ? 0.5f : 1f);

        protected override float WarmConsumptionCoefficient => 1f;

        public override void Start()
        {
            PlayerMovement.CanSprint = false;
            Debug.Log("entered");
            if (!(PlayerInventory.SelectedItem is PlaceableItemConfiguration) || PlayerInventory.SelectedItem is null)
            {
                Debug.LogError("Entered Building State without proper Item");
                
            }
            _placeableObject = (PlayerInventory.SelectedItem as PlaceableItemConfiguration).RepresentedObject;
            _placeableObject.GhostObject = Object.Instantiate(_placeableObject.Object);
            _placeableObject.GhostObject.GetComponent<Collider>().enabled = false;

        }
        public override void HandleUserInput()
        {
            if (Input.GetMouseButton(0))
            {
                if (_placeableObject.TryPlacing(_point, new Quaternion(0, Quaternion.LookRotation(PlayerMovement.transform.position - _point).y, 0,
                    Quaternion.LookRotation(PlayerMovement.transform.position - _point).w)))
                {
                    PlayerInventory.ClearSlot(PlayerInventory.SelectedInventorySlot);
                }
            }
            //base.HandleUserInput();
            if (Input.GetMouseButtonUp(0))
            {
                PlayerStateSwitcher.SwitchState<IdlePlayerState>();
            }
        }
        public override void Build()
        {
            
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (!Physics.Raycast(ray, out hit, 200, ~0, QueryTriggerInteraction.Ignore))
                return;
            _point = new Vector3(hit.point.x, hit.point.y + _placeableObject.GhostObject.GetComponent<Renderer>().bounds.extents.y, hit.point.z);
            _placeableObject.DisplayGhostObject(_point, new Quaternion(0, Quaternion.LookRotation(PlayerMovement.transform.position - _placeableObject.GhostObject.transform.position).y, 0, Quaternion.LookRotation(PlayerMovement.transform.position - _placeableObject.GhostObject.transform.position).w));
        }

        public override void InventorySelectedSlotChanged(object sender, int e)
        {
            if(PlayerInventory.SelectedItem is PlaceableItemConfiguration)
            {
                Stop();
                Start();
            }

            PlayerStateSwitcher.SwitchState<IdlePlayerState>();
        }

        public override void Stop()
        {
            Debug.Log("Exited");
            Object.Destroy(_placeableObject.GhostObject);
            PlayerMovement.CanSprint = true;
        }
        protected override void StaminaIsOver()
        {
            //nothing should happen
            return;
        }
    }
}