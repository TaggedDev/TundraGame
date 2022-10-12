using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace GUI.MainMenu
{
    public class SavesLoader : MonoBehaviour
    {
        [SerializeField] private Text noSavesFoundText;
        [SerializeField] private GameObject savesFolder;
        private readonly string savesPath = Application.streamingAssetsPath + "/Worlds/";
    
        // Start is called before the first frame update
        private void Start()
        {
            if (Directory.Exists(savesPath) && Directory.GetFiles(savesPath).Length != 0)
            {
                noSavesFoundText.gameObject.SetActive(false);
                savesFolder.SetActive(true);
            }
            else
            {
                noSavesFoundText.gameObject.SetActive(true);
                savesFolder.SetActive(false);
            }
        }
    }
}
