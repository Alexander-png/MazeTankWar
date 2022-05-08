using MazeWar.PlayerBase.Weapons.Base;
using MazeWar.PlayerBase.Weapons.Shells;

namespace MazeWar.PlayerBase.Weapons
{
    public class Cannon : BaseWeapon
    {
        private bool _triggerWasReleased;

        public override void Shoot(bool triggerPressed)
        {
            if (triggerPressed && _canShoot && _triggerWasReleased)
            {
                Instantiate(ShellPrefab, ShellSpawnPoint.transform.position, ShellSpawnPoint.transform.rotation).GetComponent<IShell>().OnShellPreDestroy += ShellDestroyed;
                _canShoot = false;
                _triggerWasReleased = false;
            }
            if (!triggerPressed)
                _triggerWasReleased = true;
        }

        private void ShellDestroyed(object sender, ShellPreDestroyEventArgs e)
        {
            _canShoot = true;
        }
    }
}
