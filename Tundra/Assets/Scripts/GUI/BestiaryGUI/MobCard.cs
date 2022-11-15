using UnityEngine;
using UnityEngine.UI;

namespace GUI.BestiaryGUI
{
    /// <summary>
    /// Represents the mob card in Bestiary panel
    /// </summary>
    public class MobCard : MonoBehaviour
    {
        [SerializeField] private Image mobAvatar;
        [SerializeField] private Text mobTitle;
        [SerializeField] private Text mobDescription;

        /// <summary>
        /// Sets the card values: avatar, title and description
        /// </summary>
        public void SetCardValues(string mobName, string mobInformation, Sprite avatarSprite)
        {
            mobTitle.text = mobName;
            mobDescription.text = mobInformation;
            mobAvatar.sprite = avatarSprite;
        }
    }
}