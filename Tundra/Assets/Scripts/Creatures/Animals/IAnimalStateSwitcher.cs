using Creatures.Animals.States;

namespace Creatures.Animals
{
    public interface IAnimalStateSwitcher
    {
        void SwitchState<T>() where T : BasicAnimalState;
    }
}