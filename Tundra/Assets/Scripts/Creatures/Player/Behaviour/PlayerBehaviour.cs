using System.Collections.Generic;
using System.Linq;
using CameraConfiguration;
using Creatures.Player.States;
using Creatures.Player.Behaviour;
using UnityEngine;

namespace Creatures.Player.Behaviour
{
    public class PlayerBehaviour : MonoBehaviour, IPlayerStateSwitcher
    {

        //Properites

        public bool IsOverweight => _inventoryController.Inventory.TotalWeight > _playerProperties.MaxLoadCapacity;

        public float OverweightCoefficient => _inventoryController.Inventory.TotalWeight / _playerProperties.MaxLoadCapacity;
        // Fields

        // Variables
        //TODO: Здесь нужно думаю, по-хорошему, как-нибудь закрыть эти поля для доступа, но разрешить их изменение в классах States
        

        private BasicPlayerState _currentState;
        private PlayerMovement _playerMovement;
        private List<BasicPlayerState> _allStates;
        private CameraMovement _cameraHolder;
        private Camera _mainCamera;
        private PlayerInventoryController _inventoryController;
        private PlayerProperties _playerProperties;

        //private float cameraDistance;


        private void Start()
        {
            //cameraDistance = Vector3.Distance(Camera.main.transform.position, transform.position);
            _mainCamera = Camera.main;
            _cameraHolder = transform.parent.GetComponentInChildren<CameraMovement>();
            _playerMovement = GetComponent<PlayerMovement>();
            _inventoryController = GetComponent<PlayerInventoryController>();
            _playerProperties = GetComponent<PlayerProperties>();
            _allStates = new List<BasicPlayerState>()
            {
                new IdlePlayerState(_playerMovement, this, _playerProperties),
                new WalkPlayerState(_playerMovement,  this, _playerProperties),
                new SprintPlayerState(_playerMovement, this, _playerProperties),
                new BusyPlayerState(_playerMovement, this, _playerProperties)
            };
            _currentState = _allStates[0];
            _currentState.Start();
            _mainCamera.transform.RotateAround(transform.position, Vector3.up, 45);
            _playerMovement.UpdateDirections();
            _playerMovement.Speed = 2f;
            //Initialize health, starvation and temperature:
        }

        private void Update()
        {
            _currentState.MoveCharacter();
            _cameraHolder.transform.position = transform.position;
            _currentState.ContinueStarving();
            _currentState.ContinueFreeze();
            if (Input.GetMouseButton(2)) _currentState.LoadForThrow();
            _currentState.SpendStamina();
            _currentState.HandleUserInput();
        }

        public void ThrowItem()
        {
            _playerProperties._throwLoadingProgress = _playerProperties.ThrowPrepareTime;
            //Вся эта странная история нужна для того, чтобы он кидал в нужую сторону. 
            //TODO: Не работает, надо фиксить.
            Vector3 target = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            target = Quaternion.Euler(0, 90, 0) * new Vector3(target.x, 0, target.z).normalized;
            //print(target);
            _inventoryController.Inventory.Slots[_inventoryController.SelectedInventorySlot].ThrowItem(transform.position, (target).normalized);
        }

        public void SwitchState<T>() where T : BasicPlayerState
        {
            var state = _allStates.FirstOrDefault(st => st is T);
            _currentState.Stop();
            state.Start();
            _currentState = state;
            _playerProperties._throwLoadingProgress = _playerProperties.ThrowPrepareTime;
        }
    }
}