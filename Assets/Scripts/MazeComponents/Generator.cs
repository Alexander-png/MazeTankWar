using System.Collections.Generic;
using UnityEngine;

namespace MazeWar.MazeComponents
{
    public class Generator : MonoBehaviour
    {
        [SerializeField]
        private int MaxMazeXSize;
        [SerializeField]
        private int MinMazeXSize;
        [SerializeField]
        private int MaxMazeYSize;
        [SerializeField]
        private int MinMazeYSize;

        [SerializeField]
        private GameObject MazeCellDrawablePrefab;
        [SerializeField]
        public Vector3 HeadCellPosition;
        [SerializeField]
        public float CellSize;

        public int LastCellCountInRow { get; private set; }
        public int LastCellCountInColumn { get; private set; }

        public MazeCellData GenerateMaze()
        {
            MazeCellData head = GenerateMazeData();
            Vector3 currentPosition = HeadCellPosition;
            MazeCellData currentCell = null;

            for (int i = 0; i < LastCellCountInColumn; i++)
            {
                if (currentCell == null)
                    currentCell = head;
                else
                {
                    while (true)
                    {
                        MazeCellData prev = currentCell.GetNext(Direction.Left);
                        if (prev == null)
                        {
                            currentPosition.y -= CellSize;
                            break;
                        }
                        currentCell = prev;
                    }
                }
                for (int j = 0; j < LastCellCountInRow; j++)
                {
                    Instantiate(MazeCellDrawablePrefab, currentPosition, Quaternion.identity).GetComponent<MazeCellDrawableBehaviour>().SetCellData(currentCell, CellSize);
                    MazeCellData next = currentCell.GetNext(Direction.Right);
                    if (next != null)
                        currentCell = next;
                    currentPosition.x += CellSize;
                }
                currentCell = currentCell.GetNext(Direction.Down);
                currentPosition.x = HeadCellPosition.x;
            }
            return head;
        }

        private MazeCellData GenerateMazeData()
        {
            MazeCellData head = GenerateMazeCells();
            int cellCount = LastCellCountInColumn * LastCellCountInRow;

            int randX = Random.Range(0, LastCellCountInRow);
            int randY = Random.Range(0, LastCellCountInColumn);
            List<MazeCellData> spanningTree = new List<MazeCellData>(cellCount);

            // Getting random start cell
            MazeCellData startCell = MazeCellData.GetCell(head, randX, randY, LastCellCountInColumn, LastCellCountInRow);
            // Adding it to the spanning tree.
            spanningTree.Add(startCell);
            Stack<MazeCellData> visitedCells = new Stack<MazeCellData>();
            while (spanningTree.Count < cellCount)
            {
                MazeCellData curr;
                // Select random cell not belongs to spanning tree.
                do
                {
                    randX = Random.Range(0, LastCellCountInRow);
                    randY = Random.Range(0, LastCellCountInColumn);
                    curr = MazeCellData.GetCell(head, randX, randY, LastCellCountInColumn, LastCellCountInRow);
                }
                while (spanningTree.Contains(curr));

                visitedCells.Push(curr);

                // Detour grid cells untill reaching any element of spanning tree.
                do
                {
                    Direction dir = Direction.Up;
                    MazeCellData next = null;

                    Dictionary<Direction, bool> checkedDirs = new Dictionary<Direction, bool>
                    {
                        { Direction.Up, false },
                        { Direction.Left, false },
                        { Direction.Down, false },
                        { Direction.Right, false },
                    };
                    // Selecting non-null next cell.
                    do
                    {
                        // If no possible moves then try again
                        if (!checkedDirs.ContainsValue(false))
                        {
                            while (visitedCells.Count > 0)
                            {
                                MazeCellData cell = visitedCells.Pop();
                                cell.RestoreWalls();
                            }
                            break;
                        }

                        dir = (Direction)Random.Range(0, 4);
                        checkedDirs[dir] = true;
                        next = curr.GetNext(dir);
                        if (visitedCells.Contains(next))
                            next = null;
                    }
                    while (next == null);

                    if (next == null)
                        break;

                    // If current sub-graph not contains next cell.
                    if (!visitedCells.Contains(next))
                    {
                        curr.RemoveWall(dir);
                        next.RemoveWallOpposite(dir);
                        visitedCells.Push(next);
                        curr = next;
                    }
                }
                while (!spanningTree.Contains(curr));

                // Add all visited cells to spanning tree.
                while (visitedCells.Count > 0)
                {
                    MazeCellData cell = visitedCells.Pop();
                    if (!spanningTree.Contains(cell))
                        spanningTree.Add(cell);
                }
            }
            return head;
        }

        private MazeCellData GenerateMazeCells()
        {
            int width = Random.Range(MinMazeXSize, MaxMazeXSize);
            int height = Random.Range(MinMazeYSize, MaxMazeYSize);

            //width = 4;
            //height = 4;
            //width = 12;
            //height = 12;

            // Remember generated width and height for further maze generation.
            LastCellCountInColumn = height;
            LastCellCountInRow = width;

            MazeCellData firstCell = null;
            MazeCellData previousRowCell = null;
            MazeCellData currentCell = null;

            for (int i = 0; i < height; i++)
            {
                if (i == 0)
                {
                    // On beginning create first cell of the maze and remember it.
                    currentCell = new MazeCellData();
                    firstCell = currentCell;
                }
                else
                {
                    // Creating new cell for new row.
                    currentCell.SetNext(Direction.Down, new MazeCellData());
                    previousRowCell = currentCell;
                    currentCell = currentCell.GetNext(Direction.Down);
                }

                // Begin from second iteration because first cell of current row already exists.
                for (int j = 1; j < width + 1; j++)
                {
                    if (j < width)
                    {
                        currentCell.SetNext(Direction.Right, new MazeCellData());
                        if (i != 0)
                        {
                            // Setting nexts on down if not on first row.
                            previousRowCell.SetNext(Direction.Down, currentCell);
                            previousRowCell = previousRowCell.GetNext(Direction.Right);
                        }
                        currentCell = currentCell.GetNext(Direction.Right);
                    }
                    else
                    {
                        if (i != 0)
                            previousRowCell.SetNext(Direction.Down, currentCell);
                    }
                }
                // Moving backward to beginning of the row.
                while (true)
                {
                    MazeCellData prev = currentCell.GetNext(Direction.Left);
                    if (prev == null)
                        break;
                    currentCell = prev;
                }
            }
            return firstCell;
        }
    }
}