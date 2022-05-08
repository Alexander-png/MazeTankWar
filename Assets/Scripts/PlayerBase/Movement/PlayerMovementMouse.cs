using MazeWar.PlayerBase.Movement.Base;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MazeWar.PlayerBase.Movement
{
    public class PlayerMovementMouse : BaseMovementBehaviour
    {
        [SerializeField]
        private float _cursorStopDistance;

        public void FixedUpdate()
        {
            MovementLogic();
        }

        private void MovementLogic()
        {
            if (!InputManager.MouseMoved)
                return;

            PlayerBody.angularVelocity = 0;
            
            Vector3 mousePos = GetMousePosition();
            float targetAngle = Mathf.Atan2(mousePos.y - PlayerBody.position.y, mousePos.x - PlayerBody.position.x) * Mathf.Rad2Deg;
            // Todo: solve problem with rotation.
            PlayerBody.rotation = targetAngle;

            if (Vector2.Distance(PlayerBody.position, mousePos) > _cursorStopDistance)
            {
                float bodyRotationAsRadian = PlayerBody.rotation * Mathf.Deg2Rad;
                PlayerBody.velocity = new Vector2(Mathf.Cos(bodyRotationAsRadian), Mathf.Sin(bodyRotationAsRadian)) * ForwardSpeed * Time.fixedDeltaTime;
            }
            else
            {
                PlayerBody.velocity = Vector2.zero;
            }
        }

        private Vector3 GetMousePosition()
        {
            // tooked form here: https://forum.unity.com/threads/mouse-to-world-position-using-perspective-camera-when-there-is-nothing-to-hit.1199350/
            Plane plane = new Plane(Vector3.back, Vector3.zero);
            Ray ray = Camera.main.ScreenPointToRay(InputManager.MousePositionOnScreen);
            if (plane.Raycast(ray, out float enter))
            {
                return ray.GetPoint(enter);
            }
            return Vector3.zero;
        }

        //private float DetermineHorizontalAxis()
        //{
        //    Vector3 mousePos = GetMousePosition();
        //    float targetAngle = Mathf.Atan2(mousePos.y - PlayerBody.position.y, mousePos.x - PlayerBody.position.x) * Mathf.Rad2Deg;
        //    if (PlayerBody.rotation > targetAngle)
        //        return 1;
        //    else if (PlayerBody.rotation < targetAngle)
        //        return -1;
        //    return 0;
        //}

        //float horizontalAxisVal = DetermineHorizontalAxis();
        //PlayerBody.angularVelocity = -RotationSpeed * horizontalAxisVal * Time.fixedDeltaTime;
        //private float DetermineHorizontalAxis()
        //{
        //    // tooked form here: https://forum.unity.com/threads/mouse-to-world-position-using-perspective-camera-when-there-is-nothing-to-hit.1199350/
        //    Plane plane = new Plane(Vector3.back, Vector3.zero);
        //    Ray ray = Camera.main.ScreenPointToRay(InputManager.MousePositionOnScreen);
        //    if (plane.Raycast(ray, out float enter))
        //    {
        //        Vector3 mousePos = ray.GetPoint(enter);
        //        // Now you have the world position you wanted.
        //        //Debug.Log(cursorPosition);

        //        float targetAngle = Mathf.Atan2(mousePos.y - PlayerBody.position.y, mousePos.x - PlayerBody.position.x) * Mathf.Rad2Deg;

        //        //PlayerBody.rotation = targetAngle;
        //        //Debug.Log($"Target angle: {targetAngle}");
        //        //Debug.Log($"Current rotation: {PlayerBody.rotation}");

        //        if (PlayerBody.rotation > targetAngle)
        //            return 1;
        //        else if (PlayerBody.rotation < targetAngle)
        //            return -1;
        //    }
        //    return 0;
        //}
    }
}
