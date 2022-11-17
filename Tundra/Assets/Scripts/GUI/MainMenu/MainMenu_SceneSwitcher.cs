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
        /// Loads level by it's ID in Build Settings
        /// </summary>
        /// <param name="id">ID of the scene in Build Settings</param>
        public void LoadLevelByID(int id)
        {
            SceneManager.LoadScene(id);
        }
    }
}
