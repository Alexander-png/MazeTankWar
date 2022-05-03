using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MazeWar.UI.PlayerList
{
    public class RecordTemplateDataContainer : MonoBehaviour
    {
        [SerializeField]
        private Image _background;
        [SerializeField]
        private Image _playerColor;
        [SerializeField]
        private TMP_Text _scoreLabel;

        public Image Background => _background;
        public Image PlayerColor => _playerColor;
        public TMP_Text ScoreLabel => _scoreLabel;
    }
}