using MazeWar.Base;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MazeWar.PlayerBase.Weapons.Shells.Fragments
{
    public class Fragment : MonoBehaviour
    {
        [SerializeField]
        private Rigidbody2D FragmentBody;

        [SerializeField]
        private float Speed;
        [SerializeField]
        private float RotationSpeed;


        public float LifeTime = 15;

        private void Awake()
        {
            FragmentBody.AddForce(transform.up * Speed, ForceMode2D.Impulse);
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
            if (collision.gameObject.tag == "Player")
            {
                collision.gameObject.SetActive(false);
                DoActionsAndDestroySelf();
            }
        }
    }
}