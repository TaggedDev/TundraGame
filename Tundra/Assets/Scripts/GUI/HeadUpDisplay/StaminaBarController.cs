using Creatures.Player.Behaviour;
using UnityEngine;
using UnityEngine.UI;

namespace GUI.HeadUpDisplay
{
    public class StaminaBarController : MonoBehaviour
    {
        private GameObject _player;
        private PlayerProperties _playerProperties;
        private Image _imageComponent;
        private Camera _mainCamera;

        // Start is called before the first frame update
        private void Start()
        {
            _mainCamera = Camera.main;
            _player = UIController.RootCanvas.GetComponent<UIController>().Player;
            _imageComponent = GetComponent<Image>();
            _playerProperties = _player.GetComponent<PlayerProperties>();
        }

        // Update is called once per frame
        private void Update()
        {
            float displayValue = _playerProperties.CurrentStaminaPoints / _playerProperties.MaxStaminaPoints;
            _imageComponent.fillAmount = displayValue;
        }

        private void FixedUpdate()
        {
            transform.position = RectTransformUtility.WorldToScreenPoint(_mainCamera, _player.transform.position) - new Vector2(0, 50);
        }
    }
}
