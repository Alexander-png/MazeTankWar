using MazeWar.Base;
using System;
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

        private void Awake()
        {
            for (int i = 0; i < Weapons.Length; i++)
            {
                IWeapon w = Weapons[i].GetComponent<IWeapon>();
                WeaponDict[w.WeaponType] = w;
                w.OnWeaponCanBeSwitched += OnCurrentWeaponShooted;
            }
        }

        // Todo: reset weapon when round restarted

        private void Start()
        {
            //GlobalManager.GameplayManager.OnRoundRestart += OnRoundRestart;

            ShootButtonPressed = false;
            CurrentWeapon = WeaponDict[DefaultWeapon];
            CurrentWeapon.ThisObject.SetActive(true);
        }

        private void OnRoundRestart(object sender, EventArgs e)
        {
            SetCurrentWeapon(DefaultWeapon);
        }

        private void OnEnable()
        {
            ShootButtonPressed = false;
        }

        private void FixedUpdate()
        {
            ShootLogic();
        }

        private void ShootLogic()
        {
            CurrentWeapon.Shoot(ShootButtonPressed);
        }

        private void OnCurrentWeaponShooted(object sender, WeaponSwitchEventArgs e)
        {
            if (e.WeaponType != DefaultWeapon)
                SetCurrentWeapon(DefaultWeapon);
        }

        public bool SetCurrentWeapon(WeaponTypes wType)
        {
            if (CurrentWeapon.CanBeSwitchedNow())
            {
                CurrentWeapon.ThisObject.SetActive(false);
                CurrentWeapon = WeaponDict[wType];
                CurrentWeapon.Reload();
                WeaponDict[wType].ThisObject.SetActive(true);
                return true;
            }
            return false;
        }

        private void OnShoot(InputValue input)
        {
            ShootButtonPressed = input.isPressed;
        }
    }
}


