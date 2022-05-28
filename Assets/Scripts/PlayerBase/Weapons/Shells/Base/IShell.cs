using System;
using UnityEngine;

namespace MazeWar.PlayerBase.Weapons.Shells.Base
{
    public class ShellPreDestroyEventArgs : EventArgs
    {
        public readonly float PreDestroyAnimationTimeSpan;

        public ShellPreDestroyEventArgs(float animationTimeSpan)
        {
            PreDestroyAnimationTimeSpan = animationTimeSpan;
        }
    }

    public interface IShell
    {
        public float LifeTime { get; }
        public Color ShellColor { get; set; }
        public EventHandler<ShellPreDestroyEventArgs> OnShellPreDestroy { get; set; }
        public void OnWeaponShoot(); 
    }
}
