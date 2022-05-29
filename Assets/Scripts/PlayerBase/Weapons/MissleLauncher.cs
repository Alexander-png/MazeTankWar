using MazeWar.PlayerBase.Weapons.Base;
using MazeWar.PlayerBase.Weapons.Shells.Base;

namespace MazeWar.PlayerBase.Weapons
{
    public class MissleLauncher : BaseWeapon
    {
        private bool _canBeSwitched = false;
        private IShell _passedMissle;

        public override void Shoot(bool triggerPressed)
        {
            if (triggerPressed && _canShoot)
            {
                _passedMissle = Instantiate(ShellPrefab, ShellSpawnPoint.transform.position, ShellSpawnPoint.transform.rotation).GetComponent<IShell>();
                _passedMissle.ShellColor = PassingShellColor;
                _passedMissle.OnShellPreDestroy += MissleDestroyed;
                _canShoot = false;
            }
        }

        private void MissleDestroyed(object sender, ShellPreDestroyEventArgs e)
        {
            _canBeSwitched = true;
            if (_passedMissle != null)
                _passedMissle.OnShellPreDestroy -= MissleDestroyed;
            _passedMissle = null;
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
