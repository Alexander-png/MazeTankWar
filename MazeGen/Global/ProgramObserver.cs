using MazeGen.UI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;
using System.Windows.Input;

namespace MazeGen.Global
{
    public enum GameState { MainMenu = 0, PreGame = 1, InGame = 2 }

    public class ProgramObserver
    {
        // Singletone pattern implementaion
        private static ProgramObserver _instance = null;
        public static ProgramObserver GetInstance()
        {
            if (_instance == null)
                _instance = new ProgramObserver();
            return _instance;
        }

        public MainWindow Window;
        private delegate void MainWindowEvents();
        private event MainWindowEvents MainWindowReady;
        
        public delegate void WindowKeyDownEvents(KeyEventArgs e);
        public event WindowKeyDownEvents KeyDown;
        public event WindowKeyDownEvents KeyUp;

        public BaseWindowHandler CurrentHandler;

        private GameState _currentGameState;
        public GameState CurrentGameState
        {
            get => _currentGameState;
            set
            {
                _currentGameState = value;
                Window.Dispatcher.Invoke(() =>
                {
                    //Window.ClearGameWindow();
                    //CurrentHandler = null;
                    //switch (_currentGameState)
                    //{
                    //    case GameState.MainMenu:
                    //        CurrentHandler = new MainMenuWindowHandler(Window);
                    //        break;
                    //    case GameState.PreGame:
                    //        CurrentHandler = new PreGameWindowHandler(Window);
                    //        break;
                    //    case GameState.InGame:
                    //        CurrentHandler = new InGameWindowHandler(Window);
                    //        break;
                    //    case GameState.BestScoreTable:
                    //        CurrentHandler = new BestScoreTableWindowHandler(Window);
                    //        break;
                    //    case GameState.AddRecordToBestScoreTable:
                    //        CurrentHandler = new AddRecordToBestScoreTable(Window);
                    //        break;
                    //    default:
                    //        throw new ArgumentException("Invalid game state.");
                    //}
                    //CurrentHandler.BeginInteraction();
                });
            }
        }

        private ProgramObserver()
        { 
            MainWindowReady += ProgramObserver_MainWindowReady;
        }

        public void CallMainWindowReady()
        {
            MainWindowReady?.Invoke();
        }

        public void CallKeyDown(KeyEventArgs e)
        {
            KeyDown?.Invoke(e);
        }

        public void CallKeyUp(KeyEventArgs e)
        {            
            KeyUp?.Invoke(e);
        }

        private void ProgramObserver_MainWindowReady()
        {
            Window = (MainWindow)App.Current.MainWindow;
            CurrentGameState = GameState.MainMenu;
        }
    }
}
