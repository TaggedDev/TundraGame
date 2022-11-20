
using Creatures.Player.Inventory;
using UnityEngine;

namespace Creatures.Player.Crafts
{
    /// <summary>
    /// A script to handle placeable objects logic.
    /// </summary>
    public class PlaceableObjectBehaviour : MonoBehaviour
    {
        [SerializeField]
        private PlaceableItemConfiguration configuration;
        [SerializeField]
        private bool canBeOpened;

        /// <summary>
        /// A <see cref="PlaceableItemConfiguration"/> for this object.
        /// </summary>
        public PlaceableItemConfiguration Configuration => configuration;

        /// <summary>
        /// Indicates if this object can be opened with an interaction key.
        /// </summary>
        public bool CanBeOpened => canBeOpened;
    }
}