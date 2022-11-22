using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GUI.MainMenu
{
    /// <summary>
    /// A handler for scene switching in Main Menu scene
    /// </summary>
    public class MainMenu_SceneSwitcher : MonoBehaviour
    {
        /// <summary>
        /// Is called when player clicks on 'Exit' button
        /// </summary>
        public void OnExitGameBTNClicked()
        {
            Debug.Log("Leaving game...");
            Application.Quit();
        }
        
        /// <summary>
        /// Loads LoadLevel Scene
        /// </summary>
        public void LoadLoadLevelScene()
        {
            SceneManager.LoadScene(Convert.ToInt32(TundraScenes.LOAD_GAME_SCENE_ID));
        }
        
        /// <summary>
        /// Loads CreateLevel scene
        /// </summary>
        public void LoadCreateLevelScene()
        {
            SceneManager.LoadScene(Convert.ToInt32(TundraScenes.CREATE_GAME_SCENE_ID));
        }
    }
}
