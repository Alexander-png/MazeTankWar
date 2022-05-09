using MazeWar.PlayerBase.Weapons.Base;
using MazeWar.PlayerBase.Weapons.Shells.Base;

namespace MazeWar.PlayerBase.Weapons
{
    public class ExplosiveCannon : BaseWeapon
    {
        private bool _canShootAgain = false;
        private bool _canBeSwitched = false;
        private IShell _passedShell;

        public override void Shoot(bool triggerPressed)
        {
            if (_canShoot)
            {
                if (triggerPressed)
                {
                    // If firing first time, just create explosive shell
                    if (_passedShell == null)
                    {
                        _passedShell = Instantiate(ShellPrefab, ShellSpawnPoint.transform.position, ShellSpawnPoint.transform.rotation).GetComponent<IShell>();
                        _passedShell.OnShellPreDestroy += ShellDestroyed;
                        _canShootAgain = false;
                    }
                    // Else explode it
                    else if (_canShootAgain)
                        _passedShell.OnWeaponShoot();
                }
                else
                    _canShootAgain = true;
            }
        }

        private void OnDisable()
        {
            if (_passedShell != null)
                _passedShell.OnWeaponShoot();
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

        private void ShellDestroyed(object sender, ShellPreDestroyEventArgs e)
        {
            _canBeSwitched = true;
            _canShoot = false;
            _passedShell = null;
            OnWeaponCanBeSwitched?.Invoke(this, new WeaponSwitchEventArgs(WeaponType));
        }
    }
}

