using MazeWar.MazeComponents;
using MazeWar.MazeComponents.Base;
using MazeWar.PlayerBase.Observer;
using System;
using UnityEngine;

namespace MazeWar.Base.Abstractions
{
    public abstract class AbstractGameplayManager : MonoBehaviour
    {
        public abstract PlayerStateObserver[] Players { get; }
        public abstract MazeGenerator MazeGenerator { get; }
        public abstract MazeCellData MazeHead { get; }
        public abstract int PlayersAliveCount { get; }

        public bool InGame { get; protected set; }
        public event RoundRestartEvent OnRoundRestart;

        protected void InvokeRestartRound() => OnRoundRestart?.Invoke();

        public delegate void RoundRestartEvent();
    }
}

