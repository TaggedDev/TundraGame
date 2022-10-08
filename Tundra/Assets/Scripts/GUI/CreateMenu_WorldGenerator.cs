using System;
using UnityEngine;
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
            int worldSeed = int.Parse(worldSeedInput.text);
            Debug.Log($"{worldName}, {worldSeed}");
        }
    }
}
