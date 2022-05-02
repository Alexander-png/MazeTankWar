using MazeWar.Base;
using MazeWar.Pickup.Scriptable;
using MazeWar.PlayerBase.Observer;
using System;
using UnityEngine;

namespace MazeWar.Pickup
{
    public class Pickup : MonoBehaviour
    {
        private bool OnCollisionWithPlayer = false;
        private PlayerStateObserver CurrentPlayerObserver;

        public event OnBonusPickedEvent OnPicked;

        [SerializeField]
        private SpriteRenderer Renderer;

        public PickupData Data { get; private set; }

        private void Awake()
        {
            GlobalManager.GameplayManager.OnRoundRestart += OnRoundRestart;
        }

        private void OnRoundRestart(object sender, EventArgs e)
        {
            DestroySelf();
        }

        public void SetPickupData(PickupData data)
        {
            Data = data;
            if (Data.PickupSprite != null)
                Renderer.sprite = data.PickupSprite;
#if DEBUG
            else
            {
                Debug.LogWarning("Pickup data sprite is null!");
                Renderer.color = Color.yellow;
            }
#endif
            gameObject.SetActive(true);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            OnCollisionWithPlayer = collision.gameObject.TryGetComponent(out CurrentPlayerObserver);
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (OnCollisionWithPlayer && CurrentPlayerObserver.SetWeapon(Data.WeaponType))
            {
                OnPicked?.Invoke();
                DestroySelf();
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (OnCollisionWithPlayer && collision.gameObject.Equals(CurrentPlayerObserver.gameObject))
            {
                CurrentPlayerObserver = null;
                OnCollisionWithPlayer = false;
            } 
        }

        private void DestroySelf()
        {
            OnPicked = null;

            GlobalManager.GameplayManager.OnRoundRestart -= OnRoundRestart;
            Destroy(gameObject);
        }

        public delegate void OnBonusPickedEvent();
    }
}

