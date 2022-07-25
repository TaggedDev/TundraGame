namespace Creatures.Mobs
{
    public interface IMobStateSwitcher
    {
        void SwitchState<T>() where T : MobBasicState;
    }
}