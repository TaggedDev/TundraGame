using Creatures.Player.Behaviour;

namespace Creatures.Player.States
{
    public abstract class BasicPlayerState
    {
        protected readonly PlayerMovement PlayerMovement;
        protected readonly IPlayerStateSwitcher PlayerStateSwitcher;
        protected readonly PlayerBehaviour PlayerBehaviour;
        protected readonly PlayerProperties PlayerProperties;

        protected BasicPlayerState(PlayerMovement playerMovement, IPlayerStateSwitcher switcher, PlayerProperties playerProperties)
        {
            PlayerBehaviour = (PlayerBehaviour)switcher;
            PlayerMovement = playerMovement;
            PlayerStateSwitcher = switcher;
            PlayerMovement = playerMovement;
            PlayerProperties = playerProperties;
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

        /// <summary>
        /// Loads weapon for throwing.
        /// </summary>
        public abstract void LoadForThrow();
    }

}