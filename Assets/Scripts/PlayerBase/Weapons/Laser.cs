using MazeWar.PlayerBase.Weapons.Base;
using MazeWar.PlayerBase.Weapons.Shells.Base;
using UnityEngine;

namespace MazeWar.PlayerBase.Weapons
{
    public class Laser : BaseWeapon
    {
        private bool _canBeSwitched = false;
        private IShell _passedLaser;

        public override void Shoot(bool triggerPressed)
        {
            if (triggerPressed && _canShoot)
            {
                _passedLaser = Instantiate(ShellPrefab, ShellSpawnPoint.transform.position, ShellSpawnPoint.transform.rotation).GetComponent<IShell>();
                _passedLaser.ShellColor = BulletColor;
                _passedLaser.OnShellPreDestroy += LaserDestroyed;
                _canShoot = false;
            }
        }

        private void LaserDestroyed(object sender, ShellPreDestroyEventArgs e)
        {
            _canBeSwitched = true;
            _passedLaser = null;
            OnWeaponCanBeSwitched?.Invoke(this, new WeaponSwitchEventArgs(WeaponType));
        }

        public override void Reload()
        {
            _canShoot = true;
            _canBeSwitched = false;
        }

        public override bool CanBeSwitchedNow()
        {
            return _canBeSwitched;
        }
    }
}
