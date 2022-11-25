using UnityEngine;

namespace Creatures.Player.Behaviour
{
    /// <summary>
    /// Represents animation logic
    /// </summary>
    public class PlayerAnimation : MonoBehaviour
    {
        private Animator _animator;
        private string _currentStateName;

        private void Start()
        {
            _animator = GetComponent<Animator>();
            _currentStateName = "Idle";
        }

        /// <summary>
        /// Switches the current animation  
        /// </summary>
        /// <param name="animationName">The name of the state in Animator</param>
        /// <param name="speed">Animation speed</param>
        public void SwitchAnimation(string animationName, float speed = 1)
        {
            if (_currentStateName == animationName)
                return;
            _animator.speed = speed;
            _animator.CrossFade(animationName, .1f);
            _currentStateName = animationName;
        }
    }
}