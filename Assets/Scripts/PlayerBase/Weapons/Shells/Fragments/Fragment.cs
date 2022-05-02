using MazeWar.Base;
using MazeWar.PlayerBase.Observer;
using System;
using System.Collections;
using UnityEngine;

namespace MazeWar.PlayerBase.Weapons.Shells.Fragments
{
    public class Fragment : MonoBehaviour
    {
        [SerializeField]
        private Rigidbody2D FragmentBody;

        [SerializeField]
        private float MinSpeed;
        [SerializeField]
        private float MaxSpeed;
        [SerializeField]
        private float RotationSpeed;

        [SerializeField]
        private float LifeTime = 15;

        private void Awake()
        {
            FragmentBody.AddForce(transform.up * UnityEngine.Random.Range(MinSpeed, MaxSpeed), ForceMode2D.Impulse);
            FragmentBody.angularVelocity = RotationSpeed;
            GlobalManager.GameplayManager.OnRoundRestart += OnRoundRestart;
            StartCoroutine(DestroySelfDelay(LifeTime));
        }

        private void OnRoundRestart(object sender, EventArgs e)
        {
            DoActionsAndDestroySelf();
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

        private void DoActionsAndDestroySelf()
        {   
            GlobalManager.GameplayManager.OnRoundRestart -= OnRoundRestart;
            StopCoroutine(DestroySelfDelay(LifeTime));
            Destroy(gameObject);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.TryGetComponent(out PlayerStateObserver observer))
            {
                observer.IsAlive = false;
                DoActionsAndDestroySelf();
            }
        }
    }
}