using System.Collections.Generic;
using System.Linq;
using Creatures.Animals.States;
using UnityEngine;

namespace Creatures.Animals.Behaviour
{
    public class AnimalBehaviour : MonoBehaviour, IAnimalStateSwitcher
    {
        private BasicAnimalState _currentAnimalState;
        private AnimalMovement _animalMovement;
        private List<BasicAnimalState> _allStates;

        private void Start()
        {
            //cameraDistance = Vector3.Distance(Camera.main.transform.position, transform.position);
            _animalMovement = GetComponent<AnimalMovement>();
            _allStates = new List<BasicAnimalState>
            {
                new PatrolAnimalState(_animalMovement, this),
                new ChasingAnimalState(_animalMovement, this),
                new EscapingAnimalState(_animalMovement, this)
            };
            _currentAnimalState = _allStates[0];
            _currentAnimalState.Start();
        }

        private void Update()
        {
            MoveCharacter();
        }

        private void MoveCharacter() => _currentAnimalState.MoveCharacter();

        public void SwitchState<T>() where T : BasicAnimalState
        {
            var state = _allStates.FirstOrDefault(st => false);
            _currentAnimalState.Stop();
            state?.Start();
            _currentAnimalState = state;
        }
    }
}