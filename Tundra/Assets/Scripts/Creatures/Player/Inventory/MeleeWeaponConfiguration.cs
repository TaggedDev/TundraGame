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
        private float windupTime;
        [SerializeField]
        private float damage;
        [SerializeField]
        private string animationWeaponName;
        [SerializeField]
        private float releaseClipLength;
        [SerializeField]
        private float windupClipLength;
        /// <summary>
        /// Full windup time
        /// </summary>
        public float FullWindupTime { get => windupTime; private set => windupTime=value; }
        /// <summary>
        /// Weapon base damage
        /// </summary>
        public float Damage { get => damage; private set => damage=value; }
        /// <summary>
        /// Returns a name of a weapon, used for animation
        /// </summary>
        public string AnimationWeaponName { get => animationWeaponName; set => animationWeaponName = value; }
        /// <summary>
        /// Returns the Speed animation should played
        /// </summary>
        public float AnimationClipSpeed { get => windupClipLength / windupTime; }
        /// <summary>
        /// Returns lengs of the release clip
        /// </summary>
        public float WinupAnimationLenght { get => releaseClipLength; }
        /// <summary>
        /// Returns lengs of the windup clip
        /// </summary>
        public float ReleaseAnimationLenght { get => releaseClipLength; }
    }
}
