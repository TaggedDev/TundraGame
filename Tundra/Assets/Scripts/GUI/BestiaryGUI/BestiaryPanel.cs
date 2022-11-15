using Bestiary;
using UnityEngine;
using UnityEngine.UI;

namespace GUI.BestiaryGUI
{
    public class BestiaryPanel : MonoBehaviour
    {
        [SerializeField] private GridLayoutGroup panelContent;
        [SerializeField] private MobCard mobCard;
        [SerializeField] private TextAsset jsonFile;
        
        private void Start()
        {
            BestiaryParser parser = new BestiaryParser(jsonFile);
            var mobs = parser.GetMobList();

            foreach (var mob in mobs)
            {
                var card = Instantiate(mobCard, panelContent.transform);
                card.SetCardValues(mob.mobName, mob.mobDescription);
            }
        }
    }
}