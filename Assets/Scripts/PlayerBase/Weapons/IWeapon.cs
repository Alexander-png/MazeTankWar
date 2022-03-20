using System;
using UnityEngine;

namespace MazeWar.PlayerBase.Weapons
{
    public enum WeaponTypes { Cannon = 0, MachineGun = 1, ShotGun = 2, Explosive = 3, Missle = 4, Laser = 5 }

    public class WeaponSwitchEventArgs : EventArgs
    {
        public readonly WeaponTypes WeaponType;

        public WeaponSwitchEventArgs(WeaponTypes weaponType)
        {
            WeaponType = weaponType;
        }
    }

    public interface IWeapon
    { 
        public WeaponTypes WeaponType { get; }
        public GameObject ThisObject { get; }
        public void Shoot(bool triggerPressed);
        public void Reload();
        public bool CanBeSwitchedNow();
        public EventHandler<WeaponSwitchEventArgs> OnWeaponCanBeSwitched { get; set; }
    }
}
