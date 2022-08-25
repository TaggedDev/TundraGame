using Creatures.Player.Behaviour;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CameraConfiguration
{
    public class CameraMovement : MonoBehaviour
    {
        public const float speedModifier = 5f;

        private Camera _mainCamera;
        [SerializeField] private PlayerMovement _player;
        [SerializeField] private float maxCameraRotationCooldown;
        private float _currentCameraRotationCooldown;

        private void Start()
        {
            _mainCamera = Camera.main;
        }
        
        private void Update()
        {
            var pos = _player.transform.position;
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
            
            var multiplier = turnDirection ? 1 : -1;
            StartCoroutine(StartSmoothCameraRotation(45*multiplier));
            //_mainCamera.transform.RotateAround(transform.position, Vector3.up, 45*multiplier);
            _player.UpdateDirections();
            _currentCameraRotationCooldown = maxCameraRotationCooldown;
        }

        private IEnumerator StartSmoothCameraRotation(float rotationValue)
        {
            if (45 - rotationValue < 0.0001f)
            {
                for (float i = 0f; i < rotationValue; i+=speedModifier)
                {
                    _mainCamera.transform.RotateAround(transform.position, Vector3.up, speedModifier);
                    yield return new WaitForFixedUpdate();
                }
            }
            else
            {
                for (float i = 0f; i > rotationValue; i-=speedModifier)
                {
                    _mainCamera.transform.RotateAround(transform.position, Vector3.up, -speedModifier);
                    yield return new WaitForFixedUpdate();
                }
            }
        }
    }
}