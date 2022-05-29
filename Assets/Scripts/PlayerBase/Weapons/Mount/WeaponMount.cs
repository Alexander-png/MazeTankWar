using MazeWar.Base;
using MazeWar.PlayerBase.Weapons.Base;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace MazeWar.PlayerBase.Weapons.Mount
{
    public class WeaponMount : MonoBehaviour
    {
        private Dictionary<WeaponTypes, IWeapon> WeaponDict = new Dictionary<WeaponTypes, IWeapon>();
        private IWeapon CurrentWeapon;
        public Color PlayerColor
        {
            set
            {
                foreach (var pair in WeaponDict)
                    pair.Value.PassingShellColor = value;
            }
        }
        public bool ShootButtonPressed;

        [SerializeField]
        private WeaponTypes DefaultWeapon;
        [SerializeField]
        private Transform _weaponContainerTransform;

        private void Awake()
        {
            for (int i = 0; i < _weaponContainerTransform.childCount; i++)
            {
                IWeapon w = _weaponContainerTransform.GetChild(i).GetComponent<IWeapon>();
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

        private void OnRoundRestart()
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

        public bool SetCurrentWeapon(WeaponTypes wType, bool forceSwitch = false)
        {
            if (CurrentWeapon.CanBeSwitchedNow() || forceSwitch)
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


