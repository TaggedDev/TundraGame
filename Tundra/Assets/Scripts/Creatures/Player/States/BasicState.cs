using Creatures.Player.Behaviour;

namespace Creatures.Player.States
{
    public abstract class BasicState
    {
        protected readonly PlayerMovement PlayerMovement;
        protected readonly IStateSwitcher StateSwitcher;

        protected BasicState(PlayerMovement playerMovement, IStateSwitcher switcher)
        {
            this.PlayerMovement = playerMovement;
            StateSwitcher = switcher;
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