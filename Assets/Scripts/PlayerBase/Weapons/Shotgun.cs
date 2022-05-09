using MazeWar.PlayerBase.Weapons.Base;
using System.Collections;
using UnityEngine;

namespace MazeWar.PlayerBase.Weapons
{
    public class Shotgun : BaseWeapon
    {
        [SerializeField, Min(1)]
        private int _magazineCapacity;
        [SerializeField]
        private float _fireRate;
        [SerializeField, Min(1)]
        private int _pelletCount;
        [SerializeField]
        private float _pelletSpread;

        private int _shellCount;
        private bool _triggerWasReleased;

        public override void Shoot(bool triggerPressed)
        {
            if (triggerPressed && _canShoot && _triggerWasReleased)
            {
                if (_canShoot)
                {
                    for (int i = 0; i < _pelletCount; i++)
                    {
                        Vector3 shellSpawnPointRotation = ShellSpawnPoint.transform.rotation.eulerAngles;
                        shellSpawnPointRotation.z += Random.Range(-_pelletSpread, _pelletSpread);
                        Instantiate(ShellPrefab, ShellSpawnPoint.transform.position, Quaternion.Euler(shellSpawnPointRotation));
                    }
                    _shellCount -= 1;
                    if (_shellCount == 0)
                        OnWeaponCanBeSwitched?.Invoke(this, new WeaponSwitchEventArgs(WeaponType));
                    _canShoot = false;
                    _triggerWasReleased = false;
                    StartCoroutine(ReloadCoroutine());
                }
            }
            if (!triggerPressed)
                _triggerWasReleased = true;
        }

        public override void Reload()
        {
            _canShoot = true;
            _shellCount = _magazineCapacity;
        }

        public override bool CanBeSwitchedNow()
        {
            return _shellCount == 0;
        }

        private IEnumerator ReloadCoroutine()
        {
            yield return new WaitForSeconds(((1000 * 60) / _fireRate) / 1000);
            _canShoot = true;
        }
    }
}
