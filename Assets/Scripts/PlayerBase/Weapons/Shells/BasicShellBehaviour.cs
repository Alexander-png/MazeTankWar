using System;
using System.Collections;
using UnityEngine;

namespace MazeWar.PlayerBase.Weapons.Shells
{
    public class BasicShellBehaviour : MonoBehaviour, IShell
    {
        private float AnimationTime = 0;

        [SerializeField]
        private Rigidbody2D ShellBody;

        [SerializeField]
        private float _LifeTime = 10f;
        public float LifeTime => _LifeTime;

        public EventHandler<ShellPreDestroyEventArgs> OnShellPreDestroy { get; set; }

        [SerializeField]
        private float Speed;

        private void Awake()
        {
            ShellBody.AddForce(transform.up * Speed, ForceMode2D.Impulse);
            StartCoroutine(DestroySelfDelay(LifeTime));
        }

        private bool Encounting = false;
        private IEnumerator DestroySelfDelay(float seconds)
        {
            if (Encounting)
                yield break;

            Encounting = true;
            yield return new WaitForSeconds(seconds);
            DoActionsAndDestroySelf(false);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.name.Contains("Player"))
            {
                collision.gameObject.SetActive(false);
                DoActionsAndDestroySelf(true);
            }
        }

        private void DoActionsAndDestroySelf(bool onCollisionWithPlayer)
        {
            if (!onCollisionWithPlayer)
            {
                Debug.LogWarning("Don't forget to add shell disappearing animation!");
            }

            OnShellPreDestroy?.Invoke(this, new ShellPreDestroyEventArgs(AnimationTime));
            StopAllCoroutines();
            OnShellPreDestroy = null;
            Destroy(gameObject);
        }
    }
}


