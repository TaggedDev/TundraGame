using System;

namespace Creatures.Player.Inventory
{
    public class ItemChangeArgs : EventArgs
    {
        public int SlotID { get; private set; }

        public BasicItemConfiguration PreviousItem { get; private set; }

        public ItemChangeArgs(int slotID, BasicItemConfiguration previousItem)
        {
            SlotID=slotID;
            PreviousItem=previousItem;
        }
    }
}