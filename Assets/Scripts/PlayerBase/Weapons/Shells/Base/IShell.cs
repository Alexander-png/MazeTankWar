using System;

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
        float LifeTime { get; }
        EventHandler<ShellPreDestroyEventArgs> OnShellPreDestroy { get; set; }
        public void OnWeaponShoot(); 
    }
}
