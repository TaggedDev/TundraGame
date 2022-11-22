using UnityEngine;

namespace Creatures.Player.Behaviour
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private float speed;
        private Camera _mainCamera;
        private bool _canSpint; 

        public bool CanSprint { get => _canSpint; set => _canSpint = value; }
        public float Speed { get => speed; set => speed = value; }
        public Vector3 Forward { get; private set; }
        public Vector3 Right { get; private set; }
        public Vector3 Heading { get; set; }

        /*private void Awake()
        {
            //Animator = GetComponent<Animator>();
        }*/

        private void Start()
        {
            _mainCamera = Camera.main;
            UpdateDirections();
        }

        public void UpdateDirections()
        {
            var cameraForward = (_mainCamera != null ? _mainCamera : Camera.main).transform.forward;
            Forward = new Vector3(cameraForward.x, 0, cameraForward.z);
            Forward = Vector3.Normalize(Forward);
            Right = Quaternion.Euler(new Vector3(0, 90, 0)) * Forward;
        }
    }
}