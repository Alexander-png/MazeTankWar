using MazeGen.EntityComponents.Base;
using MazeGen.EntityComponents.Movement.Base;
using MazeGen.Global;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace MazeGen.EntityComponents.Movement
{
    public class PlayerMovement : IPlayerMovementBehaviour
    {
        public MoveableEntity MoveableEntity { get; set; }
        public double MaxForwardSpeed { get; set; }
        public double MaxBackwardSpeed { get; set; }
        public GameLoop GameLoop { get; set; }
        public double MaxRotationSpeed { get; set; }

        public PlayerMovement(MoveableEntity entity)
        {
            MoveableEntity = entity;
            GameLoop = GameLoop.GetInstance();
            GameLoop.FixedTick += GameLoop_FixedUpdate;
        }

        ~PlayerMovement()
        {
            GameLoop.FixedTick -= GameLoop_FixedUpdate;
        }

        public void GameLoop_FixedUpdate(double elapsed)
        {
            if (MoveableEntity.CurrentSpeed != 0)
            {
                MoveableEntity.X += (MoveableEntity.CurrentSpeed * Math.Cos((MoveableEntity.Rotation - 90) * Math.PI / 180)) * elapsed;
                MoveableEntity.Y += (MoveableEntity.CurrentSpeed * Math.Sin((MoveableEntity.Rotation - 90) * Math.PI / 180)) * elapsed;
            }
            if (MoveableEntity.CurrentRotationSpeed != 0)
                MoveableEntity.Rotation += MoveableEntity.CurrentRotationSpeed * elapsed;
        }

        public void OnEntityDisposed()
        {
            
        }

        bool LeftArrowPress = false;
        bool RightArrowPress = false;
        bool UpArrowPress = false;
        bool DownArrowPress = false;

        public void OnKeyPress(KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Left:
                    LeftArrowPress = true;
                    MoveableEntity.CurrentRotationSpeed = -MaxRotationSpeed;
                    break;
                case Key.Right:
                    RightArrowPress = true;
                    MoveableEntity.CurrentRotationSpeed = MaxRotationSpeed;
                    break;
                case Key.Up:
                    UpArrowPress = true;
                    MoveableEntity.CurrentSpeed = MaxForwardSpeed;
                    break;
                case Key.Down:
                    DownArrowPress = true;
                    MoveableEntity.CurrentSpeed = MaxBackwardSpeed;
                    break;
                case Key.M:

                    break;
            }
        }

        public void OnKeyUp(KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Left:
                    LeftArrowPress = false;
                    MoveableEntity.CurrentRotationSpeed = !RightArrowPress ? 0 : MaxRotationSpeed;
                    break;
                case Key.Right:
                    RightArrowPress = false;
                    MoveableEntity.CurrentRotationSpeed = !LeftArrowPress ? 0 : -MaxRotationSpeed;
                    break;
                case Key.Up:
                    UpArrowPress = false;
                    MoveableEntity.CurrentSpeed = !DownArrowPress ? 0 : MaxBackwardSpeed;
                    break;
                case Key.Down:
                    DownArrowPress = false;
                    MoveableEntity.CurrentSpeed = !UpArrowPress ? 0 : MaxForwardSpeed;
                    break;
            }
        }
    }
}
