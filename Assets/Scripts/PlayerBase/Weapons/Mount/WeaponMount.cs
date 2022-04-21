using MazeWar.Base;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace MazeWar.PlayerBase.Weapons.Mount
{
    public class WeaponMount : MonoBehaviour
    {
        private Dictionary<WeaponTypes, IWeapon> WeaponDict = new Dictionary<WeaponTypes, IWeapon>();
        private IWeapon CurrentWeapon;
        public bool ShootButtonPressed;

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

        private void Start()
        {
            if (GlobalManager.GameplayManager == null)
                GlobalManager.OnGameplayManagerAppeared += OnGameplayManagerInitialized;
            else
                GlobalManager.GameplayManager.OnRoundRestart += OnRoundRestart;

            ShootButtonPressed = false;
            CurrentWeapon = WeaponDict[DefaultWeapon];
            CurrentWeapon.ThisObject.SetActive(true);
        }

        private void OnGameplayManagerInitialized(object sender, EventArgs e)
        {
            // Because OnGameplayManagerAppeared can be invoked only once, we don't need this subscription anymore.
            GlobalManager.OnGameplayManagerAppeared -= OnGameplayManagerInitialized;
            GlobalManager.GameplayManager.OnRoundRestart += OnRoundRestart;
        }

        private void OnRoundRestart(object sender, EventArgs e)
        {
            SetCurrentWeapon(DefaultWeapon, true);
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

        public bool SetCurrentWeapon(WeaponTypes wType, bool forced = false)
        {
            if (CurrentWeapon.CanBeSwitchedNow() || forced)
            {
                CurrentWeapon.ThisObject.SetActive(false);
                CurrentWeapon = WeaponDict[wType];
                CurrentWeapon.Reload();
                WeaponDict[wType].ThisObject.SetActive(true);
                ShootButtonPressed = false;
                return true;
            }
            return false;
        }
    }
}


