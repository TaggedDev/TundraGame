
namespace Creatures.Player.Magic
{
    public class SpellingElementSlot
    {
        private float _reloadCooldown;

        public MagicElement Element { get; set; }

        public int MaxStoneAmount { get; set; }

        public int CurrentStonesAmount { get; set; }

        public float StoneReloadTime { get; set; }

        public SpellingElementSlot(MagicElement element, int maxAmount, float reloadTime)
        {
            Element = element;
            CurrentStonesAmount = MaxStoneAmount = maxAmount;
            StoneReloadTime = reloadTime;
        }

        public void UpdateReloadState(float deltaTime)
        {
            if (CurrentStonesAmount < MaxStoneAmount)
            {
                _reloadCooldown -= deltaTime;
                if (_reloadCooldown <= 0)
                {
                    CurrentStonesAmount++;
                    _reloadCooldown = StoneReloadTime;
                }
            }
        }
    }

}
