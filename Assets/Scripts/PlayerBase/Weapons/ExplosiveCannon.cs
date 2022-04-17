using MazeWar.PlayerBase.Weapons.Base;
using MazeWar.PlayerBase.Weapons.Shells;
using System;
using UnityEngine;

namespace MazeWar.PlayerBase.Weapons
{
    public class ExplosiveCannon : BaseWeapon
    {
        private bool _CanShootAgain = false;
        private bool _CanBeSwitched = false;
        private IShell _PassedShell;

        public override void Shoot(bool triggerPressed)
        {
            if (_CanShoot)
            {
                if (triggerPressed)
                {
                    // If firing first time, just create explosive shell
                    if (_PassedShell == null)
                    {
                        _PassedShell = Instantiate(ShellPrefab, ShellSpawnPoint.transform.position, ShellSpawnPoint.transform.rotation).GetComponent<IShell>();
                        _PassedShell.OnShellPreDestroy += ShellDestroyed;
                        _CanShootAgain = false;
                    }
                    // Else explode it
                    else if (_CanShootAgain)
                        _PassedShell.OnWeaponShoot();
                }
                else
                    _CanShootAgain = true;
            }
        }

        private void OnDisable()
        {
            if (_PassedShell != null)
                _PassedShell.OnWeaponShoot();
        }

        public override void Reload() 
        {
            _CanShoot = true;
            _CanBeSwitched = false;
        }

        public override bool CanBeSwitchedNow()
        {
            return _CanBeSwitched;
        }

        private void ShellDestroyed(object sender, ShellPreDestroyEventArgs e)
        {
            _CanBeSwitched = true;
            _CanShoot = false;
            _PassedShell = null;
            OnWeaponCanBeSwitched?.Invoke(this, new WeaponSwitchEventArgs(WeaponType));
        }
    }
}

