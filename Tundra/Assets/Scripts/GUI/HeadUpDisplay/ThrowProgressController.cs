using Creatures.Player.Behaviour;
using GUI.HeadUpDisplay;
using UnityEngine;
using UnityEngine.UI;

namespace GUI.HeadUpDisplay
{
    public class ThrowProgressController : MonoBehaviour
    {
        private GameObject _player;

        private PlayerProperties properties;
        private Image progressBar;


        // Start is called before the first frame update
        private void Start()
        {
            _player = UIController._rootCanvas.GetComponent<UIController>()._player;
            properties = _player.GetComponent<PlayerProperties>();
            progressBar = GetComponent<Image>();
        }

        // Update is called once per frame
        private void Update()
        {
            progressBar.fillAmount = (properties.ThrowPrepareTime - properties._throwLoadingProgress) / properties.ThrowPrepareTime;
        }

        private void FixedUpdate()
        {
            transform.position = RectTransformUtility.WorldToScreenPoint(Camera.main, _player.transform.position) + new Vector2(0, 80);
        }
    }
}
