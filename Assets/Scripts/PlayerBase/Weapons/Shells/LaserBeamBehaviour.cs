using MazeWar.PlayerBase.Weapons.Shells.Base;
using UnityEngine;

namespace MazeWar.PlayerBase.Weapons.Shells
{
    public class LaserBeamBehaviour : BaseShellBehaviour
    {
        [SerializeField]
        private SpriteRenderer _spriteRenedrer;
        [SerializeField]
        private TrailRenderer _trail;

        public override Color ShellColor
        { 
            get => _spriteRenedrer.color; 
            set
            {
                if (_spriteRenedrer != null)
                    _spriteRenedrer.color = value;
                if (_trail != null)
                {
                    _trail.startColor = value;
                    _trail.endColor = value;
                }
            }
        }

        protected override void DoActionsAndDestroySelf(bool onCollisionWithPlayer)
        {
            _trail.transform.gameObject.transform.SetParent(null);
            base.DoActionsAndDestroySelf(onCollisionWithPlayer);
        }
    }
}