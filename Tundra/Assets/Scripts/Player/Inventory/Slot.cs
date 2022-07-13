namespace Player.Inventory
{
    public class Slot
    {
        public const int MaxStackVolume = 5;

        public bool IsEmpty => ItemsAmount == 0;

        public bool IsFull => ItemsAmount == MaxStackVolume;

        public Item Item { get; internal set; }

        public int ItemsAmount { get; internal set; }

        //TODO: сделать управление предметами в слоте.

        public void Clear()
        {
            ItemsAmount = 0;
            Item = null;
        }
    }
}