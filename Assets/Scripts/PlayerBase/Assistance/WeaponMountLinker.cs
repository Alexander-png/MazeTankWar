using MazeWar.PlayerBase.Weapons.Mount;
using UnityEngine;

namespace MazeWar.PlayerBase.Assistance
{
    public class WeaponMountLinker : MonoBehaviour
    {
        [SerializeField]
        private WeaponMount _weaponMount;
        public WeaponMount WeaponMount => _weaponMount;
    }
}
