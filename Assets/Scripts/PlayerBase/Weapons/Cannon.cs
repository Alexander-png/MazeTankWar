using MazeWar.PlayerBase.Weapons.Base;
using MazeWar.PlayerBase.Weapons.Shells;

namespace MazeWar.PlayerBase.Weapons
{
    public class Cannon : BaseWeapon
    {
        public override void Shoot(bool triggerPressed)
        {
            if (triggerPressed && _CanShoot)
            {
                Instantiate(ShellPrefab, ShellSpawnPoint.transform.position, ShellSpawnPoint.transform.rotation).GetComponent<IShell>().OnShellPreDestroy += ShellDestroyed;
                _CanShoot = false;
            }
        }

        private void ShellDestroyed(object sender, ShellPreDestroyEventArgs e)
        {
            _CanShoot = true;
        }
    }
}
