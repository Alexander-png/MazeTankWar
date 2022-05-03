using MazeWar.Base.Abstractions;
using MazeWar.PlayerBase.Observer;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace MazeWar.UI.PlayerList
{
    public class PlayerListBehaviour : MonoBehaviour
    {
        [SerializeField]
        private AbstractGameplayManager _gameManager;
        [SerializeField]
        private GameObject _recordContainer;
        [SerializeField]
        private Image _recordTemplate;
        [SerializeField]
        private float _stepBetweenRecords;
        [SerializeField]
        private int _showFirstCount;

        private List<RecordTemplateDataContainer> _records = new List<RecordTemplateDataContainer>();

        private void Start()
        {
            _gameManager.OnRoundRestart += OnRoundRestart;
        }

        private void OnDisable()
        {
            _gameManager.OnRoundRestart -= OnRoundRestart;
        }

        private void OnRoundRestart()
        {
            foreach (RecordTemplateDataContainer rec in _records)
            {
                Destroy(rec.gameObject);
            }
            _records.Clear();

            PlayerStateObserver[] players = _gameManager.Players;
            Array.Sort(players, (x, y) => -x.Score.CompareTo(y.Score));

            for (int i = 0; i < _showFirstCount && i < players.Length; i++)
            {
                RecordTemplateDataContainer record = Instantiate(_recordTemplate, _recordContainer.transform).GetComponent<RecordTemplateDataContainer>();
                record.Background.rectTransform.anchoredPosition = new Vector2(0, -((record.Background.rectTransform.rect.height + _stepBetweenRecords) * i));

                record.PlayerColor.color = players[i].PlayerColor;
                record.ScoreLabel.text = players[i].Score.ToString();
                record.gameObject.SetActive(true);
                _records.Add(record);
            }
        }
    }
}