using Creatures.Player.States;

namespace Creatures.Player
{
    public interface IPlayerStateSwitcher
    {
        void SwitchState<T>() where T : BasicPlayerState;
    }
}
