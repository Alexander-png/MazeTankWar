using MazeWar.PlayerBase.Weapons.Base;
using System.Collections;
using UnityEngine;

namespace MazeWar.PlayerBase.Weapons
{
    public class MachineGun : BaseWeapon
    {
        [SerializeField, Min(1)]
        private int _magazineCapacity;
        [SerializeField]
        private float _fireRate;
        [SerializeField]
        private float _warmUpTime;
        [SerializeField]
        private float _spread;

        private int _shellCount;
        private bool _isShooting;
        private bool _shootCancelled;
        private Coroutine _shootCoroutine;

        public override void Shoot(bool triggerPressed)
        {
            if (triggerPressed)
            {
                if (_shootCoroutine == null)
                    _shootCoroutine = StartCoroutine(ShootCoroutine());
            }
            else
            {
                if (_isShooting)
                {
                    if (_shootCoroutine != null)
                        StopCoroutine(_shootCoroutine);
                    _shootCoroutine = null;
                    _isShooting = false;
                    _shootCancelled = true;
                    OnWeaponCanBeSwitched?.Invoke(this, new WeaponSwitchEventArgs(WeaponType));
                }
            }
        }

        public override void Reload()
        {
            if (_shootCoroutine != null)
            {
                StopCoroutine(_shootCoroutine);
                _shootCoroutine = null;
            }
            _isShooting = false;
            _shootCancelled = false;
            _shellCount = _magazineCapacity;
        }

        public override bool CanBeSwitchedNow()
        {
            return !_isShooting && _magazineCapacity != _shellCount || _shootCancelled;
        }

        private IEnumerator ShootCoroutine()
        {
            _isShooting = true;
            yield return new WaitForSeconds(_warmUpTime);
            while (_shellCount > 0)
            {
                yield return new WaitForSeconds(((1000 * 60) / _fireRate) / 1000);
                Vector3 shellSpawnPointRotation = ShellSpawnPoint.transform.rotation.eulerAngles;
                shellSpawnPointRotation.z += Random.Range(-_spread, _spread);
                Instantiate(ShellPrefab, ShellSpawnPoint.transform.position, Quaternion.Euler(shellSpawnPointRotation));
                _shellCount -= 1;
            }
            _isShooting = false;
            OnWeaponCanBeSwitched?.Invoke(this, new WeaponSwitchEventArgs(WeaponType));
        }
    }
}
