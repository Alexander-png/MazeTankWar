using MazeWar.Base;
using MazeWar.PlayerBase.Observer;
using System;
using UnityEngine;

namespace MazeWar.MazeComponents
{
    public enum Direction { Up, Left, Right, Down }

    public class MazeCellData
    {
        private MazeCellDrawableBehaviour _thisCell = null;

        private MazeCellData _topNext = null;
        private MazeCellData _leftNext = null;
        private MazeCellData _rightNext = null;
        private MazeCellData _bottomNext = null;

        public bool HasTopWall { get; private set; } = true;
        public bool HasLeftWall { get; private set; } = true;
        public bool HasRightWall { get; private set; } = true;
        public bool HasBottomWall { get; private set; } = true;

        public bool TopWallBuilt { get; set; } = false;
        public bool LeftWallBuilt { get; set; } = false;
        public bool RightWallBuilt { get; set; } = false;
        public bool BottomWallBuilt { get; set; } = false;
        
        public MazeCellDrawableBehaviour ThisCell
        {
            get => _thisCell;
            set
            {
                if (_thisCell != null)
                    throw new InvalidOperationException("Maze cell data can only be set once.");
                _thisCell = value;
            }
        }

        public bool IsAnyPlayerHere { get; set; } = false;
        public bool IsAnyPickupHere { get; set; } = false;

        public static MazeCellData GetCell(MazeCellData origin, int colOffset, int rowOffset, int colRange, int rowRange)
        {
            MazeCellData res = origin;
            for (int i = 0; i < colOffset && i < rowRange - 1 && res != null; i++)
                res = res.GetNext(Direction.Right);
            for (int i = 0; i < rowOffset && i < colRange - 1 && res != null; i++)
                res = res.GetNext(Direction.Down);
            return res;
        }

        public void SetNext(Direction dir, MazeCellData next)
        {
            switch (dir)
            {
                case Direction.Up:
                    if (_topNext != null)
                        _topNext._bottomNext = null;
                    _topNext = next;
                    if (_topNext != null)
                        _topNext._bottomNext = this;
                    break;
                case Direction.Left:
                    if (_leftNext != null)
                        _leftNext._rightNext = null;
                    _leftNext = next;
                    if (_leftNext != null)
                        _leftNext._rightNext = this;
                    break;
                case Direction.Right:
                    if (_rightNext != null)
                        _rightNext._leftNext = null;
                    _rightNext = next;
                    if (_rightNext != null)
                        _rightNext._leftNext = this;
                    break;
                case Direction.Down:
                    if (_bottomNext != null)
                        _bottomNext._topNext = null;
                    _bottomNext = next;
                    if (_bottomNext != null)
                        _bottomNext._topNext = this;
                    break;
                default:
                    throw new ArgumentException($"Not defined value: {dir}");
            }
        }

        public MazeCellData GetNext(Direction dir)
        {
            switch (dir)
            {
                case Direction.Up:
                    return _topNext;
                case Direction.Left:
                    return _leftNext;
                case Direction.Right:
                    return _rightNext;
                case Direction.Down:
                    return _bottomNext;
            }
            throw new ArgumentException($"Not defined value: {dir}");
        }

        public void RemoveWall(Direction dir)
        {
            switch (dir)
            {
                case Direction.Up:
                    HasTopWall = false;
                    break;
                case Direction.Left:
                    HasLeftWall = false;
                    break;
                case Direction.Right:
                    HasRightWall = false;
                    break;
                case Direction.Down:
                    HasBottomWall = false;
                    break;
                default:
                    throw new ArgumentException($"Not defined value: {dir}");
            }
        }

        public void RemoveWallOpposite(Direction dir)
        {
            switch (dir)
            {
                case Direction.Up:
                    HasBottomWall = false;
                    break;
                case Direction.Left:
                    HasRightWall = false;
                    break;
                case Direction.Right:
                    HasLeftWall = false;
                    break;
                case Direction.Down:
                    HasTopWall = false;
                    break;
                default:
                    throw new ArgumentException($"Not defined value: {dir}");
            }
        }

        public void RestoreWalls()
        {
            HasTopWall = true;
            HasLeftWall = true;
            HasRightWall = true;
            HasBottomWall = true;
        }

        public bool CanEnterFrom(Direction dir)
        {
            switch (dir)
            {
                case Direction.Up:
                    return !HasBottomWall;
                case Direction.Down:
                    return !HasTopWall;
                case Direction.Right:
                    return !HasLeftWall;
                case Direction.Left:
                    return !HasRightWall;
                default:
                    throw new ArgumentException($"Not defined value: {dir}");
            }
        }
    }

    public class MazeCellDrawableBehaviour : MonoBehaviour
    {
        private PlayerStateObserver _currentPlayerObserver;
        private Pickup.Pickup _currentPickup;

        [SerializeField]
        private GameObject _topWall;
        [SerializeField]
        private GameObject _leftWall;
        [SerializeField]
        private GameObject _rightWall;
        [SerializeField]
        private GameObject _bottomWall;

        public MazeCellData Data { get; private set; }

        public void SetCellData(MazeCellData data, float cellSize)
        {
            gameObject.transform.localScale = new Vector3(cellSize, cellSize, cellSize);
            SetCellData(data);
        }

        public void SetCellData(MazeCellData data)
        {
            Data = data;
            Data.ThisCell = this;

            MazeCellData cachedCellData = Data.GetNext(Direction.Up);
            if (Data.HasTopWall && (cachedCellData == null || (cachedCellData.HasBottomWall && !cachedCellData.BottomWallBuilt)))
            {
                _topWall.SetActive(true);
                Data.TopWallBuilt = true;
            }
            cachedCellData = Data.GetNext(Direction.Left);
            if (Data.HasLeftWall && (cachedCellData == null || (cachedCellData.HasRightWall && !cachedCellData.RightWallBuilt)))
            {
                _leftWall.SetActive(true);
                Data.LeftWallBuilt = true;
            }
            cachedCellData = Data.GetNext(Direction.Right);
            if (Data.HasRightWall && (cachedCellData == null || (cachedCellData.HasLeftWall && !cachedCellData.LeftWallBuilt)))
            {
                _rightWall.SetActive(true);
                Data.RightWallBuilt = true;
            }
            cachedCellData = Data.GetNext(Direction.Down);
            if (Data.HasBottomWall && (cachedCellData == null || (cachedCellData.HasTopWall && !cachedCellData.TopWallBuilt)))
            {
                _bottomWall.SetActive(true);
                Data.BottomWallBuilt = true;
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out PlayerStateObserver observer))
            {
                _currentPlayerObserver = observer;
                Data.IsAnyPlayerHere = true;
            }
            if (collision.TryGetComponent(out Pickup.Pickup pick))
            {
                _currentPickup = pick;
                Data.IsAnyPickupHere = true;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (_currentPlayerObserver != null && collision.gameObject.Equals(_currentPlayerObserver.gameObject))
            {
                _currentPlayerObserver = null;
                Data.IsAnyPlayerHere = false;
            }
            if (_currentPickup != null && collision.gameObject.Equals(_currentPickup.gameObject))
            {
                Data.IsAnyPickupHere = false;
            }
        }
    }
}