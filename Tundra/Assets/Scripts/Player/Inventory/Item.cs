using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Player.Inventory
{
    public class Item : ScriptableObject
    {
		public Sprite Icon { get; private set; }

        public string Title { get; private set; }

        public string Description { get; private set; }

        public virtual void PutToInventory(Inventory inventory, int count, Func<int, Slot> putNewItem)
        {
            
            for (int i = 0; i < count; i++)
            {
                var state = putNewItem(1);

                if (state == null)
                {
                    return;
                }
            }
        }
    }
}
