using UnityEngine;
using UnityEngine.UI;

namespace GUI.PlayerInventoryUI
{
    public class UISlot : MonoBehaviour
    {
        private Image _image;
        
        private void Start()
        {
            _image = GetComponent<Image>();
        }
    }
}