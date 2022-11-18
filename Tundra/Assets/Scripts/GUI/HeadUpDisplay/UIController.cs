using UnityEngine;

namespace GUI.HeadUpDisplay
{
    public class UIController : MonoBehaviour
    {
        [SerializeField] internal GameObject _player;
        internal static GameObject _rootCanvas;
        
        private void Awake()
        {
            _rootCanvas = gameObject;
        }
    }
}