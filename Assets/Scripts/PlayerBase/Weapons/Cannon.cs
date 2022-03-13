using MazeWar.PlayerBase.Weapons.Shells;
using UnityEngine;

namespace MazeWar.PlayerBase.Weapons
{
    public class Cannon : MonoBehaviour, IWeapon
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

        public void Shoot()
        {
            if (CanShoot)
            {
                Instantiate(ShellPrefab, ShellSpawnPoint.transform.position, ShellSpawnPoint.transform.rotation).GetComponent<IShell>().OnShellPreDestroy += ShellDestroyed;
                CanShoot = false;
            }
        }

        private void ShellDestroyed(object sender, ShellPreDestroyEventArgs e)
        {
            CanShoot = true;
        }
    }
}
