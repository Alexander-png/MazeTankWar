using MazeWar.PlayerBase.Assistance;
using MazeWar.PlayerBase.Weapons;
using UnityEngine;

namespace MazeWar.PlayerBase.Observer
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class PlayerStateObserver : MonoBehaviour
    {
        private bool _isAlive;

        [SerializeField]
        private Color _playerColor;
        [SerializeField]
        private WeaponMountLinker _weaponMountLinker;
        [SerializeField]
        private SpriteRenderer _spriteRenderer;

        public bool IsAlive
        {
            get => _isAlive;
            set
            {
                _isAlive = value;
                if (!_isAlive)
                    OnKilled?.Invoke(this);
                gameObject.SetActive(_isAlive);
            }
        }

        public event KilledEvent OnKilled;

        public int Score { get; set; }
        public Color PlayerColor => _playerColor;

        private void OnValidate()
        {
            _spriteRenderer.color = PlayerColor;
        }

        private void Start()
        {
            _spriteRenderer.color = PlayerColor;
        }

        public bool SetWeapon(WeaponTypes wType)
        {
            return _weaponMountLinker.WeaponMount.SetCurrentWeapon(wType);
        }

        public void PlayExplosionAnimation()
        {
            Debug.LogWarning("TODO: add explosion animation");
        }

        public delegate void KilledEvent(PlayerStateObserver sender);
    }
}