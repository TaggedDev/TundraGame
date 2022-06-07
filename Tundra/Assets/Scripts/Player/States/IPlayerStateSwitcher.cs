using AssemblyCSharp.Assets.Scripts.States;

namespace AssemblyCSharp.Assets.Scripts.Player.States
{
    public interface IPlayerStateSwitcher
    {
        void SwitchState<T>() where T : BasicState;
    }
}
