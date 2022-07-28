using Creatures.Player.Behaviour;

namespace Creatures.Player.States
{
    public abstract class BasicPlayerState
    {
        protected readonly PlayerMovement PlayerMovement;
        protected readonly IPlayerStateSwitcher PlayerStateSwitcher;

        protected BasicPlayerState(PlayerMovement playerMovement, IPlayerStateSwitcher switcher)
        {
            PlayerMovement = playerMovement;
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