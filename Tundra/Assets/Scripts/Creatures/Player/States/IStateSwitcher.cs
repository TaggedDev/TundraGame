namespace Creatures.Player.States
{
    public interface IStateSwitcher
    {
        void SwitchState<T>() where T : BasicState;
    }
}
