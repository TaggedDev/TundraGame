using Creatures.Animals.Behaviour;

namespace Creatures.Animals.States
{
    public class ChasingAnimalState : BasicAnimalState
    {
        public ChasingAnimalState(AnimalMovement animalMovement, IAnimalStateSwitcher switcher) : base(animalMovement, switcher)
        { }

        public override void Start()
        {
            throw new System.NotImplementedException();
        }

        public override void Stop()
        {
            throw new System.NotImplementedException();
        }

        public override void MoveCharacter()
        {
            throw new System.NotImplementedException();
        }
    }
}