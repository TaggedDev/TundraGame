using UnityEngine;

namespace GUI.MainMenu
{
    public class MainMenu_SceneSwitcher : MonoBehaviour
    {
        /// <summary>
        /// Is called when player clicks on 'Start Game' button
        /// </summary>
        public void OnStartGameBTNClicked()
        {
            Debug.Log("Switching to game creation scene");
        }
        
        /// <summary>
        /// Is called when player clicks on 'Load Game' button
        /// </summary>
        public void OnLoadGameBTNClicked()
        {
            Debug.Log("Switching to game loading scene");
        }

        /// <summary>
        /// Is called when player clicks on 'Exit' button
        /// </summary>
        public void OnExitGameBTNClicked()
        {
            Debug.Log("Leaving game...");
            Application.Quit();
        }
    }
}
