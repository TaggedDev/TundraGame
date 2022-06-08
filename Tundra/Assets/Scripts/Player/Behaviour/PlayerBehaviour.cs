using System.Collections.Generic;
using System.Linq;
using Player.States;
using UnityEngine;

namespace Player.Behaviour
{
    public class PlayerBehaviour : MonoBehaviour, IPlayerStateSwitcher
    {
        private BasicState _currentState;
        private PlayerMovement _playerMovement;
        private List<BasicState> _allStates;
        //private float cameraDistance;

        [SerializeField]
        private float cameraRotationSpeed = 1;
        [SerializeField]
        private GameObject cameraHolder;

        private void Start()
        {
            _playerMovement = GetComponent<PlayerMovement>();
            //cameraDistance = Vector3.Distance(Camera.main.transform.position, transform.position);
            _allStates = new List<BasicState>()
            {
                new IdleState(_playerMovement, this),
                new WalkingState(_playerMovement,  this),
                new SprintingState(_playerMovement, this),
            };
            _currentState = _allStates[0];
            _currentState.Start();
            UnityEngine.Camera.main.transform.RotateAround(transform.position, Vector3.up, 45);
            _playerMovement.UpdateDirections();
        }

        private void Update()
        {
            MoveCharacter();
            cameraHolder.transform.position = transform.position;
            RotateCamera();
        }

        public void MoveCharacter() => _currentState.MoveCharacter();

        public void SwitchState<T>() where T : BasicState
        {
            BasicState state = _allStates.FirstOrDefault(st => st is T);
            _currentState.Stop();
            state.Start();
            _currentState = state;
        }

        public void RotateCamera()
        {
            float rot = 0;
            if (Input.GetKey(KeyCode.Q)) rot -= 1;
            if (Input.GetKey(KeyCode.E)) rot += 1;
            UnityEngine.Camera.main.transform.RotateAround(transform.position, Vector3.up, rot * cameraRotationSpeed);
            _playerMovement.UpdateDirections();
        }
    }
}
