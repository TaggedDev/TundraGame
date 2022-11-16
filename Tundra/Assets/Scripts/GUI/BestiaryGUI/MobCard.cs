using UnityEngine;
using UnityEngine.UI;

namespace GUI.BestiaryGUI
{
    /// <summary>
    /// Represents the mob card in Bestiary panel
    /// </summary>
    public class MobCard : MonoBehaviour
    {
        [SerializeField] private Image isKilledMask;
        [SerializeField] private Image mobAvatar;
        [SerializeField] private Text mobTitle;
        [SerializeField] private Text mobDescription;
        private int _mobAlphabeticIndex;

        public int MobAlphabeticIndex
        {
            get => _mobAlphabeticIndex;
            set => _mobAlphabeticIndex = value;
        }

        /// <summary>
        /// Sets the card values: avatar, title and description
        /// </summary>
        public void SetCardValues(int index, string mobName, string mobInformation, Sprite avatarSprite, bool isMobKilled)
        {
            _mobAlphabeticIndex = index;
            // Setting the values 
            mobTitle.text = mobName;
            mobDescription.text = mobInformation;
            mobAvatar.sprite = avatarSprite;
            // Enabling killed mask if player has killed this mob
            isKilledMask.gameObject.SetActive(isMobKilled);
        }

        /// <summary>
        /// Enabled IsKilled mask on this mob card
        /// </summary>
        public void SetMobKilled()
        {
            isKilledMask.gameObject.SetActive(true);
        }
    }
}