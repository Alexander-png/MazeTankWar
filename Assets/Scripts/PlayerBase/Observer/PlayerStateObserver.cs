using MazeWar.Base;
using MazeWar.PlayerBase.Weapons;
using MazeWar.PlayerBase.Weapons.Mount;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MazeWar.PlayerBase.Observer
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class PlayerStateObserver : MonoBehaviour
    {
        [SerializeField]
        private GameplayManager GameplayManager;
        [SerializeField]
        private WeaponMount PlayerWeaponMount;
        [SerializeField]
        private SpriteRenderer SpriteRenderer;

        private bool _IsAlive;
        public bool IsAlive
        {
            get => _IsAlive;
            set
            {
                _IsAlive = value;
                if (!_IsAlive)
                {
                    if (GameplayManager != null)
                    {
                        if (GameplayManager.InGame)
                            GameplayManager.OnPlayerKilled();
                        else
                            PlayExplosionAnimation();
                    }
                }
                gameObject.SetActive(_IsAlive);
            }
        }

        [NonSerialized]
        public int Score;

        public Color PlayerColor;

        private void Start()
        {
            SpriteRenderer.color = PlayerColor;
        }

        public bool SetWeapon(WeaponTypes wType)
        {
            return PlayerWeaponMount.SetCurrentWeapon(wType);
        }

        private void OnShoot(InputValue input)
        {
            PlayerWeaponMount.ShootButtonPressed = input.isPressed;
        }

        private void PlayExplosionAnimation()
        {
            Debug.LogWarning("TODO: add explosion animation");
        }
    }
}