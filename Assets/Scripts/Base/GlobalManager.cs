//https://www.google.com/search?q=Unity+best+Singleton&sa=X&ved=2ahUKEwieqdyn1MP2AhURyYsKHTXcCrkQ1QJ6BAgwEAE&biw=2560&bih=1329&dpr=1
using System;
using UnityEngine;

namespace MazeWar.Base
{
    public class GlobalManager : MonoBehaviour
    {
        public static EventHandler<EventArgs> OnGameplayManagerAppeared;

        private static GameplayManager _GameplayManager = null;
        public static GameplayManager GameplayManager
        { 
            get => _GameplayManager;
            set
            {
                if (_GameplayManager != null)
                    throw new InvalidOperationException("Gameplay manager can be set only once.");
                _GameplayManager = value;
                OnGameplayManagerAppeared?.Invoke(null, EventArgs.Empty);
            }
        }
        public static GlobalManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
                Destroy(gameObject);
            else
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
#if DEBUG
                Debug.Log("Global manager was created");
#endif
            }   
        }
    }
}


