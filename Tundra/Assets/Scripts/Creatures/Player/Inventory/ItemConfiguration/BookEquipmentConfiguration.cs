using Creatures.Player.Magic;
using UnityEngine;

namespace Creatures.Player.Inventory.ItemConfiguration
{
    [CreateAssetMenu(fileName = "New Equipment Configuration", menuName = "Items/Equipment/Book Configuration")]
    public class BookEquipmentConfiguration : EquipmentConfiguration
    {
        [SerializeField]
        private MagicElementSlot[] _magicElements;
        [SerializeField]
        private int _freeSheets;

        public MagicElementSlot[] MagicElements { get => _magicElements; set => _magicElements=value; }
        public int FreeSheets { get => _freeSheets; set => _freeSheets=value; }

        public void ReloadStones()
        {
            foreach (var slot in MagicElements)
            {
                slot.UpdateReloadState(Time.deltaTime);
            }
        }
    }
}
