using System;
using Environment;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace GUI
{
    public class CreateMenu_WorldGenerator : MonoBehaviour
    {
        [SerializeField] private Text worldNameInput;
        [SerializeField] private Text worldSeedInput;

        private void Start()
        {
            if (worldNameInput is null)
                throw new Exception("World name input field is not assigned");
            if (worldSeedInput is null)
                throw new Exception("World seed input field is not assigned");
        }

        public void CreateWorld()
        {
            string worldName = worldNameInput.text;
            
            // Check if player didn't put value in seed placeholder
            int worldSeed;
            if (string.IsNullOrEmpty(worldSeedInput.text))
                worldSeed = 146;
            else
                worldSeed = int.Parse(worldSeedInput.text); 
            
            WorldConstants.WorldName = worldName;
            WorldConstants.WorldSeed = worldSeed;
            SceneManager.LoadScene(5);
        }
    }
}
