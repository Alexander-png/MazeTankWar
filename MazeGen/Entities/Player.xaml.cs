using MazeGen.EntityComponents.Base;
using MazeGen.EntityComponents.Movement;
using MazeGen.EntityComponents.Movement.Base;
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

namespace MazeGen.Entities
{
    /// <summary>
    /// Логика взаимодействия для Player.xaml
    /// </summary>
    public partial class Player : MoveableEntity
    {
        public Player() : base()
        {
            InitializeComponent();
            Body.RenderTransform = Rott;
            Body.RenderTransformOrigin = new Point(0.5, 0.5);
            MovementBehaviour = new PlayerMovement(this) { MaxForwardSpeed = 0.3, MaxBackwardSpeed = -0.3, MaxRotationSpeed = 0.3 };
            BodyWidth = Width;
            BodyHeight = Height;
        }

        public void KeyUpN(KeyEventArgs e)
        {
            ((IPlayerMovementBehaviour)MovementBehaviour).OnKeyUp(e);
        }

        public void KeyDownN(KeyEventArgs e)
        {
            ((IPlayerMovementBehaviour)MovementBehaviour).OnKeyPress(e);
        }
    }
}
