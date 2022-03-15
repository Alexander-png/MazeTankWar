using MazeWar.Base;
using UnityEngine;

namespace MazeWar.PlayerBase.Observer
{
    public class PlayerObjectObserver : MonoBehaviour
    {
        [SerializeField]
        private GameplayManager GameplayManager;

        private void OnDisable()
        {
            if (GameplayManager != null)
                GameplayManager.OnPlayerKilled();
        }
    }
}