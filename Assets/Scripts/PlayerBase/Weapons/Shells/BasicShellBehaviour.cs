using MazeWar.PlayerBase.Weapons.Shells.Base;
using UnityEngine;

namespace MazeWar.PlayerBase.Weapons.Shells
{
    public class BasicShellBehaviour : BaseShellBehaviour
    {
        protected override void DoActionsAndDestroySelf(bool onCollisionWithPlayer)
        {
            if (!onCollisionWithPlayer)
            {
                Debug.LogWarning("Don't forget to add shell disappearing animation!");
            }
            base.DoActionsAndDestroySelf(onCollisionWithPlayer);
        }
    }
}