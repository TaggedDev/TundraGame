namespace Creatures.Mobs.Wolf
{
    public interface IMobStateSwitcher
    {
        void SwitchState<T>() where T : MobBasicState;
    }
}