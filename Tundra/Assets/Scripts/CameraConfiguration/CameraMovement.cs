using Creatures.Player.Behaviour;
using UnityEngine;

namespace CameraConfiguration
{
    public class CameraMovement : MonoBehaviour
    {
        private Camera _mainCamera;
        private PlayerMovement _playerMovement;
        [SerializeField] private GameObject _focusObject;
        [SerializeField] private float maxCameraRotationCooldown;
        private float _currentCameraRotationCooldown;

        private void Start()
        {
            _mainCamera = Camera.main;
            _playerMovement = transform.parent.GetComponentInChildren<PlayerMovement>();
        }
        
        private void Update()
        {
            var pos = _focusObject.transform.position;
            transform.position = new Vector3(pos.x, pos.y, pos.z - 3);
            if (Input.GetKey(KeyCode.Q)) RotateCamera(true);
            if (Input.GetKey(KeyCode.E)) RotateCamera(false);
            if (_currentCameraRotationCooldown > 0) _currentCameraRotationCooldown -= Time.deltaTime;
        }
        
        /// <summary>
        /// Rotates camera by 45 degrees left or right
        /// </summary>
        /// <param name="turnDirection">Pass true if left, false if right</param>
        private void RotateCamera(bool turnDirection)
        {
            if (!(_currentCameraRotationCooldown <= 0)) return;
            
            var multiplier = turnDirection ? 1 : -1 ;
            _mainCamera.transform.RotateAround(transform.position, Vector3.up, 45*multiplier);
            _playerMovement.UpdateDirections();
            _currentCameraRotationCooldown = maxCameraRotationCooldown;
        }
    }
}