using MazeWar.MazeComponents;
using MazeWar.MazeComponents.Base;
using MazeWar.PlayerBase.Observer;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// todo: UI

namespace MazeWar.Base
{
    public class GameplayManager : MonoBehaviour
    {
        private GlobalManager GlobalManager;
        public bool InGame { get; private set; } = false;
        public MazeCellData MazeHead { get; private set; }
        public EventHandler<EventArgs> OnRoundRestart;

        [SerializeField]
        private MazeGenerator _mazeGenerator;
        [SerializeField]
        private PickupManager _pickupManager;
        [SerializeField]
        private Camera Camera;
        [SerializeField]
        private PlayerStateObserver[] Players;
        [SerializeField]
        private float _roundRestartTime = 3f;

        public MazeGenerator MazeGenerator => _mazeGenerator;
        public PickupManager PickupManager => _pickupManager;

        private int _PlayersAliveCount;
        public int PlayersAliveCount
        {
            get => _PlayersAliveCount;
            private set
            {
                _PlayersAliveCount = value;
                if (InGame)
                {
                    if (_PlayersAliveCount <= 1)
                    {
                        if (RestartRoundCoroutine != null)
                            StopCoroutine(RestartRoundCoroutine);
                        RestartRoundCoroutine = StartCoroutine(RestartRoundDelay(_roundRestartTime));
                    }
                }
            }
        }

        private void Start()
        {
            GlobalManager = GlobalManager.Instance;
            GlobalManager.GameplayManager = this;
            _pickupManager.Init();
        }

        private Coroutine RestartRoundCoroutine;
        private IEnumerator RestartRoundDelay(float delay)
        {
            WaitForEndOfFrame wait = new WaitForEndOfFrame();
            while (delay > 0)
            {
                delay -= Time.deltaTime;
                yield return wait;
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
            OnRoundRestart?.Invoke(this, EventArgs.Empty);
            AddScoreToAlivePlayer();
            PlayersAliveCount = 0;
            if (MazeHead != null)
                ClearMaze();
            MazeHead = _mazeGenerator.GenerateMaze();
            CenterCameraAndZoom();
            MovePlayersToRandomCell();
            _pickupManager.StartSpawninigPickups();
            InGame = true;
        }

        public void OnPlayerKilled()
        {
            PlayersAliveCount -= 1;
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
            Camera.transform.position = new Vector3(newCameraX - cellCenterToEdgeRadius, newCameraY + cellCenterToEdgeRadius, -Mathf.Max(cellsInRow, cellsInCol) * _mazeGenerator.CellSize);
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
                            PlayersAliveCount += 1;
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
