using MazeWar.PlayerBase.Movement.Base;
using UnityEngine;

namespace MazeWar.PlayerBase.Movement
{
    public class PlayerMovementKeyboard : BaseMovementBehaviour
    {
        private void FixedUpdate()
        {
            MovementLogic();
        }

        // Using Axis vals because listening input actions in Dynamic update, but
        // moving physical body in FixedUpdate.
        private void MovementLogic()
        {
            float verticalAxisVal = _inputManager.VerticalAxis;
            float horizontalAxisVal = _inputManager.HorizontalAxis;

            // Rotating body
            PlayerBody.angularVelocity = -RotationSpeed * horizontalAxisVal * Time.fixedDeltaTime;

            float currentSpeed = 0;
            if (verticalAxisVal > 0)
                currentSpeed = ForwardSpeed;
            else if (verticalAxisVal < 0)
                currentSpeed = BackwardSpeed;
            currentSpeed *= Time.fixedDeltaTime;
            // Moving body
            float bodyRotationAsRadian = PlayerBody.rotation * Mathf.Deg2Rad;
            PlayerBody.velocity = new Vector2(Mathf.Cos(bodyRotationAsRadian), Mathf.Sin(bodyRotationAsRadian)) * currentSpeed;
        }
    }
}