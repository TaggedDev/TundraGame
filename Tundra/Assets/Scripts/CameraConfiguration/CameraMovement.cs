using Creatures.Player.Behaviour;
using System.Collections;
using UnityEngine;

namespace CameraConfiguration
{
    public class CameraMovement : MonoBehaviour
    {
        /// <summary>
        /// Camera movement speed coefficient.
        /// </summary>
        [SerializeField] private float speedModifier = 5f;
        /// <summary>
        /// Main camera reference.
        /// </summary>
        private Camera _mainCamera;
        /// <summary>
        /// Player movement script reference.
        /// </summary>
        [SerializeField] private PlayerMovement _player;
        /// <summary>
        /// Maximal cooldown for the camera rotation.
        /// </summary>
        [SerializeField] private float maxCameraRotationCooldown;
        /// <summary>
        /// Internal rotation cooldown value.
        /// </summary>
        private float _currentCameraRotationCooldown;

        private void Start()
        {
            _mainCamera = Camera.main;
        }
        
        private void Update()
        {
            var pos = _player.transform.position;
            transform.position = new Vector3(pos.x, pos.y, pos.z);
            if (Input.GetKey(KeyCode.LeftControl))
            {
                if (Input.GetKey(KeyCode.Q)) RotateCamera(true);
                if (Input.GetKey(KeyCode.E)) RotateCamera(false);
            }
            if (_currentCameraRotationCooldown > 0) _currentCameraRotationCooldown -= Time.deltaTime;
        }
        
        /// <summary>
        /// Rotates camera by 45 degrees left or right
        /// </summary>
        /// <param name="turnDirection">Pass true if left, false if right</param>
        private void RotateCamera(bool turnDirection)
        {
            if (!(_currentCameraRotationCooldown <= 0)) return;
            //Selects rotation direction.
            var multiplier = turnDirection ? 1 : -1;
            //Starts smooth camera rotation of 45 degrees by Y axis.
            StartCoroutine(StartSmoothCameraRotation(45, multiplier));
            //Updates player movement directions.
            _player.UpdateDirections();
            //Resets rotation cooldown.
            _currentCameraRotationCooldown = maxCameraRotationCooldown;
        }

        /// <summary>
        /// Starts smooth camera rotation by Y axis.
        /// </summary>
        /// <param name="rotationValue">Value in degrees which should be camera rotated by.</param>
        /// <param name="multiplier">Multiplier which represents rotation direction.</param>
        /// <returns></returns>
        private IEnumerator StartSmoothCameraRotation(float rotationValue, float multiplier)
        {
            //Updates camera rotation by speed coefficient every fixed update.
            for (float i = 0f; i < rotationValue; i+=speedModifier)
            {
                _mainCamera.transform.RotateAround(transform.position, Vector3.up, speedModifier * multiplier);
                yield return new WaitForFixedUpdate();
            }
        }
    }
}