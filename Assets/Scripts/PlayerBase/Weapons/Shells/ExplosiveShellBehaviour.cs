using MazeWar.Base;
using System;
using System.Collections;
using UnityEngine;

namespace MazeWar.PlayerBase.Weapons.Shells
{
    public class ExplosiveShellBehaviour : MonoBehaviour, IShell
    {
        private float AnimationTime = 0;

        [SerializeField]
        private Rigidbody2D ShellBody;

        [SerializeField]
        private float _LifeTime = 10f;
        public float LifeTime => _LifeTime;

        [SerializeField]
        private float Speed;

        [SerializeField]
        private GameObject FragmentPrefab;

        [SerializeField]
        public int FragmentCount;

        public EventHandler<ShellPreDestroyEventArgs> OnShellPreDestroy { get; set; }

        private void Awake()
        {
            ShellBody.AddForce(transform.up * Speed, ForceMode2D.Impulse);
            GlobalManager.GameplayManager.OnRoundRestart += OnRoundRestart;
            StartCoroutine(DestroySelfDelay(LifeTime));
        }

        private void OnRoundRestart(object sender, EventArgs e)
        {
            DoActionsAndDestroySelf(true);
        }

        private bool Encounting = false;
        private IEnumerator DestroySelfDelay(float seconds)
        {
            if (Encounting)
                yield break;

            Encounting = true;
            yield return new WaitForSeconds(seconds);
            DoActionsAndDestroySelf();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.tag == "Player")
            {
                collision.gameObject.SetActive(false);
                DoActionsAndDestroySelf();
            }
        }

        private void DoActionsAndDestroySelf(bool onRoundRestart = false)
        {
            OnShellPreDestroy?.Invoke(this, new ShellPreDestroyEventArgs(AnimationTime));
            GlobalManager.GameplayManager.OnRoundRestart -= OnRoundRestart;
            StopCoroutine(DestroySelfDelay(LifeTime));
            OnShellPreDestroy = null;
            if (!onRoundRestart)
                ReleaseFragments();
            Destroy(gameObject);
        }

        private void ReleaseFragments()
        {
            for (int i = 0; i < FragmentCount; i++)
                Instantiate(FragmentPrefab, gameObject.transform.position, Quaternion.Euler(new Vector3(0, 0, UnityEngine.Random.Range(0, 361))));
        }

        public void OnWeaponShoot()
        {
            DoActionsAndDestroySelf();
        }
    }
}


