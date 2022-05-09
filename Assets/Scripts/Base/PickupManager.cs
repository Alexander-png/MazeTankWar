using MazeWar.Base.Abstractions;
using MazeWar.MazeComponents;
using MazeWar.Pickup.Scriptable;
using System.Collections;
using UnityEngine;

namespace MazeWar.Base
{
    public class PickupManager : MonoBehaviour
    {
        private AbstractGameplayManager _gameplayManager;
        private int _possiblePickupCountInTheMaze;
        private int _currentPickupCountInTheMaze;

        [SerializeField]
        private Pickup.Pickup _pickupPrefab;
        [SerializeField]
        private PickupData[] _pataPickups;
        [SerializeField]
        private float _pickupSpawnDelay;

        private Coroutine _pickupSpawningRepeatCoroutine;

        public void Init()
        {
            _gameplayManager = GlobalManager.GameplayManager;
            _gameplayManager.OnRoundRestart += OnRoundRestart;
        }

        private void OnDisable()
        {
            _gameplayManager.OnRoundRestart -= OnRoundRestart;
        }

        private void OnRoundRestart()
        {
            _currentPickupCountInTheMaze = 0;
        }

        public void StartSpawninigPickups()
        {
            _possiblePickupCountInTheMaze = _gameplayManager.MazeGenerator.LastCellCountInRow * _gameplayManager.MazeGenerator.LastCellCountInColumn - _gameplayManager.PlayersAliveCount;
            _pickupSpawningRepeatCoroutine = StartCoroutine(SpawnPickupsDelay(_pickupSpawnDelay));
        }

        public void StopSpawningPickups()
        {
            if (_pickupSpawningRepeatCoroutine != null)
                StopCoroutine(_pickupSpawningRepeatCoroutine);
        }

        private IEnumerator SpawnPickupsDelay(float delay)
        {
            int randX;
            int randY;
            int randomPickupDataIndex;
            int lastCellCountinRow = _gameplayManager.MazeGenerator.LastCellCountInRow;
            int lastCellCountinColumn = _gameplayManager.MazeGenerator.LastCellCountInColumn;

            MazeCellData cell;
            while (true)
            {
                yield return new WaitForSeconds(delay);
                if (_currentPickupCountInTheMaze < _possiblePickupCountInTheMaze)
                {
                    while (true)
                    {
                        randX = Random.Range(0, lastCellCountinRow);
                        randY = Random.Range(0, lastCellCountinColumn);
                        cell = MazeCellData.GetCell(_gameplayManager.MazeHead, randX, randY, lastCellCountinColumn, lastCellCountinRow);
                        if (!cell.IsAnyPickupHere && !cell.IsAnyPlayerHere)
                        {
                            //randomPickupDataIndex = Random.Range(0, DataPickups.Length);
                            //randomPickupDataIndex = 0; // Debug machine gun
                            randomPickupDataIndex = 1; // Debug shotgun
                            //randomPickupDataIndex = 2; // Debug explosive 
                            Pickup.Pickup pick = Instantiate(_pickupPrefab, cell.ThisCell.transform).GetComponent<Pickup.Pickup>();
                            pick.SetPickupData(_pataPickups[randomPickupDataIndex]);
                            pick.OnPicked += OnPickupPicked;
                            _currentPickupCountInTheMaze += 1;
                            break;
                        }
                    }
                }
            }
        }

        public void OnPickupPicked()
        {
            _currentPickupCountInTheMaze -= 1;
        }
    }
}