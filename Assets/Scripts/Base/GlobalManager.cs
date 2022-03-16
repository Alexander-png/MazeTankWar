//https://www.google.com/search?q=Unity+best+Singleton&sa=X&ved=2ahUKEwieqdyn1MP2AhURyYsKHTXcCrkQ1QJ6BAgwEAE&biw=2560&bih=1329&dpr=1
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MazeWar.Base
{
    public class GlobalManager : MonoBehaviour
    {
        public static GameplayManager GameplayManager = null;
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


