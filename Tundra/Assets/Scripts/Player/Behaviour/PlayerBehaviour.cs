using AssemblyCSharp.Assets.Scripts.Behaviour;
using AssemblyCSharp.Assets.Scripts.Player.States;
using AssemblyCSharp.Assets.Scripts.States;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AssemblyCSharp.Assets.Scripts.Player
{
    public class PlayerBehaviour : MonoBehaviour, IPlayerStateSwitcher
    {
        private BasicState _currentState;
        private PlayerMovement _playerMovement;
        private List<BasicState> _allStates;

        private void Start()
        {
            _playerMovement = GetComponent<PlayerMovement>();

            _allStates = new List<BasicState>()
            {
                new IdleState(_playerMovement, this),
                new WalkingState(_playerMovement,  this),
                new SprintingState(_playerMovement, this),
            };
            _currentState = _allStates[0];
            _currentState.Start();
        }

        private void Update()
        {
            MoveCharacter();
        }

        public void MoveCharacter() => _currentState.MoveCharacter();

        public void SwitchState<T>() where T : BasicState
        {
            BasicState state = _allStates.FirstOrDefault(st => st is T);
            _currentState.Stop();
            state.Start();
            _currentState = state;
        }
    }
}
