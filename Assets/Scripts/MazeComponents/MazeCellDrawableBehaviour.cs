using MazeWar.Base;
using System;
using UnityEngine;

namespace MazeWar.MazeComponents
{
    public enum Direction { Up, Left, Right, Down }

    public class MazeCellData
    {
        private MazeCellData TopNext = null;
        private MazeCellData LeftNext = null;
        private MazeCellData RightNext = null;
        private MazeCellData BottomNext = null;

        public bool HasTopWall { get; private set; } = true;
        public bool HasLeftWall { get; private set; } = true;
        public bool HasRightWall { get; private set; } = true;
        public bool HasBottomWall { get; private set; } = true;

        public bool TopWallBuilt = false;
        public bool LeftWallBuilt = false;
        public bool RightWallBuilt = false;
        public bool BottomWallBuilt = false;

        private MazeCellDrawableBehaviour _ThisCell = null;
        public MazeCellDrawableBehaviour ThisCell
        {
            get => _ThisCell;
            set
            {
                if (_ThisCell != null)
                    throw new InvalidOperationException("Maze cell data can only be set once.");
                _ThisCell = value;
            }
        }

        public bool IsAnyPlayerHere = false;
        public bool IsAnyPickupHere = false;

        public static MazeCellData GetCell(MazeCellData mazeHead, int colNumber, int rowNumber, int cellsInCol, int cellsInRow)
        {
            MazeCellData res = mazeHead;
            for (int i = 0; i < colNumber && i < cellsInRow - 1 && res != null; i++)
                res = res.GetNext(Direction.Right);
            for (int i = 0; i < rowNumber && i < cellsInCol - 1 && res != null; i++)
                res = res.GetNext(Direction.Down);
            return res;
        }

        public void SetNext(Direction dir, MazeCellData next)
        {
            switch (dir)
            {
                case Direction.Up:
                    if (TopNext != null)
                        TopNext.BottomNext = null;
                    TopNext = next;
                    if (TopNext != null)
                        TopNext.BottomNext = this;
                    break;
                case Direction.Left:
                    if (LeftNext != null)
                        LeftNext.RightNext = null;
                    LeftNext = next;
                    if (LeftNext != null)
                        LeftNext.RightNext = this;
                    break;
                case Direction.Right:
                    if (RightNext != null)
                        RightNext.LeftNext = null;
                    RightNext = next;
                    if (RightNext != null)
                        RightNext.LeftNext = this;
                    break;
                case Direction.Down:
                    if (BottomNext != null)
                        BottomNext.TopNext = null;
                    BottomNext = next;
                    if (BottomNext != null)
                        BottomNext.TopNext = this;
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
                    return TopNext;
                case Direction.Left:
                    return LeftNext;
                case Direction.Right:
                    return RightNext;
                case Direction.Down:
                    return BottomNext;
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
        private PickupManager PickupManager;

        [SerializeField]
        private GameObject TopWall;
        [SerializeField]
        private GameObject LeftWall;
        [SerializeField]
        private GameObject RightWall;
        [SerializeField]
        private GameObject BottomWall;

        public MazeCellData Data { get; private set; }

        private void Awake()
        {
            PickupManager = GlobalManager.GameplayManager.PickupManager;
        }

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
                TopWall.SetActive(true);
                Data.TopWallBuilt = true;
            }
            cachedCellData = Data.GetNext(Direction.Left);
            if (Data.HasLeftWall && (cachedCellData == null || (cachedCellData.HasRightWall && !cachedCellData.RightWallBuilt)))
            {
                LeftWall.SetActive(true);
                Data.LeftWallBuilt = true;
            }
            cachedCellData = Data.GetNext(Direction.Right);
            if (Data.HasRightWall && (cachedCellData == null || (cachedCellData.HasLeftWall && !cachedCellData.LeftWallBuilt)))
            {
                RightWall.SetActive(true);
                Data.RightWallBuilt = true;
            }
            cachedCellData = Data.GetNext(Direction.Down);
            if (Data.HasBottomWall && (cachedCellData == null || (cachedCellData.HasTopWall && !cachedCellData.TopWallBuilt)))
            {
                BottomWall.SetActive(true);
                Data.BottomWallBuilt = true;
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag == "Player")
                Data.IsAnyPlayerHere = true;
            if (collision.tag == "Pickup")
                Data.IsAnyPickupHere = true;
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.tag == "Player")
                Data.IsAnyPlayerHere = false;
            if (collision.tag == "Pickup")
            {
                Data.IsAnyPickupHere = false;
                PickupManager.OnPickupPicked();
            }
        }
    }
}