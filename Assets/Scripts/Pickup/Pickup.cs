using MazeWar.Base;
using MazeWar.Pickup.Scriptable;
using MazeWar.PlayerBase.Observer;
using System;
using UnityEngine;

namespace MazeWar.Pickup
{
    public class Pickup : MonoBehaviour
    {
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

        PlayerObjectObserver CurrentPlayerObserver;
        private void OnTriggerEnter2D(Collider2D collision)
        {
            // if collided with any of players (player layer id is 6)
            // Try to set player weapon type on trigger stay to avoid multiple GetComponent calls.
            if (collision.gameObject.layer == 6)
                CurrentPlayerObserver = collision.gameObject.GetComponent<PlayerObjectObserver>();
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.gameObject.layer == 6 && CurrentPlayerObserver.SetWeapon(Data.WeaponType))
                DestroySelf();
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.layer == 6)
                CurrentPlayerObserver = null;
        }

        private void DestroySelf()
        {
            GlobalManager.GameplayManager.OnRoundRestart -= OnRoundRestart;
            Destroy(gameObject);
        }
    }
}

