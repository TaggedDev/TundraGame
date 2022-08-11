using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Creatures.Player.Inventory
{
    [CreateAssetMenu(fileName = "New Ranged Configuration", menuName = "Items/Ranged Configuration")]
    public class RangedWeaponConfiguration : WeaponConfiguration
    {
        [SerializeField]
        private float halfOfMinAiming;
        [SerializeField]
        private float shotDamage;
        [SerializeField]
        private float aimingTime;
        [SerializeField]
        private float cameraAimingZoomDistance;

        /// <summary>
        /// Половина от максимального угла сведения (минимального прицеливания).
        /// </summary>
        public float HalfOfMinAiming { get => halfOfMinAiming; private set => halfOfMinAiming=value; }
        /// <summary>
        /// Урон от выстрела. 
        /// </summary>
        public float ShotDamage { get => shotDamage; private set => shotDamage=value; }
        /// <summary>
        /// Время сведения.
        /// </summary>
        public float AimingTime { get => aimingTime; private set => aimingTime=value; }
        /// <summary>
        /// Отдаление камеры при прицеливании.
        /// </summary>
        public float CameraAimingZoomDistance { get => cameraAimingZoomDistance; set => cameraAimingZoomDistance=value; }
    }
}
