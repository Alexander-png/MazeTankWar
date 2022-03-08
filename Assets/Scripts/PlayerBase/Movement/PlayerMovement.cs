using MazeWar.Base;
using UnityEngine;

namespace MazeWar.PlayerBase
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField]
        private Observer Observer;
        [SerializeField]
        private Rigidbody2D PlayerBody;

        [SerializeField]
        private float RotationSpeed;
        [SerializeField]
        private float ForwardSpeed;
        [SerializeField]
        private float BackwardSpeed;

        private void FixedUpdate()
        {
            MovementLogic();
        }

        private void MovementLogic()
        {
            // Getting raw vertical axit
            float verticalAxisVal = Input.GetAxisRaw("Vertical");
            float horizontalAxisVal = Input.GetAxisRaw("Horizontal");

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
            PlayerBody.velocity = new Vector2(Mathf.Cos(bodyRotationAsRadian) * currentSpeed, Mathf.Sin(bodyRotationAsRadian) * currentSpeed);
        }
    }
}