using MazeGen.Global;
using MazeGen.MazeComponents.Elements;
using MazeGen.MazeComponents.Generating;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MazeGen
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        GameHandler GameHandler;
        public MainWindow()
        {
            InitializeComponent();
            KeyDown += MainWindow_KeyDown;
            KeyUp += MainWindow_KeyUp;
            Closing += MainWindow_Closing;   
            Loaded += MainWindow_Loaded;
            App.Current.MainWindow = this;
            GameHandler = new GameHandler();
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            GameHandler.StartGame();
            Player.X = 75;
            Player.Y = 75;
        }

        public void AddMaze(Tuple<Grid, MazeCell> maze)
        {
            ClearGameWindow();
            Dispatcher.Invoke(() =>
            {
                MazeArea.Children.Add(maze.Item1);
                Player.MazeHead = maze.Item2;
            });
        }

        public void ShowParams(double x, double y, int cellX, int cellY)
        {
            XVal.Content = $"X={x}";
            YVal.Content = $"Y={y}";
            CellXVal.Content = $"CellX={cellX}";
            CellYVal.Content = $"CellY={cellY}";
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            GameLoop.GetInstance().Clear();
        }

        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            Player.KeyDownN(e);
        }

        private void MainWindow_KeyUp(object sender, KeyEventArgs e)
        {
            Player.KeyUpN(e);
        }

        public void ClearGameWindow()
        {
            Dispatcher.Invoke(() =>
            {
                MazeArea.Children.Clear();
            });
        }
    }
}
