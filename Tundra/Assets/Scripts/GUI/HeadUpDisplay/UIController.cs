using UnityEngine;

namespace GUI.HeadUpDisplay
{
    public class UIController : MonoBehaviour
    {
        internal static GameObject _rootCanvas;

        [SerializeField] internal GameObject _player;

        private void Awake()
        {
            _rootCanvas = gameObject;
        }
    }
}
