using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace GUI.MainMenu
{
    /// <summary>
    /// Manager of all saves on user's PC
    /// </summary>
    public class SavesLoader : MonoBehaviour
    {
        [SerializeField] private Text noSavesFoundText;
        [SerializeField] private ScrollRect savesFolder;
        [SerializeField] private VerticalLayoutGroup content;
        [SerializeField] private LevelPlank levelSavePrefab;
        private readonly string savesPath = Application.streamingAssetsPath + "/Worlds/";
        
        private void Start()
        {
            if (Directory.Exists(savesPath) && Directory.GetFiles(savesPath).Length != 0)
            {
                noSavesFoundText.gameObject.SetActive(false);
                savesFolder.gameObject.SetActive(true);
                LoadSaves();
            }
            else
            {
                noSavesFoundText.gameObject.SetActive(true);
                savesFolder.gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// Instantiates the saves cards
        /// </summary>
        private void LoadSaves()
        {
            string[] files = Directory.GetFiles(savesPath, "*.txt");
            foreach (string file in files)
            {
                // Instantiating a prefab of a level card
                LevelPlank card = Instantiate(levelSavePrefab.gameObject, content.gameObject.transform).GetComponent<LevelPlank>();
                
                // In order to get the world name we use the filename  
                string filename = Path.GetFileName(file);
                
                // The data is saved in text files, each in new line using syntax: "key: value\n" 
                StreamReader reader = new StreamReader(file);
                // We read the file, get the first line and access the key to get the timestamp text
                var date = reader.ReadLine().Split(':')[1];
                // We read the next line and get the key to get the world seed
                var seed = int.Parse(reader.ReadLine().Split(':')[1]);

                string data = $"SaveStamp:{date}\nWorldSeed:{seed}\n" + reader.ReadToEnd();
                
                // Setting card values
                card.SetPlankValues(filename.Substring(0, filename.Length-4), date, seed, data);
            }
        }
    }
}
