using MazeWar.EditorAssistance.Attributes;
using MazeWar.PlayerBase.Observer;
using MazeWar.PlayerBase.Weapons.Shells.Base;
using System.Collections;
using UnityEngine;

namespace MazeWar.PlayerBase.Weapons.Shells
{
    public class Missle : BaseShellBehaviour
    {
        [SerializeField]
        private SpriteRenderer _spriteRenedrer;
        [SerializeField]
        private float _activationTime;

        [SerializeField, ReadOnly]
        private PlayerStateObserver _target;

        public override Color ShellColor
        {
            get => _spriteRenedrer.color;
            set
            {
                if (_spriteRenedrer != null)
                    _spriteRenedrer.color = value;
            }
        }

        protected override void DoActionsAndDestroySelf(bool onCollisionWithPlayer)
        {
            if (!onCollisionWithPlayer)
            {
                Debug.LogWarning("Don't forget to add shell disappearing animation!");
            }
            base.DoActionsAndDestroySelf(onCollisionWithPlayer);
        }

        protected override void Awake()
        {
            _LifeTime += _activationTime;
            base.Awake();
        }

        private void Start()
        {
            StartCoroutine(ActivateCoroutine(_activationTime));
        }

        //private void Update()
        //{
            
        //}

        private IEnumerator ActivateCoroutine(float activationTime)
        {
            yield return new WaitForSeconds(activationTime);
            _target = FindNearestPlayer();
        }

        private PlayerStateObserver FindNearestPlayer()
        {
            return null;
        }

        private void FindRouteToTarget()
        {

        }
    }
}