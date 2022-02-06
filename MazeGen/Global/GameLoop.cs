using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;

namespace MazeGen.Global
{
    public class GameLoop
    {
        private static GameLoop _instance = null;
        public static GameLoop GetInstance()
        {
            if (_instance == null)
                _instance = new GameLoop();
            return _instance;
        }

        public bool IsRunning { get; set; }
        Timer FixedUpdateTimer;

        private GameLoop()
        {
            FixedUpdateTimer = new Timer();
            FixedUpdateTimer.AutoReset = true;
            FixedUpdateTimer.Interval = 2;
            FixedUpdateTimer.Elapsed += FixedUpdate;
        }

        DateTime LastFixedTick;
        private void FixedUpdate(object sender, ElapsedEventArgs e)
        {
            FixedTick?.Invoke((DateTime.Now - LastFixedTick).TotalMilliseconds);
            LastFixedTick = DateTime.Now;
        }

        public delegate void GameLoopTick(double elapsed);
        public event GameLoopTick Tick;
        public event GameLoopTick FixedTick;

        public void Start()
        {
            if (IsRunning)
                return;
            FixedUpdateTimer.Start();
            LastFixedTick = DateTime.Now;

            IsRunning = true;
            DateTime lastTime = DateTime.Now;
            while (IsRunning)
            {
                DateTime current = DateTime.Now;
                Tick?.Invoke((current - lastTime).TotalMilliseconds);
                lastTime = current;
            }
        }

        public void Clear()
        {
            Tick = null;
            IsRunning = false;
            FixedUpdateTimer.Stop();
        }
    }
}
