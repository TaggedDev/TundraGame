using UnityEngine;

namespace Player.Behaviour
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private float _speed;
        //private Animator _animator;
        private Vector3 _forward;
        private Vector3 _right;
        private Vector3 _heading;

        public float Speed { get => _speed; set => _speed = value; }
        //public Animator Animator { get => _animator; set => _animator = value; }
        public Vector3 Forward { get => _forward; set => _forward = value; }
        public Vector3 Right { get => _right; set => _right = value; }
        public Vector3 Heading { get => _heading; set => _heading = value; }

        private void Awake()
        {
            //Animator = GetComponent<Animator>();
        }

        private void Start()
        {
            UpdateDirections();
        }

        public void UpdateDirections()
        {
            var cameraForward = UnityEngine.Camera.main.transform.forward;
            Forward = new Vector3(cameraForward.x, 0, cameraForward.z);
            Forward = Vector3.Normalize(Forward);
            Right = Quaternion.Euler(new Vector3(0, 90, 0)) * Forward;
        }
    }
}