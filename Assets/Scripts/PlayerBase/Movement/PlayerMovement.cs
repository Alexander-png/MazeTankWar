using UnityEngine;
using UnityEngine.InputSystem;

namespace MazeWar.PlayerBase.Movement
{
    public class PlayerMovement : MonoBehaviour
    {
        //https://ru.stackoverflow.com/questions/936026/%D0%9F%D1%80%D0%B0%D0%B2%D0%B8%D0%BB%D1%8C%D0%BD%D0%B0%D1%8F-%D1%80%D0%B5%D0%B0%D0%BB%D0%B8%D0%B7%D0%B0%D1%86%D0%B8%D1%8F-%D0%BF%D0%B5%D1%80%D0%B5%D0%B4%D0%B2%D0%B8%D0%B6%D0%B5%D0%BD%D0%B8%D1%8F-%D0%BF%D0%B5%D1%80%D1%81%D0%BE%D0%BD%D0%B0%D0%B6%D0%B0

        [SerializeField]
        private Rigidbody2D PlayerBody;

        [SerializeField]
        private float RotationSpeed;
        [SerializeField]
        private float ForwardSpeed;
        [SerializeField]
        private float BackwardSpeed;

        private float VerticalAxis = 0;
        private float HorizontalAxis = 0;

        private void OnEnable()
        {
            VerticalAxis = 0;
            HorizontalAxis = 0;
        }

        private void FixedUpdate()
        {
            MovementLogic();
        }

        private void MovementLogic()
        {
            float verticalAxisVal = VerticalAxis;
            float horizontalAxisVal = HorizontalAxis;

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

        #region Input logic
        // Using Axis vals because listening input action in Dynamic update, but
        // moving physical body in FixedUpdate.
        public void OnMove(InputValue input)
        {
            VerticalAxis = input.Get<float>();
        }

        public void OnTurn(InputValue input)
        {
            HorizontalAxis = input.Get<float>();
        }
        #endregion
    }
}