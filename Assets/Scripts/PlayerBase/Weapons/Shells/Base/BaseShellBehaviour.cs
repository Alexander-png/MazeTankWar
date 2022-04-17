using MazeWar.Base;
using System;
using System.Collections;
using UnityEngine;

namespace MazeWar.PlayerBase.Weapons.Shells.Base
{
    [RequireComponent(typeof(Rigidbody2D))]
    public abstract class BaseShellBehaviour : MonoBehaviour, IShell
    {
        private float AnimationTime = 0;

        [SerializeField]
        protected Rigidbody2D ShellBody;

        [SerializeField]
        protected float Speed;

        [SerializeField]
        protected float _LifeTime = 10f;
        public float LifeTime => _LifeTime;

        public EventHandler<ShellPreDestroyEventArgs> OnShellPreDestroy { get; set; }

        protected virtual void Awake()
        {
            ShellBody.AddForce(transform.up * Speed, ForceMode2D.Impulse);
            GlobalManager.GameplayManager.OnRoundRestart += OnRoundRestart;
            StartCoroutine(DestroySelfDelay(LifeTime));
        }

        protected bool Encounting = false;
        protected IEnumerator DestroySelfDelay(float seconds)
        {
            if (Encounting)
                yield break;

            Encounting = true;
            yield return new WaitForSeconds(seconds);
            DoActionsAndDestroySelf(false);
        }

        protected virtual void DoActionsAndDestroySelf(bool onCollisionWithPlayer)
        {
            Debug.LogWarning("BaseShellBehaviour: On base DoActionsAndDestroySelf");

            OnShellPreDestroy?.Invoke(this, new ShellPreDestroyEventArgs(AnimationTime));
            StopCoroutine(DestroySelfDelay(LifeTime));
            OnShellPreDestroy = null;
            GlobalManager.GameplayManager.OnRoundRestart -= OnRoundRestart;
            Destroy(gameObject);
        }

        protected virtual void OnRoundRestart(object sender, EventArgs e)
        {
            DoActionsAndDestroySelf(false);
        }

        public virtual void OnWeaponShoot()
        {
            Debug.LogWarning("BaseShellBehaviour: On base OnWeaponShoot");
        }

        protected virtual void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.tag == "Player")
            {
                collision.gameObject.SetActive(false);
                DoActionsAndDestroySelf(true);
            }
        }
    }
}
