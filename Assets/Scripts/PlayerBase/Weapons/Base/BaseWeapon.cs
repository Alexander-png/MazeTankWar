using System;
using UnityEngine;

namespace MazeWar.PlayerBase.Weapons.Base
{
    public abstract class BaseWeapon : MonoBehaviour, IWeapon
    {
        [SerializeField]
        protected GameObject ShellPrefab;
        [SerializeField]
        protected GameObject ShellSpawnPoint;
        [SerializeField]
        protected WeaponTypes _WeaponType;

        public Color BulletColor { get; set; }
        public WeaponTypes WeaponType => _WeaponType;
        public GameObject ThisObject => gameObject;
        public EventHandler<WeaponSwitchEventArgs> OnWeaponCanBeSwitched { get; set; }

        protected bool _canShoot = true;

        public virtual void Shoot(bool triggerPressed)
        {
            
        }

        public virtual void Reload()
        {
            
        }

        public virtual bool CanBeSwitchedNow()
        {
            return true;
        }
    }
}


