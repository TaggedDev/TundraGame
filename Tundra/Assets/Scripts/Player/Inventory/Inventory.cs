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
		public Slot[] Slots { get; private set; }

        //TODO: организовать сохранение предметов и т.д
		public void AddItem(Item item, int count)
        {
			
        }

        private Slot PutNewItem(Item item, int count)
        {
            throw new NotImplementedException();
        }
    }
}
