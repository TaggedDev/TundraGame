
namespace Player.States
{
    public interface IPlayerStateSwitcher
    {
        void SwitchState<T>() where T : BasicState;
    }
}
