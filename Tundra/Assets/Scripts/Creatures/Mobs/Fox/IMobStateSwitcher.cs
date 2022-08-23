namespace Creatures.Mobs.Fox
{
    public interface IMobStateSwitcher
    {
        void SwitchState<T>() where T : MobBasicState;
    }
}