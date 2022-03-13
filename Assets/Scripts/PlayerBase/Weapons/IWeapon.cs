using UnityEngine;

namespace MazeWar.PlayerBase.Weapons
{
    public enum WeaponTypes { Cannon = 0, MachineGun = 1, ShotGun = 2, Explosive = 3, Missle = 4, Laser = 5 }

    public interface IWeapon
    { 
        public WeaponTypes WeaponType { get; }
        public GameObject ThisObject { get; }
        public void Shoot();
    }

    //https://habr.com/ru/post/421523/
    //[CreateAssetMenu(fileName = "New tank weapon", menuName = "Tank weapon", order = 51)]
    //public class Weapon : ScriptableObject
    //{
    //    public WeaponTypes WeaponType;
    //    [SerializeField]
    //    private Sprite WeaponSprite;

    //    //private GameObject PassedBullet { get; set; }
    //    //public virtual void Shoot()
    //    //{
    //    //    Debug.LogWarning("The base Weapon.Shoot() was called. Please ensure that shoot method was overridden.");
    //    //}
    //}
}
