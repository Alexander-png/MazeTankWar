using MazeWar.PlayerBase.Weapons.Shells;
using UnityEngine;

namespace MazeWar.PlayerBase.Weapons
{
    public class ExplosiveCannon : MonoBehaviour, IWeapon
    {
        [SerializeField]
        private GameObject ShellPrefab;
        [SerializeField]
        private GameObject ShellSpawnPoint;
        [SerializeField]
        private WeaponTypes _WeaponType;
        public WeaponTypes WeaponType => _WeaponType;

        [SerializeField]
        private GameObject _ThisObject;
        public GameObject ThisObject => _ThisObject;

        private bool CanShoot = true;
        private bool CanShootAgain = false;
        private bool CanBeSwitched = false;
        private IShell PassedShell;

        public void Shoot(bool triggerPressed)
        {
            if (CanShoot)
            {
                if (triggerPressed)
                {
                    // If firing first time, just create explosive shell
                    if (PassedShell == null)
                    {
                        PassedShell = Instantiate(ShellPrefab, ShellSpawnPoint.transform.position, ShellSpawnPoint.transform.rotation).GetComponent<IShell>();
                        PassedShell.OnShellPreDestroy += ShellDestroyed;
                        CanShootAgain = false;
                    }
                    // Else explode it
                    else if (CanShootAgain)
                        PassedShell.OnWeaponShoot();
                }
                else
                    CanShootAgain = true;
            }
        }

        public void Reload() 
        {
            CanShoot = true;
        }

        public bool CanBeSwitchedNow()
        {
            return CanBeSwitched;
        }

        private void ShellDestroyed(object sender, ShellPreDestroyEventArgs e)
        {
            CanBeSwitched = true;
            CanShoot = false;
            PassedShell = null;
        }
    }
}

