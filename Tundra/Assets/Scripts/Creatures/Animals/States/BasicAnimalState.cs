using Creatures.Animals.Behaviour;
using Creatures.Player;

namespace Creatures.Animals.States
{
    public abstract class BasicAnimalState
    {
        protected readonly AnimalMovement AnimalMovement;
        protected readonly IAnimalStateSwitcher PlayerStateSwitcher;

        protected BasicAnimalState(AnimalMovement animalMovement, IAnimalStateSwitcher switcher)
        {
            AnimalMovement = animalMovement;
            PlayerStateSwitcher = switcher;
        }

        /// <summary>
        /// On State changed | Start
        /// </summary>
        public abstract void Start();

        /// <summary>
        /// On State changed | Stop
        /// </summary>
        public abstract void Stop();

        /// <summary>
        /// Basic movement with sprint
        /// </summary>
        public abstract void MoveCharacter();
    }
}