
using Creatures.Player.Inventory;
using UnityEngine;

namespace Creatures.Player.Crafts
{
    public class PlaceableObjectBehaviour : MonoBehaviour
    {
        [SerializeField]
        private PlaceableItemConfiguration configuration;
        [SerializeField]
        private bool canBeOpened;

        public PlaceableItemConfiguration Configuration => configuration;

        public bool CanBeOpened => canBeOpened;
    }
}