using Player.Behaviour;

namespace Player.States
{
    public abstract class BasicState
    {
        protected readonly PlayerMovement PlayerMovement;
        protected readonly IPlayerStateSwitcher StateSwitcher;

        protected BasicState(PlayerMovement playerMovement, IPlayerStateSwitcher switcher)
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