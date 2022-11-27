using UnityEngine;
using UnityEngine.UI;

namespace GUI.PlayerInventoryUI
{
    public class UIInventorySlot : MonoBehaviour
    {
        private Image _image;
        
        private void Start()
        {
            _image = GetComponent<Image>();
        }
    }
}