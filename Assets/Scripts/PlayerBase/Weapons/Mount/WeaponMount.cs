using MazeWar.Base;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MazeWar.PlayerBase.Weapons.Mount
{
    public class WeaponMount : MonoBehaviour
    {
        private Dictionary<WeaponTypes, IWeapon> WeaponDict = new Dictionary<WeaponTypes, IWeapon>();
        private IWeapon CurrentWeapon;
        private bool ShootButtonPressed;

        [SerializeField]
        private WeaponTypes DefaultWeapon;
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

        private void OnEnable()
        {
            ShootButtonPressed = false;
        }

        public void OnRoundStarted()
        {
            CurrentWeapon = WeaponDict[DefaultWeapon];
            CurrentWeapon.ThisObject.SetActive(true);
        }

        private void FixedUpdate()
        {
            if (ShootButtonPressed)
                ShootLogic();
        }

        private void OnShoot(InputValue input)
        {
            ShootButtonPressed = input.isPressed;
        }

        private void ShootLogic()
        {
            CurrentWeapon.Shoot();
        }
    }
}


