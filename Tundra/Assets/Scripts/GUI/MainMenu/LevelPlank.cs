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
        private string _levelData;

        /// <summary>
        /// Sets values on level plank card
        /// </summary>
        /// <param name="worldName">The name of the level</param>
        /// <param name="date">The date of creation of this level</param>
        /// <param name="seed">The saved data of this level</param>
        /// <param name="data">The data to pass in world constants</param>
        public void SetPlankValues(string worldName, string date, int seed, string data)
        {
            levelName.text = worldName;
            levelCreationDate.text = date;
            _levelSeed = seed;
            _levelData = data;
        }

        /// <summary>
        /// Sets the world settings to load a scene and switches to game scene. Load the saved items
        /// </summary>
        public void LoadLevel()
        {
            // We set world seed and load the scene with game
            WorldConstants.WorldName = levelName.text;
            WorldConstants.WorldSeed = _levelSeed;
            WorldConstants.WorldData = _levelData; 
            SceneManager.LoadScene(5);
        }
    }
}
