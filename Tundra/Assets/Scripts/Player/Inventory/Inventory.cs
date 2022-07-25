using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Player.Inventory
{
    public class Inventory : MonoBehaviour
    {
        public int MaxInventoryCapacity { get; private set; }

		public Slot[] Slots { get; private set; }

        //TODO: организовать сохранение предметов и т.д
        public bool AddItem(BasicItemConfiguration item, int amount, out int rem)
        {
            while (amount > 0)
            {
                Slot slot = FindNearestSlot(item, amount, out int remainder);
                if (slot != null)
                {
                    if (remainder > 0)
                    {
                        slot.Fill(item);
                    }
                    else slot.PushItem(item, amount);
                    amount = remainder;
                }
                else
                {
                    rem = remainder;
                    return false;
                }
            }
            rem = 0;
            return true;
        }

        public List<Slot> FindSlotsWithItem(BasicItemConfiguration item)
        {
            List<Slot> slots = Slots.Where(x => x.Item == item).ToList();
            return slots;
        }

        public int CountItemOfTypeInTheInventory(BasicItemConfiguration item)
        {
            return Slots.Aggregate(0, (x, y) => x += y.Item == item ? y.ItemsAmount : 0);//Исхожу из ситуации, что ItemConfiguration существует в единственном экземпляре для каждого предмета
        }

        private Slot FindNearestSlot(BasicItemConfiguration item, int amount, out int remainder)
        {
            remainder = 0;
            Slot found = Slots.FirstOrDefault(x => x.IsEmpty || x.Item == item && !x.IsFull);
            if (found != null)
            {
                remainder = found.ItemsAmount + amount - Slot.MaxStackVolume;
                return found;
            }
            remainder = amount;
            return null;
        }

        private void Start()
        {
            Slots = new Slot[MaxInventoryCapacity];
        }
    }
}
