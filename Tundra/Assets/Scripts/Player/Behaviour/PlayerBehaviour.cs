using System.Collections.Generic;
using System.Linq;
using CameraConfiguration;
using Player.States;
using UnityEngine;

namespace Player.Behaviour
{
    public class PlayerBehaviour : MonoBehaviour, IPlayerStateSwitcher
    {
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
        }

        private void Update()
        {
            MoveCharacter();
            _cameraHolder.transform.position = transform.position;
        }

        private void MoveCharacter() => _currentState.MoveCharacter();

        public void SwitchState<T>() where T : BasicState
        {
            var state = _allStates.FirstOrDefault(st => st is T);
            _currentState.Stop();
            state.Start();
            _currentState = state;
        }
    }
}
