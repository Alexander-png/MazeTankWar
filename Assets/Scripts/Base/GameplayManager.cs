using MazeWar.Base.Abstractions;
using MazeWar.MazeComponents;
using MazeWar.MazeComponents.Base;
using MazeWar.PlayerBase.Observer;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MazeWar.Base
{
    public class GameplayManager : AbstractGameplayManager
    {
        private GlobalManager _globalManager;
        private MazeCellData _mazeHead;
        private Coroutine _restartRoundCoroutine;
        private PlayerStateObserver[] _players;

        [SerializeField]
        private MazeGenerator _mazeGenerator;
        [SerializeField]
        private PickupManager _pickupManager;
        [SerializeField]
        private Camera _camera;
        [SerializeField]
        private Transform _playerContainerTransfrom;

        [SerializeField]
        private float _roundRestartTime = 3f;

        public override MazeGenerator MazeGenerator => _mazeGenerator;
        public PickupManager PickupManager => _pickupManager;
        public override MazeCellData MazeHead => _mazeHead;

        public override int PlayersAliveCount
        { 
            get
            {
                int count = 0;
                for (int i = 0; i < _players.Length; i++)
                    if (_players[i].IsAlive)
                        count += 1;
                return count;
            }
        }

        public override PlayerStateObserver[] Players
        {
            get
            {
                PlayerStateObserver[] ret = new PlayerStateObserver[_players.Length];
                _players.CopyTo(ret, 0);
                return ret;
            }
        }

        private void Start()
        {
            _globalManager = GlobalManager.Instance;
            GlobalManager.GameplayManager = this;
            _pickupManager.Init();
            Initialize();
        }

        private void Initialize()
        {
            _players = new PlayerStateObserver[_playerContainerTransfrom.childCount];
            for (int i = 0; i < _players.Length; i++)
            {
                _players[i] = _playerContainerTransfrom.GetChild(i).GetComponent<PlayerStateObserver>();
                _players[i].OnKilled += OnPlayerKilled;
            }   
        }

        private IEnumerator RestartRoundDelay(float delay)
        {
            while (delay > 0)
            {
                delay -= Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            RestartRound();
        }

        private void AddScoreToAlivePlayer()
        {
            for (int i = 0; i < Players.Length; i++)
            {
                if (Players[i].IsAlive)
                    Players[i].Score += 1;
            }
        }

        private void RestartRound()
        {
            InGame = false;
            _pickupManager.StopSpawningPickups();
            AddScoreToAlivePlayer();
            InvokeRestartRound();
            if (MazeHead != null)
                ClearMaze();
            _mazeHead = _mazeGenerator.GenerateMaze();
            CenterCameraAndZoom();
            MovePlayersToRandomCell();
            _pickupManager.StartSpawninigPickups();
            InGame = true;
        }

        public void OnPlayerKilled(PlayerStateObserver sender)
        {
            if (InGame)
            {
                sender.PlayExplosionAnimation();
                if (PlayersAliveCount <= 1)
                {
                    if (_restartRoundCoroutine != null)
                        StopCoroutine(_restartRoundCoroutine);
                    _restartRoundCoroutine = StartCoroutine(RestartRoundDelay(_roundRestartTime));
                }
            }
        }

        private void ClearMaze()
        {
            List<MazeCellData> firstRow = new List<MazeCellData>(_mazeGenerator.LastCellCountInRow);
            MazeCellData currentCell = MazeHead;
            while (true)
            {
                if (currentCell == null)
                    break;
                firstRow.Add(currentCell);
                currentCell = currentCell.GetNext(Direction.Right);
            }
            Stack<MazeCellData> col = new Stack<MazeCellData>(_mazeGenerator.LastCellCountInColumn);
            for (int i = 0; i < firstRow.Count; i++)
            {
                MazeCellData currCell = firstRow[i];
                while (true)
                {
                    if (currCell == null)
                        break;
                    col.Push(currCell);
                    currCell = currCell.GetNext(Direction.Down);
                }
                while (col.Count > 0)
                    Destroy(col.Pop().ThisCell.gameObject);
            }
        }

        private void CenterCameraAndZoom()
        {
            int cellsInRow = _mazeGenerator.LastCellCountInRow;
            int cellsInCol = _mazeGenerator.LastCellCountInColumn;

            float mazeWidth = cellsInRow * _mazeGenerator.CellSize;
            float mazeHeight = cellsInCol * _mazeGenerator.CellSize;
            float newCameraX = mazeWidth / 2 + _mazeGenerator.HeadCellPosition.x;
            float newCameraY = -(mazeHeight / 2 + _mazeGenerator.HeadCellPosition.y);

            // Adding this radius because center of the maze cell is in the center of its sprite, 
            // but not on top left corner.
            float cellCenterToEdgeRadius = _mazeGenerator.CellSize / 2;
            _camera.transform.position = new Vector3(newCameraX - cellCenterToEdgeRadius, newCameraY + cellCenterToEdgeRadius, -Mathf.Max(cellsInRow, cellsInCol) * _mazeGenerator.CellSize);
        }

        private void MovePlayersToRandomCell()
        {
            int randX;
            int randY;
            MazeCellData cell;
            List<Tuple<int, int>> playerCorrdsMap = new List<Tuple<int, int>>(Players.Length);

            for (int i = 0; i < Players.Length; i++)
            {
                if (Players[i] != null)
                {
                    while (true)
                    {
                        randX = UnityEngine.Random.Range(0, _mazeGenerator.LastCellCountInRow);
                        randY = UnityEngine.Random.Range(0, _mazeGenerator.LastCellCountInColumn);
                        cell = MazeCellData.GetCell(MazeHead, randX, randY, _mazeGenerator.LastCellCountInColumn, _mazeGenerator.LastCellCountInRow);

                        Tuple<int, int> match = playerCorrdsMap.Find(c => c.Item1 == randX && c.Item2 == randY);
                        if (match == null)
                        {
                            Players[i].transform.position = cell.ThisCell.transform.position;
                            Players[i].transform.Rotate(new Vector3(0, 0, UnityEngine.Random.Range(0, 361)));
                            Players[i].IsAlive = true;
                            playerCorrdsMap.Add(new Tuple<int, int>(randX, randY));
                            break;
                        }
                    }
                }
            }
        }

        // Todo: remove on release
        private void OnGenerateMazeDebug()
        {
            RestartRound();
        }
    }
}
