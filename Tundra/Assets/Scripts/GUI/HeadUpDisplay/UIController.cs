using UnityEngine;

namespace GUI.HeadUpDisplay
{
    /// <summary>
    /// A script to keep shared data of all HUD scripts.
    /// </summary>
    public class UIController : MonoBehaviour
    {
        [SerializeField] private GameObject player;

        /// <summary>
        /// A <see cref="PocketCraftUI"/> instance.
        /// </summary>
        public static PocketCraftUI PocketCraftUI { get; set; }

        /// <summary>
        /// A <see cref="CraftPanelUI"/> instance.
        /// </summary>
        public static CraftPanelUI CraftPanel { get; set; }

        /// <summary>
        /// A HUD root canvas instance.
        /// </summary>
        public static GameObject RootCanvas => RootUIInstance.gameObject;

        /// <summary>
        /// Instance of the root canvas script.
        /// </summary>
        public static UIController RootUIInstance { get; private set; }

        /// <summary>
        /// A player <see cref="GameObject"/> instance.
        /// </summary>
        public GameObject Player => player;

        private void Awake()
        {
            RootUIInstance = this;
            PocketCraftUI = GetComponentInChildren<PocketCraftUI>();
            PocketCraftUI.gameObject.SetActive(false);
        }
    }
}