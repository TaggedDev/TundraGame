using System.Collections.Generic;
using System.Linq;
using CameraConfiguration;
using Creatures.Player.States;
using UnityEngine;
using System;
using GUI.BestiaryGUI;
using GUI.GameplayGUI;
using Creatures.Mobs;
using Creatures.Player.Inventory;
using Environment.Terrain;
using Creatures.Player.Inventory.ItemConfiguration;

namespace Creatures.Player.Behaviour
{
    public class PlayerBehaviour : MonoBehaviour, IPlayerStateSwitcher
    {
        
        
        // Properties
        public bool IsOverweight => _inventoryController.Inventory.TotalWeight > _playerProperties.MaxLoadCapacity;
        public float OverweightCoefficient => _inventoryController.Inventory.TotalWeight / _playerProperties.MaxLoadCapacity;

        // Variables
        [SerializeField] private EscapeMenu escapeCanvas;
        [SerializeField] private DeathMenu deathCanvas;
        [SerializeField] private GameObject hitPosition;
        [SerializeField] private BestiaryPanel bestiaryPanel;
        private Animator _animator;
        private BasicPlayerState _currentState;
        private PlayerMovement _playerMovement;
        private List<BasicPlayerState> _allStates;
        private CameraMovement _cameraHolder;
        private Camera _mainCamera;
        private PlayerInventory _inventoryController;
        private PlayerProperties _playerProperties;
        private PlayerInventory _playerInventory;
        private PlayerMagic _playerMagic;
        private bool _isDead;
        
        public BasicPlayerState CurrentState => _currentState;
        public event EventHandler StateChanged;

        private void Start()
        {
            _mainCamera = Camera.main;
            _cameraHolder = transform.parent.GetComponentInChildren<CameraMovement>();
            
            _playerMovement = GetComponent<PlayerMovement>();
            _inventoryController = GetComponent<PlayerInventory>();
            _playerProperties = GetComponent<PlayerProperties>();
            _playerInventory = GetComponent<PlayerInventory>();
            _playerMagic = GetComponent<PlayerMagic>();
            GetComponent<PlayerAnimation>();
            _animator = GetComponent<Animator>();
            GetComponent<Rigidbody>();
            GetComponent<Animator>();
            _allStates = new List<BasicPlayerState>
            {
                new IdlePlayerState(_playerMovement, this, _playerProperties, _playerInventory, escapeCanvas, 
                    bestiaryPanel),
                new WalkPlayerState(_playerMovement,  this, _playerProperties, _playerInventory, escapeCanvas, 
                    bestiaryPanel),
                new SprintPlayerState(_playerMovement, this, _playerProperties, _playerInventory, escapeCanvas, 
                    bestiaryPanel),
                new BusyPlayerState(_playerMovement, this, _playerProperties, _playerInventory, escapeCanvas, 
                    bestiaryPanel),
                new MagicCastingPlayerState(_playerMovement, this, _playerProperties, _playerMagic, _playerInventory, 
                    escapeCanvas, bestiaryPanel),
                new BuildingPlayerState(_playerMovement, this, _playerProperties, _playerInventory, escapeCanvas, 
                    bestiaryPanel),
                new WindupHitPlayerState(_playerMovement, this, _playerProperties, _playerInventory, escapeCanvas, 
                    bestiaryPanel)
            };
            _currentState = _allStates[0];
            _currentState.Start();
            _mainCamera.transform.RotateAround(transform.position, Vector3.up, 45);
            _playerMovement.UpdateDirections();
            _playerMovement.Speed = 2f;
            _inventoryController.SelectedItemChanged += (_, __) =>
                CurrentState.OnPlayerSelectedItemChanged(_inventoryController);
            //Initialize health, starvation and temperature:
            _playerInventory.SelectedItemChanged += OnSelectedItemChanged;
            Screen.fullScreen = true;
        }

        private void Update()
        {
            if (_isDead)
                return;

            if (Input.GetKeyDown(KeyCode.Escape))
                _currentState.HandleEscapeButton();

            _cameraHolder.transform.position = transform.position;
            _currentState.ContinueStarving();
            _currentState.ContinueFreezing();
            _currentState.SpendStamina();
            _currentState.HandleUserInput();
            _currentState.Build();
        }

        private void FixedUpdate()
        {
            if (_isDead)
                return;
            
            if (_playerProperties.CurrentHealthPoints <= 0)
                KillPlayer();

            _currentState.MoveCharacter();
        }

        public void SwitchState<T>() where T : BasicPlayerState
        {
            //Debug.Log(_currentState.ToString() + "->" + typeof(T).ToString());
            var state = _allStates.FirstOrDefault(st => st is T);
            _currentState.Stop();
            state.Start();
            _currentState = state;
            StateChanged?.Invoke(this, null);
            
            // Delete later
            
        }
        private void OnSelectedItemChanged(object sender, int arg)
        {
            CurrentState.InventorySelectedSlotChanged(sender, arg);
        }
        /// <summary>
        /// Performs an attack
        /// </summary>
        internal void Hit()
        {
            foreach(Collider hitObject in Physics.OverlapBox(hitPosition.transform.position, new Vector3(0.125f, 0.25f, 0.125f)))
            {
                var mob = hitObject.gameObject.GetComponent<Mob>();
                if (mob != null)
                {
                    mob.CurrentMobHealth -= (_playerInventory.SelectedItem as MeleeWeaponConfiguration).Damage * 
                        Mathf.Lerp(_playerProperties.MinDamageModificator, _playerProperties.MaxDamageModificator, Mathf.Sqrt(4 * _playerProperties.CurrentCircleBarFillingTime / (_playerInventory.SelectedItem as MeleeWeaponConfiguration).FullWindupTime));
                }
                var entity = hitObject.gameObject.GetComponent<Entity>();
                if(entity != null)
                {
                    entity.HealthPoints -= (_playerInventory.SelectedItem as MeleeWeaponConfiguration).Damage *
                        Mathf.Lerp(_playerProperties.MinDamageModificator, _playerProperties.MaxDamageModificator, Mathf.Sqrt(4 * _playerProperties.CurrentCircleBarFillingTime / (_playerInventory.SelectedItem as MeleeWeaponConfiguration).FullWindupTime));
                }
            }
            //Debug.Log("hit " + (Mathf.Lerp(_playerProperties.MinDamageModificator, _playerProperties.MaxDamageModificator, Mathf.Sqrt(4 * _playerProperties.CurrentCircleBarFillingTime / (_playerInventory.SelectedItem as MeleeWeaponConfiguration).FullWindupTime))).ToString());
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
        
        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireCube(hitPosition.transform.position, new Vector3(0.125f, 0.25f, 0.125f));
        }

    }
}