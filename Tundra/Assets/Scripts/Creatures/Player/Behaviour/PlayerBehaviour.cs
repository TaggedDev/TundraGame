using System.Collections.Generic;
using System.Linq;
using CameraConfiguration;
using Creatures.Player.States;
using UnityEngine;
using System;
using GUI.GameplayGUI;

namespace Creatures.Player.Behaviour
{
    public class PlayerBehaviour : MonoBehaviour, IPlayerStateSwitcher
    {
        
        // Properties
        public bool IsOverweight => _inventoryController.Inventory.TotalWeight > _playerProperties.MaxLoadCapacity;
        public float OverweightCoefficient => _inventoryController.Inventory.TotalWeight / _playerProperties.MaxLoadCapacity;

        // Variables
        //TODO: Здесь нужно думаю, по-хорошему, как-нибудь закрыть эти поля для доступа, но разрешить их изменение в классах States

        [SerializeField] private EscapeMenu escapeCanvas;
        [SerializeField] private DeathMenu deathCanvas;
        private Animator _animator;
        private BasicPlayerState _currentState;
        private PlayerMovement _playerMovement;
        private List<BasicPlayerState> _allStates;
        private CameraMovement _cameraHolder;
        private Camera _mainCamera;
        private PlayerInventory _inventoryController;
        private PlayerProperties _playerProperties;
        private PlayerInventory _playerInventory;
        private Rigidbody _rigidbody;
        private PlayerMagic _playerMagic;
        private bool _isDead;
        private PlayerBuild _playerBuild;
        //private float cameraDistance;

        public BasicPlayerState CurrentState => _currentState;

        public event EventHandler StateChanged;

        private void Start()
        {
            /*if (escapeCanvas is null)
                throw new Exception("Escape Canvas object was not assigned to the player behaviour");*/

            //cameraDistance = Vector3.Distance(Camera.main.transform.position, transform.position);
            _mainCamera = Camera.main;
            _cameraHolder = transform.parent.GetComponentInChildren<CameraMovement>();
            _playerMovement = GetComponent<PlayerMovement>();
            _inventoryController = GetComponent<PlayerInventory>();
            _playerProperties = GetComponent<PlayerProperties>();
            _playerInventory = GetComponent<PlayerInventory>();
            _playerMagic = GetComponent<PlayerMagic>();
            _playerBuild = GetComponent<PlayerBuild>();
            _rigidbody = GetComponent<Rigidbody>();
            _animator = GetComponent<Animator>();
            _allStates = new List<BasicPlayerState>()
            {
                new IdlePlayerState(_playerMovement, this, _playerProperties, _playerInventory, escapeCanvas),
                new WalkPlayerState(_playerMovement,  this, _playerProperties, _playerInventory, escapeCanvas),
                new SprintPlayerState(_playerMovement, this, _playerProperties, _playerInventory, escapeCanvas),
                new BusyPlayerState(_playerMovement, this, _playerProperties, _playerInventory, escapeCanvas),
                new MagicCastingPlayerState(_playerMovement, this, _playerProperties, _playerMagic, _playerInventory, escapeCanvas),
                new BuildingPlayerState(_playerMovement, this, _playerProperties, _playerInventory, escapeCanvas, _playerBuild),
                new EatingPlayerState(_playerMovement, this, _playerProperties, _playerInventory, escapeCanvas)
            };
            _currentState = _allStates[0];
            _currentState.Start();
            _mainCamera.transform.RotateAround(transform.position, Vector3.up, 45);
            _playerMovement.UpdateDirections();
            _playerMovement.Speed = 2f;
            _inventoryController.SelectedItemChanged += (sender, e) => 
            {
                CurrentState.OnPlayerSelectedItemChanged(_inventoryController);
            };
            //Initialize health, starvation and temperature:
        }

        private void Update()
        {
            if (_isDead)
                return;
            
            if (Input.GetKeyDown(KeyCode.Escape))
                _currentState.HandleEscapeButton();


            _cameraHolder.transform.position = transform.position;
            _currentState.ContinueStarving();
            _currentState.ContinueFreeze();
            _currentState.LoadForThrow();
            _currentState.SpendStamina();
            _currentState.HandleUserInput();
            _currentState.PrepareForHit();
        }

        private void FixedUpdate()
        {
            if (_isDead)
                return;
            
            if (_playerProperties.CurrentHealth <= 0)
                KillPlayer();

            _currentState.MoveCharacter();
            _animator.SetFloat("Speed", _rigidbody.velocity.magnitude);
            _animator.SetBool("Shift Pressed", Input.GetKey(KeyCode.LeftShift));
        }

        public void ThrowItem()
        {
            _animator.SetTrigger("Throw");
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
            _animator.SetBool("Busy Mode", typeof(T) == typeof(BusyPlayerState));
            var state = _allStates.FirstOrDefault(st => st is T);
            _currentState.Stop();
            state.Start();
            _currentState = state;
            _playerProperties._throwLoadingProgress = _playerProperties.ThrowPrepareTime;
            StateChanged?.Invoke(this, null);
            Debug.Log(typeof(T).ToString());
        }

        internal void Hit()
        {
            //Now it does almost nothing.
            //TODO: make hit logic.
            _animator.SetTrigger("Fist Attack");
        }

        /// <summary>
        /// Kills player, plays animation, blacks out the screen and shows the end menu
        /// </summary>
        private void KillPlayer()
        {
            // TODO: Play the death animation, do the screen blackout and show menu
            _isDead = true;
            deathCanvas.EnableSelf();
        }
    }
}