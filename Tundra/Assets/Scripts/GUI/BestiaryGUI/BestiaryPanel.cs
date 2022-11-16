using Bestiary;
using UnityEngine;
using UnityEngine.UI;

namespace GUI.BestiaryGUI
{
    /// <summary>
    /// A bestiary panel of a player 
    /// </summary>
    public class BestiaryPanel : MonoBehaviour
    {
        public BestiaryMob[] Mobs { get; private set; }
        [SerializeField] private GridLayoutGroup panelContent;
        [SerializeField] private MobCard mobCard;
        [SerializeField] private TextAsset jsonFile;
        private MobCard[] _mobCards;


        private void Start()
        {
            BestiaryParser parser = new BestiaryParser(jsonFile);
            Mobs = parser.GetMobList();

            Sprite[] sprites = Resources.LoadAll<Sprite>("Mobs/Avatars");
            _mobCards = new MobCard[Mobs.Length];
            
            for (int i = 0; i < Mobs.Length; i++)
            {
                BestiaryMob mob = Mobs[i];
                MobCard card = Instantiate(mobCard, panelContent.transform);
                Sprite mobAvatar = sprites[i];
                card.SetCardValues(mob.MobName, mob.MobDescription, mobAvatar, mob.IsKilled);
                _mobCards[i] = card;
            }
            
            gameObject.SetActive(false);
        }

        public void OnEnable()
        {
            // Using OnEnable method to check which mobs are already killed to show them slained
            
            // Since OnEnable is called before the Start, we prevent the null exception on scene initialisation 
            if (_mobCards is null)
                return;
            
            // Iterating through mobCards and mobs. The size of them is equal
            for (int i = 0; i < _mobCards.Length; i++)
            {
                var mob = Mobs[i];
                var card = _mobCards[i];
                if (mob.IsKilled)
                    card.SetMobKilled();
            }
        }
    }
}