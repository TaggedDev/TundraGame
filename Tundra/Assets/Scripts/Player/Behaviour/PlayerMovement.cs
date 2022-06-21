using UnityEngine;

namespace Player.Behaviour
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private float speed;
        //private Animator _animator;
        private Camera _mainCamera;

        public float Speed { get => speed; set => speed = value; }
        //public Animator Animator { get => _animator; set => _animator = value; }
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
            var cameraForward = _mainCamera.transform.forward;
            Forward = new Vector3(cameraForward.x, 0, cameraForward.z);
            Forward = Vector3.Normalize(Forward);
            Right = Quaternion.Euler(new Vector3(0, 90, 0)) * Forward;
        }
    }
}