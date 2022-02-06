using MazeGen.MazeComponents.Elements;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MazeGen.MazeComponents.Generating
{
    public class MazeGenerator
    {
        private const int MaxWidth = 12;
        private const int MinWidth = 4;
        private const int MaxHeight = 12;
        private const int MinHeight = 4;

        private static Random Rand = new Random();

        public static int LastWidth { get; private set; }
        public static int LastHeight { get; private set; }

        public static Tuple<Grid, MazeCell> GenerateMaze()
        {
            Tuple<Grid, MazeCell> graph = GenerateGrid();

            int randX = Rand.Next(0, LastWidth);
            int randY = Rand.Next(0, LastHeight);

            List<MazeCell> spanningTree = new List<MazeCell>();
            MazeCell startCell = MazeCell.GetCell(graph.Item2, randX, randY);
            
            // Adding it to the spanning tree.
            spanningTree.Add(startCell);

            int cellCount = LastHeight * LastWidth;
            Stack<MazeCell> visitedCells = new Stack<MazeCell>();
            MazeCell head = MazeCell.GetCell(graph.Item2, 0, 0);
            // Spanning tree will contain all the cells in grid.
            while (spanningTree.Count < cellCount)
            {
                MazeCell curr = null;
                // Select random cell not belongs to spanning tree.
                do
                {
                    randX = Rand.Next(0, LastWidth);
                    randY = Rand.Next(0, LastHeight);
                    curr = MazeCell.GetCell(head, randX, randY);
                }
                while (spanningTree.Contains(curr));
                visitedCells.Push(curr);

                // Detour grid cells untill reaching any element of spanning tree.
                do
                {
                    Direction dir = Direction.Up;
                    MazeCell next = null;

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
                                MazeCell cell = visitedCells.Pop();
                                cell.RestoreWalls();
                            }
                            break;
                        }

                        dir = (Direction)Rand.Next(0, 4);
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
                    MazeCell cell = visitedCells.Pop();
                    if (!spanningTree.Contains(cell))
                        spanningTree.Add(cell);
                }   
            }
            return graph;
        }

        static int CellCount = 0;

        private static MazeCell MakeNewCell(int col, int row)
        {
            MazeCell ret = new MazeCell();
            ret.Tag = CellCount;
            Grid.SetColumn(ret, col);
            Grid.SetRow(ret, row);
            CellCount += 1;
            return ret;
        }   

        public static Tuple<Grid, MazeCell> GenerateGrid()
        {
            CellCount = 0;

            int width = 3;// Rand.Next(MinWidth, MaxWidth + 1);
            int height = 3;// Rand.Next(MinHeight, MaxHeight + 1);

            // Remember generated width and height for further maze generation.
            LastHeight = height;
            LastWidth = width;

            Grid mazeContainer = new Grid();
            for (int i = 0; i < width; i++)
                mazeContainer.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
            for (int i = 0; i < height; i++)
                mazeContainer.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });

            MazeCell firstCell = null;
            MazeCell previousRowCell = null;
            MazeCell currentCell = null;

            for (int i = 0; i < height; i++)
            {
                if (i != 0)
                {
                    // Creating new cell for new row.
                    MazeCell newCell = MakeNewCell(0, i);
                    mazeContainer.Children.Add(newCell);
                    currentCell.SetNext(Direction.Down, newCell);

                    previousRowCell = currentCell;
                    currentCell = currentCell.GetNext(Direction.Down);
                }
                else
                {
                    // On beginning create remember first cell of the maze.
                    currentCell = MakeNewCell(0, 0);
                    mazeContainer.Children.Add(currentCell);
                    firstCell = currentCell;
                }

                // Begin from second iteration because first cell of current row already exists.
                for (int j = 1; j < width + 1; j++)
                {
                    if (j < width)
                    {
                        MazeCell newCell = MakeNewCell(j, i);
                        mazeContainer.Children.Add(newCell);
                        currentCell.SetNext(Direction.Right, newCell);
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
                    MazeCell prev = currentCell.GetNext(Direction.Left);
                    if (prev == null)
                        break;
                    currentCell = prev;
                }
            }
            return new Tuple<Grid, MazeCell>(mazeContainer, firstCell);
        }
    }
}