using MazeWar.PlayerBase.Weapons.Shells;
using System;
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

        public EventHandler<WeaponSwitchEventArgs> OnWeaponCanBeSwitched { get; set; }

        private bool CanShoot = true;


        public void Shoot(bool triggerPressed)
        {
            if (triggerPressed && CanShoot)
            {
                Instantiate(ShellPrefab, ShellSpawnPoint.transform.position, ShellSpawnPoint.transform.rotation).GetComponent<IShell>().OnShellPreDestroy += ShellDestroyed;
                CanShoot = false;
            }
        }

        // The basic cannon can't be reloaded manually
        public void Reload() { }

        public bool CanBeSwitchedNow()
        {
            return true;
        }

        private void ShellDestroyed(object sender, ShellPreDestroyEventArgs e)
        {
            CanShoot = true;
        }
    }
}
