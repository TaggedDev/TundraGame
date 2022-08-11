using System.Collections.Generic;
using System.Linq;
using CameraConfiguration;
using Creatures.Player.States;
using Player.Behaviour;
using UnityEngine;

namespace Creatures.Player.Behaviour
{
    public class PlayerBehaviour : MonoBehaviour, IPlayerStateSwitcher
    {

        //Properites
        public float MaxStarve => maxStarve;
        public float CurrentStarveCapacity => _currentStarveCapacity;

        public float PerfectTemperature => perfectTemperature;
        public float CurrentTemperature => _currentTemperature;

        public float AbsoluteTemperatureAmplitude => absoluteTemperatureAmplitude;

        public float MaxHealth
        {
            get => maxHealth;
            set => maxHealth = value;
        }

        public float CurrentHealth => _currentHealth;

        public float CurrentStamina => _currentStamina;

        public float MaxStamina => maxStamina;

        public float MaxWarmLevel => maxWarmLevel;

        public float CurrentWarmLevel => _currentWarmLevel;

        public bool IsOverweight => _inventoryController.Inventory.TotalWeight > maxLoadCapacity;

        public float OverweightCoefficient => _inventoryController.Inventory.TotalWeight / maxLoadCapacity;

        public float ThrowPrepareTime => throwPrepareTime;
        // Fields
        [SerializeField] private float maxStarve;
        [SerializeField] private float saturationTime;

        [SerializeField] private float perfectTemperature;
        [SerializeField] private float absoluteTemperatureAmplitude;
        [SerializeField] private float hotTemperature;
        [SerializeField] private float maxWarmLevel;

        [SerializeField] private float maxHealth;

        [SerializeField] private float maxStamina;
        [SerializeField] private float maxLoadCapacity;
        [SerializeField] private float throwPrepareTime;
        // Variables
        //TODO: Здесь нужно думаю, по-хорошему, как-нибудь закрыть эти поля для доступа, но разрешить их изменение в классах States
        internal float _currentStarveCapacity;
        internal float _currentSaturationTime;

        internal float _currentTemperature;
        internal float _currentWarmLevel;

        internal float _currentHealth;

        internal float _currentStamina;
        internal float _throwLoadingProgress;

        private BasicPlayerState _currentState;
        private PlayerMovement _playerMovement;
        private List<BasicPlayerState> _allStates;
        private CameraMovement _cameraHolder;
        private Camera _mainCamera;
        private PlayerInventoryController _inventoryController;

        //private float cameraDistance;


        private void Start()
        {
            //cameraDistance = Vector3.Distance(Camera.main.transform.position, transform.position);
            _mainCamera = Camera.main;
            _cameraHolder = transform.parent.GetComponentInChildren<CameraMovement>();
            _playerMovement = GetComponent<PlayerMovement>();
            _inventoryController = GetComponent<PlayerInventoryController>();
            _allStates = new List<BasicPlayerState>()
            {
                new IdlePlayerState(_playerMovement, this),
                new WalkingPlayerState(_playerMovement,  this),
                new SprintingPlayerState(_playerMovement, this),
            };
            _currentState = _allStates[0];
            _currentState.Start();
            _mainCamera.transform.RotateAround(transform.position, Vector3.up, 45);
            _playerMovement.UpdateDirections();

            //Initialize health, starvation and temperature:
            _currentStarveCapacity = maxStarve;
            _currentSaturationTime = saturationTime;

            _currentTemperature = PerfectTemperature;

            _currentHealth = maxHealth;

            _currentStamina = maxStamina;
        }

        private void Update()
        {
            MoveCharacter();
            _cameraHolder.transform.position = transform.position;
            ContinueStarving();
            UpdateTemperature();
            LoadForThrow();
        }

        private void MoveCharacter() => _currentState.MoveCharacter();

        private void ContinueStarving() => _currentState.ContinueStarving();

        private void UpdateTemperature() => _currentState.UpdateTemperature();

        private void LoadForThrow() => _currentState.LoadForThrow();

        public void ThrowItem()
        {
            _throwLoadingProgress = ThrowPrepareTime;//Вся эта странная история нужна для того, чтобы он кидал в нужую сторону. 
            Vector3 target = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            target = Quaternion.Euler(0, 90, 0) * new Vector3(target.x, 0, target.z).normalized;
            print(target);
            _inventoryController.Inventory.Slots[_inventoryController.SelectedInventorySlot].ThrowItem(transform.position, (target).normalized);//TODO: возможно целью всё же будет мышь.
        }

        public void SwitchState<T>() where T : BasicPlayerState
        {
            var state = _allStates.FirstOrDefault(st => st is T);
            _currentState.Stop();
            state.Start();
            _currentState = state;
            _throwLoadingProgress = ThrowPrepareTime;
        }
    }
}