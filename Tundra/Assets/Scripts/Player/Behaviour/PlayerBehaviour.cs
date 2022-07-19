using System.Collections.Generic;
using System.Linq;
using CameraConfiguration;
using Player.States;
using UnityEngine;

namespace Player.Behaviour
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
        // Fields
        [SerializeField] private float maxStarve;
        [SerializeField] private float saturationTime;

        [SerializeField] private float perfectTemperature;
        [SerializeField] private float absoluteTemperatureAmplitude;
        [SerializeField] private float hotTemperature;
        [SerializeField] private float maxWarmLevel;

        [SerializeField] private float maxHealth;

        [SerializeField] private float maxStamina;
        // Variables
        //TODO: Здесь нужно думаю, по-хорошему, как-нибудь закрыть эти поля для доступа, но разрешить их изменение в классах States
        internal float _currentStarveCapacity;
        internal float _currentSaturationTime;

        internal float _currentTemperature;
        internal float _currentWarmLevel;

        internal float _currentHealth;

        internal float _currentStamina;

        private BasicState _currentState;
        private PlayerMovement _playerMovement;
        private List<BasicState> _allStates;
        private CameraMovement _cameraHolder;
        private Camera _mainCamera;
        
        //private float cameraDistance;


        private void Start()
        {
            //cameraDistance = Vector3.Distance(Camera.main.transform.position, transform.position);
            _mainCamera = Camera.main;
            _cameraHolder = transform.parent.GetComponentInChildren<CameraMovement>();
            _playerMovement = GetComponent<PlayerMovement>();
            _allStates = new List<BasicState>()
            {
                new IdleState(_playerMovement, this),
                new WalkingState(_playerMovement,  this),
                new SprintingState(_playerMovement, this),
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
        }

        private void Update()
        {
            MoveCharacter();
            _cameraHolder.transform.position = transform.position;
            ContinueStarving();
            UpdateTemperature();
        }

        private void MoveCharacter() => _currentState.MoveCharacter();

        private void ContinueStarving() => _currentState.ContinueStarving();

        private void UpdateTemperature() => _currentState.UpdateTemperature();

        public void SwitchState<T>() where T : BasicState
        {
            var state = _allStates.FirstOrDefault(st => st is T);
            _currentState.Stop();
            state.Start();
            _currentState = state;
        }
    }
}
