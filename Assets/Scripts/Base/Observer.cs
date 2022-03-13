using MazeWar.MazeComponents;
using System.Collections.Generic;
using UnityEngine;

namespace MazeWar.Base
{
    public class Observer : MonoBehaviour
    {
        private MazeCellData MazeHead;

        [SerializeField]
        private Generator MazeGenerator;
        [SerializeField]
        private Camera Camera;
        [SerializeField]
        private GameObject Player;



        void Update()
        {
            if (Input.GetKeyDown(KeyCode.G))
            {
                if (MazeHead != null)
                    ClearMaze();
                MazeHead = MazeGenerator.GenerateMaze();
                CenterCameraAndZoom();
                MovePlayerToRandomCell();
            }
        }

        private void ClearMaze()
        {
            List<MazeCellData> firstRow = new List<MazeCellData>(MazeGenerator.LastCellCountInRow);
            MazeCellData currentCell = MazeHead;
            while (true)
            {
                if (currentCell == null)
                    break;
                firstRow.Add(currentCell);
                currentCell = currentCell.GetNext(Direction.Right);
            }
            Stack<MazeCellData> col = new Stack<MazeCellData>(MazeGenerator.LastCellCountInColumn);
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
            int cellsInRow = MazeGenerator.LastCellCountInRow;
            int cellsInCol = MazeGenerator.LastCellCountInColumn;

            float mazeWidth = cellsInRow * MazeGenerator.CellSize;
            float mazeHeight = cellsInCol * MazeGenerator.CellSize;
            float newCameraX = mazeWidth / 2 + MazeGenerator.HeadCellPosition.x;
            float newCameraY = -(mazeHeight / 2 + MazeGenerator.HeadCellPosition.y);

            // Adding this radius because center of the maze cell is in the center of its sprite, 
            // but not on top left corner.
            float cellCenterToEdgeRadius = MazeGenerator.CellSize / 2;
            Camera.transform.position = new Vector3(newCameraX - cellCenterToEdgeRadius, newCameraY + cellCenterToEdgeRadius, -Mathf.Max(cellsInRow, cellsInCol) * MazeGenerator.CellSize);
        }

        private void MovePlayerToRandomCell()
        {
            int randX = Random.Range(0, MazeGenerator.LastCellCountInRow);
            int randY = Random.Range(0, MazeGenerator.LastCellCountInColumn);
            MazeCellData cell = MazeCellData.GetCell(MazeHead, randX, randY, MazeGenerator.LastCellCountInColumn, MazeGenerator.LastCellCountInRow);

            Player.transform.position = new Vector3(cell.ThisCell.transform.position.x, cell.ThisCell.transform.position.y, MazeGenerator.HeadCellPosition.z);
            //int randRotation = Random.Range(0, 360);
            //Player.transform.rotation = new Quaternion();// randRotation;
        }
    }
}
