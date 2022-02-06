using MazeGen.EntityComponents.Base;
using MazeGen.MazeComponents.Generating;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MazeGen.MazeComponents.Elements
{
    public enum Direction { Up, Left, Right, Down }

    /// <summary>
    /// Логика взаимодействия для MazeCell.xaml
    /// </summary>
    public partial class MazeCell : BaseEntity
    {
        public const int Size = 100;

        private Dictionary<Direction, Line> Walls;

        public MazeCell TopNext { get; private set; }
        public MazeCell LeftNext { get; private set; }
        public MazeCell RightNext { get; private set; }
        public MazeCell BottomNext { get; private set; }

        public void SetNext(Direction dir, MazeCell next)
        {
            switch (dir)
            {
                case Direction.Up:
                    //SetNext(next, TopNext, TopNext?.BottomNext);
                    if (TopNext != null)
                        TopNext.BottomNext = null;
                    TopNext = next;
                    if (TopNext != null)
                        TopNext.BottomNext = this;
                    break;
                case Direction.Left:
                    //SetNext(next, LeftNext, LeftNext?.RightNext);
                    if (LeftNext != null)
                        LeftNext.RightNext = null;
                    LeftNext = next;
                    if (LeftNext != null)
                        LeftNext.RightNext = this;
                    break;
                case Direction.Right:
                    //SetNext(next, RightNext, RightNext?.LeftNext);
                    if (RightNext != null)
                        RightNext.LeftNext = null;
                    RightNext = next;
                    if (RightNext != null)
                        RightNext.LeftNext = this;
                    break;
                case Direction.Down:
                    //SetNext(next, BottomNext, BottomNext?.TopNext);
                    if (BottomNext != null)
                        BottomNext.TopNext = null;
                    BottomNext = next;
                    if (BottomNext != null)
                        BottomNext.TopNext = this;
                    break;
            }
        }

        //private void SetNext(MazeCell toSet, MazeCell target, MazeCell adjacent)
        //{
        //    if (target != null)
        //        adjacent = null;
        //    target = toSet;
        //    if (target != null)
        //        adjacent = this;
        //}

        public MazeCell GetNext(Direction dir)
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

        public override Rect GetBoxCollider()
        {
            return new Rect(Grid.GetColumn(this) * Size, Grid.GetRow(this) * Size, Size, Size);
        }

        public override bool CheckCollision(BaseEntity entity)
        {
            return false;
        }

        public static MazeCell GetCell(MazeCell head, int colNumber, int rowNumber)
        {
            MazeCell res = head;
            for (int i = 0; i < colNumber && i < MazeGenerator.LastWidth - 1 && res != null; i++)
                res = res.GetNext(Direction.Right);
            for (int i = 0; i < rowNumber && i < MazeGenerator.LastHeight - 1 && res != null; i++)
                res = res?.GetNext(Direction.Down);
            return res;
        }

        public void RemoveWall(Direction dir)
        {
            Walls[dir].Visibility = Visibility.Hidden;
        }

        public void RemoveWallOpposite(Direction dir)
        {
            Direction opposite;
            switch (dir)
            {
                case Direction.Up:
                    opposite = Direction.Down;
                    break;
                case Direction.Left:
                    opposite = Direction.Right;
                    break;
                case Direction.Right:
                    opposite = Direction.Left;
                    break;
                case Direction.Down:
                    opposite = Direction.Up;
                    break;
                default:
                    throw new ArgumentException();
            }
            Walls[opposite].Visibility = Visibility.Hidden;
        }

        public void RestoreWalls()
        {
            Walls[Direction.Up].Visibility = Visibility.Visible;
            Walls[Direction.Left].Visibility = Visibility.Visible;
            Walls[Direction.Right].Visibility = Visibility.Visible;
            Walls[Direction.Down].Visibility = Visibility.Visible;
        }

        protected override void CalculateCurrentCell()
        {
            // Empty for this class.
        }

        public MazeCell() : base()
        {
            InitializeComponent();
            Walls = new Dictionary<Direction, Line>()
            {
                { Direction.Up, TopWall },
                { Direction.Left, LeftWall },
                { Direction.Right, RightWall },
                { Direction.Down, BottomWall },
            };
        }

        ~MazeCell()
        {
            TopNext = null;
            LeftNext = null;
            RightNext = null;
            BottomNext = null;
        }
    }
}
