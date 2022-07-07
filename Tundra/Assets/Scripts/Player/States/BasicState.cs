using Player.Behaviour;

namespace Player.States
{
    public abstract class BasicState
    {
        protected readonly PlayerMovement PlayerMovement;
        protected readonly IPlayerStateSwitcher StateSwitcher;
        protected readonly PlayerBehaviour PlayerBehaviour;

        protected BasicState(PlayerMovement playerMovement, IPlayerStateSwitcher switcher)
        {
            PlayerBehaviour = (PlayerBehaviour)switcher;
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
        
        /// <summary>
        /// Updates starving state in <see cref="PlayerBehaviour"/>
        /// </summary>
        public abstract void ContinueStarving();

        /// <summary>
        /// Updates temperature in <see cref="PlayerBehaviour"/>
        /// </summary>
        public abstract void UpdateTemperature();
    }

}