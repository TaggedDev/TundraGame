using System;
using System.Collections.Generic;
using System.IO;
using Creatures.Player.Behaviour;
using Environment;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace GUI.MainMenu
{
    /// <summary>
    /// Describes the behaviour of buttons in Escape Menu
    /// </summary>
    public class EscapeMenu : MonoBehaviour
    {
        // Link to a button that is used to save the current world 
        [SerializeField] private Button saveButton;
        [SerializeField] private PlayerBehaviour player;
        [SerializeField] private LoadOrder loadOrder;

        private void Start()
        {
            // Creates directory of StreamingAssets with inner folder Worlds in it
            Directory.CreateDirectory(Application.streamingAssetsPath + "/Worlds/");
        }

        /// <summary>
        /// Generates world save or overwrites it
        /// </summary>
        public void SaveThisWorld()
        {
            // A path to world save object
            string worldSavePath = Application.streamingAssetsPath + "/Worlds/" + WorldConstants.WorldName + ".txt";

            string saveContent = CollectSaveData();
            // We rewrite the text in save file.
            File.WriteAllText(worldSavePath, saveContent);

            // Disabling button to prevent save spam
            saveButton.interactable = false;
        }

        /// <summary>
        /// Collects and translates in txt data to save
        /// </summary>
        /// <returns>A string to write in text file</returns>
        private string CollectSaveData()
        {
            List<SaveData> saveDatum = new List<SaveData>
            {
                new SaveData("SaveStamp", $"{DateTime.Today.Day}.{DateTime.Today.Month}.{DateTime.Today.Year}"),
                new SaveData("WorldSeed", WorldConstants.WorldSeed),
                new SaveData("PlayerPosition", player.transform.localPosition)
            };

            string saveText = string.Empty;
            
            foreach (var saveData in saveDatum)
                saveText += $"{saveData.GenerateSaveText()}\n";
            
            return saveText;
        }
    
        /// <summary>
        /// Called when player pushes the exit to menu button
        /// </summary>
        public void ExitToMainMenu()
        {
            loadOrder.UnloadCurrentScene();
            SceneManager.LoadScene(0);
        }
    }

    /// <summary>
    /// Class describes the behaviour of fields to be saved in world save file
    /// </summary>
    internal class SaveData
    {
        private readonly string _fieldName;
        private readonly string _fieldValue;
        
        /// <summary>
        /// Generates the object of save data
        /// </summary>
        /// <param name="name">The name of the saved value</param>
        /// <param name="value">The value to save</param>
        public SaveData(string name, object value)
        {
            _fieldName = name;
            _fieldValue = value.ToString();
        }

        /// <summary>
        /// Generates the text that has to be written in save file
        /// </summary>
        /// <returns>The text that has to be written in save file</returns>
        public string GenerateSaveText()
        {
            return $"{_fieldName}:{_fieldValue}";
        }
    }
    
}
