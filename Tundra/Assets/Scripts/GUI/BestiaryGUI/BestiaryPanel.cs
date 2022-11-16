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

            Sprite[] sprites = Resources.LoadAll<Sprite>("Mobs/Avatars");
            
            for (int i = 0; i < mobs.Length; i++)
            {
                BestiaryMob mob = mobs[i];
                MobCard card = Instantiate(mobCard, panelContent.transform);
                Sprite mobAvatar = sprites[i];
                card.SetCardValues(mob.mobName, mob.mobDescription, mobAvatar, mob.isKilled);
            }
            
            gameObject.SetActive(false);
        }
    }
}