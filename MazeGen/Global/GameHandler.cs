using MazeGen.Global;
using MazeGen.MazeComponents.Elements;
using MazeGen.MazeComponents.Generating;
using System;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MazeGen.Global
{
    public class GameHandler
    { 
        GameLoop GameLoop = GameLoop.GetInstance();
        MainWindow w;

        public GameHandler()
        {
            GameLoop = GameLoop.GetInstance();
            w = App.Current.MainWindow as MainWindow;
        }

        private void Update(double elapsed)
        {
            
        }

        public async void StartGame(bool firstwave = false)
        {
            Tuple<Grid, MazeCell> res = MazeGenerator.GenerateMaze();
            w.AddMaze(res);
            await Task.Run(() => GameLoop.Start());
        }
    }
}
