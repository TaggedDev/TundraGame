using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GUI.GameplayGUI
{
    public class UIController : MonoBehaviour
    {
        [SerializeField] 
        private GameObject player;

        public static PocketCraftUI PocketCraftUI { get; set; }
        public static CraftPanelUI CraftPanel { get; set; }
        public static GameObject RootCanvas => RootUIInstance.gameObject;

        public static UIController RootUIInstance { get; private set; }
        public GameObject Player { get => player; set => player=value; }

        private void Awake()
        {
            RootUIInstance = this;
            PocketCraftUI = GetComponentInChildren<PocketCraftUI>();
            PocketCraftUI.gameObject.SetActive(false);
        }
    }
}