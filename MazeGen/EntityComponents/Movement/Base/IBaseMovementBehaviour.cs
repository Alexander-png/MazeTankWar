using MazeGen.EntityComponents.Base;
using MazeGen.Global;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace MazeGen.EntityComponents.Movement.Base
{
    public interface IBaseMovementBehaviour
    {
        MoveableEntity MoveableEntity { get; set; }
        GameLoop GameLoop { get; set; }
        void OnEntityDisposed();
        void GameLoop_FixedUpdate(double elapsed);
    }

    public interface IPlayerMovementBehaviour : IBaseMovementBehaviour
    {
        public double MaxForwardSpeed { get; set; }
        public double MaxBackwardSpeed { get; set; }
        public double MaxRotationSpeed { get; set; }
        void OnKeyPress(KeyEventArgs e);
        void OnKeyUp(KeyEventArgs e);

    }

    public interface IBulletMovementBehaviour : IBaseMovementBehaviour
    {

    }
}
