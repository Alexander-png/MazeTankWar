using MazeWar.PlayerBase.Weapons;
using UnityEngine;

//https://habr.com/ru/post/421523/
//https://pavcreations.com/equipment-system-for-an-rpg-unity-game/
namespace MazeWar.Pickup.Scriptable
{
    [CreateAssetMenu(fileName = "New pickup", menuName = "Pickup", order = 51)]
    public class PickupData : ScriptableObject
    {
        public WeaponTypes WeaponType;
        public Sprite PickupSprite;
    }
}


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