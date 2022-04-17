using MazeWar.Base;
using MazeWar.PlayerBase.Weapons.Shells.Base;
using System;
using UnityEngine;

namespace MazeWar.PlayerBase.Weapons.Shells
{
    public class ExplosiveShellBehaviour : BaseShellBehaviour
    {
        [SerializeField]
        private GameObject FragmentPrefab;
        [SerializeField]
        private int FragmentCount;

        protected override void OnRoundRestart(object sender, EventArgs e)
        {
            DoActionsAndDestroySelf(false);
        }

        protected override void DoActionsAndDestroySelf(bool onCollisionWithPlayer)
        {
            if (onCollisionWithPlayer || IsInGame())
                ReleaseFragments();
            base.DoActionsAndDestroySelf(onCollisionWithPlayer);
        }

        private void ReleaseFragments()
        {
            int currentDirection = 0;
            int minStep = 5;
            int maxStep = 30;

            for (int i = 0; i < FragmentCount; i++)
            {
                currentDirection += UnityEngine.Random.Range(minStep, maxStep);
                Instantiate(FragmentPrefab, gameObject.transform.position, Quaternion.Euler(new Vector3(0, 0, currentDirection)));
            }   
        }

        private bool IsInGame() => GlobalManager.GameplayManager.InGame;

        public override void OnWeaponShoot()
        {
            DoActionsAndDestroySelf(false);
        }
    }
}


