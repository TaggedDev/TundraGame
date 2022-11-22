using System;
using GUI.MainMenu;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GUI.GameplayGUI
{
    public class DeathMenu : MonoBehaviour
    {
        private void Start()
        {
            gameObject.SetActive(false);
        }

        /// <summary>
        /// Turns on this gameobject
        /// </summary>
        public void EnableSelf()
        {
            gameObject.SetActive(true);
        }
        
        /// <summary>
        /// Is called when 'To Main Menu' button is pressed on death menu 
        /// </summary>
        public void ToMainMenu()
        {
            SceneManager.LoadScene(Convert.ToInt32(TundraScenes.MAIN_MENU_SCENE_ID));
        }
    }
}