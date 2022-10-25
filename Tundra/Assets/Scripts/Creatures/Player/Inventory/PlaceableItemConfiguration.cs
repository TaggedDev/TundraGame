using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Environment;

namespace Creatures.Player.Inventory
{
    [CreateAssetMenu(fileName = "New Placeable Configuration", menuName = "Items/Placable Configuration")]
    public class PlaceableItemConfiguration : BasicItemConfiguration
    {
        private PlaceableObject _representingItem;

        public PlaceableObject RepresentingItem { get => _representingItem; }
    }
}
