using MazeGen.Global;
using MazeGen.MazeComponents.Elements;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MazeGen.EntityComponents.Base
{
    public partial class BaseEntity : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        protected delegate void FieldChangedEvents(bool value);
        protected event FieldChangedEvents DestroyedChanged;

        private string _imageSource;
        public string ImageSource
        {
            get => _imageSource;
            set
            {
                _imageSource = value;
                OnPropertyChanged();
            }
        }

        private double _x;
        private double _y;
        
        public double X
        {
            get => _x;
            set
            {
                _x = value;
                SetPosition(X, Y);
            }
        }
        public double Y
        {
            get => _y;
            set
            {
                _y = value;
                SetPosition(X, Y);
            }
        }

        protected virtual void SetPosition(double x, double y)
        {
            Dispatcher.Invoke(() =>
            {
                Canvas.SetLeft(this, x - (BodyWidth / 2));
                Canvas.SetTop(this, y - (BodyHeight / 2));
            });
            CalculateCurrentCell();
        }

        public MazeCell MazeHead;
        //public MazeCell CurrentCell { get; protected set; }
        public List<MazeCell> IntersectedCells { get; protected set; }

        protected virtual void CalculateCurrentCell()
        {
            int cellX = Convert.ToInt32(X) / MazeCell.Size;
            int cellY = Convert.ToInt32(Y) / MazeCell.Size;

            // If on first time then find cell in which it is
            if (IntersectedCells == null)
            {
                IntersectedCells = new List<MazeCell>();

                MazeCell newCell = MazeCell.GetCell(MazeHead, cellX, cellY);
                Dispatcher.Invoke(() =>
                {
                    if (CurrentCell != null)
                        CurrentCell.Background = Brushes.White;
                    CurrentCell = newCell;
                    CurrentCell.Background = Brushes.Blue;
                });

            }
            else
            {
                Direction nextHorzDir = _x % 100 > 50 ? Direction.Right : Direction.Left;
                Direction nextVertDir = _y % 100 > 50 ? Direction.Down : Direction.Up;

                MazeCell nextHorzCell = CurrentCell.GetNext(nextHorzDir);
                MazeCell nextVertCell = CurrentCell.GetNext(nextVertDir);

            }
            Dispatcher.Invoke(() => (App.Current.MainWindow as MainWindow).ShowParams(_x, _y, cellX, cellY)); // Debug


            //public MazeCell CurrentCell { get; protected set; }

            //int cellX = Convert.ToInt32(X) / MazeCell.Size;// % MazeCell.Size;
            //int cellY = Convert.ToInt32(Y) / MazeCell.Size;// % MazeCell.Size;
            //MazeCell newCell = MazeCell.GetCell(MazeHead, cellX, cellY);
            //Dispatcher.Invoke(() =>
            //{
            //    if (CurrentCell != null)
            //        CurrentCell.Background = Brushes.White;
            //    CurrentCell = newCell;
            //    CurrentCell.Background = Brushes.Blue;
            //    (App.Current.MainWindow as MainWindow).ShowParams(_x, _y, cellX, cellY); // Debug
            //});
        }

        public virtual bool CheckCollision(BaseEntity entity)
        {
            return GetBoxCollider().IntersectsWith(entity.GetBoxCollider());
        }

        private bool _Destroyed;
        public bool Destroyed
        {
            get => _Destroyed;
            set
            {
                _Destroyed = value;
                DestroyedChanged?.Invoke(_Destroyed);
                if (_Destroyed)
                    RemoveFromScreen();
            }
        }

        public double BodyHeight;
        public double BodyWidth;

        public bool HasRigidBody { get; set; }

        public virtual Rect GetBoxCollider()
        {
            return new Rect(X, Y, BodyWidth, BodyHeight);
        }

        public ProgramObserver Observer;
        //public GameHandler GameHandler;
        public GameLoop GameLoop;

        protected virtual void RemoveFromScreen()
        {
            //(Observer.CurrentHandler as InGameWindowHandler).GameHandler.RemoveEntityFromScreen(this);
        }

        protected virtual void Clear()
        {

        }

        public BaseEntity()
        {
            Observer = ProgramObserver.GetInstance();
            //GameHandler = (Observer.CurrentHandler as InGameWindowHandler).GameHandler;
            GameLoop = GameLoop.GetInstance();
        }

        public BaseEntity(double x, double y) : this()
        {
            X = x;
            Y = y;
        }

        public BaseEntity(double x, double y, double height, double width) : this(x, y)
        {
            BodyHeight = height;
            BodyWidth = width;
        }

        ~BaseEntity()
        {
            Clear();
        }
    }
}