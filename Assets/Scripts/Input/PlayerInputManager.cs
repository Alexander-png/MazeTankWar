using MazeWar.PlayerBase.Assistance;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MazeWar.Input
{
    public class PlayerInputManager : MonoBehaviour
    {
        [SerializeField]
        private WeaponMountLinker _weaponMountLinker;

        public float VerticalAxis { get; private set; }
        public float HorizontalAxis { get; private set; }
        public Vector2 MousePositionOnScreen { get; private set; }
        public bool MouseMoved { get; private set; }

        private void OnEnable()
        {
            VerticalAxis = 0;
            HorizontalAxis = 0;
            MouseMoved = false;
        }

        #region Input logic
        private void OnMove(InputValue input)
        {
            VerticalAxis = input.Get<float>();
        }

        private void OnTurn(InputValue input)
        {
            HorizontalAxis = input.Get<float>();
        }

        private void OnShoot(InputValue input)
        {
            _weaponMountLinker.WeaponMount.ShootButtonPressed = input.isPressed;
        }

        private void OnMousePosition(InputValue input)
        {
            Vector2 newMousePos = input.Get<Vector2>();
            if (MousePositionOnScreen != newMousePos)
            {
                MousePositionOnScreen = newMousePos;
                MouseMoved = true;
            }
        }
        #endregion
    }
}
