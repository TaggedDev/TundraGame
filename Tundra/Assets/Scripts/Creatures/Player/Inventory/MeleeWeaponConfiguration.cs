using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Creatures.Player.Inventory
{
    [CreateAssetMenu(fileName = "New Melee Configuration", menuName = "Items/Melee Configuration")]
    public class MeleeWeaponConfiguration : WeaponConfiguration
    {
        [SerializeField]
        private float _WindupTime;
        [SerializeField]
        private float _damage;
        [SerializeField]
        private string _animationWeaponName;
        [SerializeField]
        private float _releaseClipLength;
        [SerializeField]
        private float _windupClipLength;
        /// <summary>
        /// Full windup time
        /// </summary>
        public float FullWindupTime { get => _WindupTime; private set => _WindupTime=value; }
        /// <summary>
        /// Weapon base damage
        /// </summary>
        public float Damage { get => _damage; private set => _damage=value; }
        /// <summary>
        /// Returns a name of a weapon, used for animation
        /// </summary>
        public string AnimationWeaponName { get => _animationWeaponName; set => _animationWeaponName = value; }
        /// <summary>
        /// Returns the Speed animation should played
        /// </summary>
        public float AnimationClipSpeed { get => _windupClipLength / _WindupTime; }
        /// <summary>
        /// Returns lengs of the release clip
        /// </summary>
        public float WinupAnimationLenght { get => _releaseClipLength; }
        /// <summary>
        /// Returns lengs of the windup clip
        /// </summary>
        public float ReleaseAnimationLenght { get => _releaseClipLength; }
    }
}
