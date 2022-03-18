using MazeWar.Base;
using MazeWar.PlayerBase.Weapons;
using MazeWar.PlayerBase.Weapons.Mount;
using UnityEngine;

namespace MazeWar.PlayerBase.Observer
{
    public class PlayerObjectObserver : MonoBehaviour
    {
        [SerializeField]
        private GameplayManager GameplayManager;
        [SerializeField]
        private WeaponMount PlayerWeaponMount;

        private void OnDisable()
        {
            if (GameplayManager != null)
                GameplayManager.OnPlayerKilled();
        }

        public bool SetWeapon(WeaponTypes wType)
        {
            return PlayerWeaponMount.SetCurrentWeapon(wType);
        }
    }
}