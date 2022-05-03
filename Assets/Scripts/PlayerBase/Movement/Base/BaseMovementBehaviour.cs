using UnityEngine;

namespace MazeWar.PlayerBase.Movement.Base
{
    //https://ru.stackoverflow.com/questions/936026/%D0%9F%D1%80%D0%B0%D0%B2%D0%B8%D0%BB%D1%8C%D0%BD%D0%B0%D1%8F-%D1%80%D0%B5%D0%B0%D0%BB%D0%B8%D0%B7%D0%B0%D1%86%D0%B8%D1%8F-%D0%BF%D0%B5%D1%80%D0%B5%D0%B4%D0%B2%D0%B8%D0%B6%D0%B5%D0%BD%D0%B8%D1%8F-%D0%BF%D0%B5%D1%80%D1%81%D0%BE%D0%BD%D0%B0%D0%B6%D0%B0
    [RequireComponent(typeof(Input.PlayerInputManager))]
    public abstract class BaseMovementBehaviour : MonoBehaviour
    {
        [SerializeField]
        protected Rigidbody2D PlayerBody;
        [SerializeField]
        protected Input.PlayerInputManager _inputManager;

        [SerializeField]
        protected float RotationSpeed;
        [SerializeField]
        protected float ForwardSpeed;
        [SerializeField]
        protected float BackwardSpeed;
    }
}
