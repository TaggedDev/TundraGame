using System;

namespace Creatures.Player.Inventory
{
    /// <summary>
    /// A class to provide event args to <see cref="InventoryContainer.ContentChanged"/> event etc.
    /// </summary>
    public class ItemChangeArgs : EventArgs
    {
        /// <summary>
        /// Slot in which was an update of item.
        /// </summary>
        public int SlotID { get; }

        /// <summary>
        /// Old item was in this sot before change.
        /// </summary>
        public BasicItemConfiguration PreviousItem { get; }

        public ItemChangeArgs(int slotID, BasicItemConfiguration previousItem)
        {
            SlotID=slotID;
            PreviousItem=previousItem;
        }
    }
}