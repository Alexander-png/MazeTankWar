using UnityEngine;

namespace MazeWar.PlayerBase.Collision
{
    public class CollisionObserver : MonoBehaviour
    {
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.tag == "Shell")
            {
                //collision
            }
        }
    }
}
