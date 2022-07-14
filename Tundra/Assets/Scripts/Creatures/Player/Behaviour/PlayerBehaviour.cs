using System.Collections.Generic;
using System.Linq;
using Creatures.Player.States;
using UnityEngine;

namespace Creatures.Player.Behaviour
{
    public class PlayerBehaviour : MonoBehaviour, IPlayerStateSwitcher
    {
        private BasicPlayerState _currentPlayerState;
        private PlayerMovement _playerMovement;
        private List<BasicPlayerState> _allStates;

        private void Start()
        {
            //cameraDistance = Vector3.Distance(Camera.main.transform.position, transform.position);
            _playerMovement = GetComponent<PlayerMovement>();
            _allStates = new List<BasicPlayerState>()
            {
                new IdlePlayerState(_playerMovement, this),
                new WalkingPlayerState(_playerMovement,  this),
                new SprintingPlayerState(_playerMovement, this),
            };
            _currentPlayerState = _allStates[0];
            _currentPlayerState.Start();
            _playerMovement.UpdateDirections();
        }

        private void Update()
        {
            MoveCharacter();
        }

        private void MoveCharacter() => _currentPlayerState.MoveCharacter();

        public void SwitchState<T>() where T : BasicPlayerState
        {
            var state = _allStates.FirstOrDefault(st => st is T);
            _currentPlayerState.Stop();
            state.Start();
            _currentPlayerState = state;
        }
    }
}
