using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MazeWar.MazeComponents.Base
{
    public abstract class MazeGenerator : MonoBehaviour
    {
        public int LastCellCountInRow { get; protected set; }
        public int LastCellCountInColumn { get; protected set; }
        public abstract Vector3 HeadCellPosition { get; }
        public abstract float CellSize { get; }
        public abstract MazeCellData GenerateMaze();
    }
}