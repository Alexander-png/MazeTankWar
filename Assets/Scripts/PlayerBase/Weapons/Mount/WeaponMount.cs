using MazeWar.Base;
using System.Collections.Generic;
using UnityEngine;

namespace MazeWar.PlayerBase.Weapons.Mount
{
    public class WeaponMount : MonoBehaviour
    {
        private Dictionary<WeaponTypes, IWeapon> WeaponDict = new Dictionary<WeaponTypes, IWeapon>();
        private IWeapon CurrentWeapon;

        [SerializeField]
        private WeaponTypes DefaultWeapon;
        [SerializeField]
        private Observer Observer;
        [SerializeField]
        private GameObject[] Weapons;

        private void Start()
        {
            for (int i = 0; i < Weapons.Length; i++)
            {
                IWeapon w = Weapons[i].GetComponent<IWeapon>();
                WeaponDict[w.WeaponType] = w;
            }
            OnRoundStarted();
        }

        public void OnRoundStarted()
        {
            CurrentWeapon = WeaponDict[DefaultWeapon];
            CurrentWeapon.ThisObject.SetActive(true);
        }

        private void FixedUpdate()
        {
            ShootLogic();
        }

        private void ShootLogic()
        {
            if (Input.GetAxis("Fire1") != 0)
                CurrentWeapon.Shoot();
        }
    }
}


