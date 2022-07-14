namespace Creatures.Animals.States
{
    public abstract class BasicAnimalState
    {
        private readonly IAnimalStateSwitcher PlayerStateSwitcher;

        protected BasicAnimalState(IAnimalStateSwitcher switcher)
        {
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