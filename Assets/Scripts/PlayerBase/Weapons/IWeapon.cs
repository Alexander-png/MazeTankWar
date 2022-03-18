using UnityEngine;

namespace MazeWar.PlayerBase.Weapons
{
    public enum WeaponTypes { Cannon = 0, MachineGun = 1, ShotGun = 2, Explosive = 3, Missle = 4, Laser = 5 }

    public interface IWeapon
    { 
        public WeaponTypes WeaponType { get; }
        public GameObject ThisObject { get; }
        public void Shoot();
        public bool CanBeSwitchedNow();
    }
}
