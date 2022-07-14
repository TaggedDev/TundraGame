using System.Collections.Generic;
using System.Linq;
using Creatures.Player.Behaviour;
using Creatures.Player.States;
using UnityEngine;

namespace Creatures.Animals.Behaviour
{
    public class AnimalBehaviour : MonoBehaviour, IStateSwitcher
    {
        private BasicState _currentState;
        private PlayerMovement _playerMovement;
        private List<BasicState> _allStates;

        private void Start()
        {
            //cameraDistance = Vector3.Distance(Camera.main.transform.position, transform.position);
            _playerMovement = GetComponent<PlayerMovement>();
            _allStates = new List<BasicState>()
            {
                new IdleState(_playerMovement, this),
                new WalkingState(_playerMovement, this),
                new SprintingState(_playerMovement, this),
            };
            _currentState = _allStates[0];
            _currentState.Start();
            _playerMovement.UpdateDirections();
        }

        private void Update()
        {
            MoveCharacter();
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