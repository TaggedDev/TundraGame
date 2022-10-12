using Environment;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace GUI.MainMenu
{
    public class LevelPlank : MonoBehaviour
    {
        [SerializeField] private Text levelName;
        [SerializeField] private Text levelCreationDate;
        private int _levelSeed;

        /// <summary>
        /// Sets values on level plank card
        /// </summary>
        /// <param name="worldName">The name of the level</param>
        /// <param name="date">The date of creation of this level</param>
        /// <param name="seed">The saved data of this level</param>
        public void SetPlankValues(string worldName, string date, int seed)
        {
            levelName.text = worldName;
            levelCreationDate.text = date;
            _levelSeed = seed;
        }

        /// <summary>
        /// Sets the world settings to load a scene and switches to game scene. Load the saved items
        /// </summary>
        public void LoadLevel()
        {
            // We set world seed and load the scene with game
            WorldConstants.WorldSeed = _levelSeed;
            SceneManager.LoadScene(5);
        }
    }
}
