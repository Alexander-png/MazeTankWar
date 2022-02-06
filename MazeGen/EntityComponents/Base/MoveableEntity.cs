using MazeGen.EntityComponents.Movement.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace MazeGen.EntityComponents.Base
{
    public class MoveableEntity : BaseEntity
    {
        protected RotateTransform Rott = new RotateTransform();

        private double _rotation;
        public double Rotation
        {
            get => _rotation;
            set
            {
                if (value > 360)
                    value = 0;
                if (value < 0)
                    value = 360;
                _rotation = value;
                Dispatcher.Invoke(() => Rott.Angle = _rotation);
            }
        }

        public double CurrentSpeed;
        public double CurrentRotationSpeed;
        protected IBaseMovementBehaviour MovementBehaviour;

        public override Rect GetBoxCollider()
        {
            Rect collider = new Rect(X, Y, Width, Height);
            collider.Transform(Rott.Value);
            return collider;
        }

        public MoveableEntity()
        {

        }

        public MoveableEntity(double x, double y, double rot, double height, double width) : base(x, y, height, width)
        {
            Rotation = rot;
        }
    }
}
