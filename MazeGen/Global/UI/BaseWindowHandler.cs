using MazeGen.Global;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace MazeGen.UI
{
    public abstract class BaseWindowHandler
    {
        public MainWindow MainWindow;
        public ProgramObserver Observer;
        protected BaseWindowHandler(MainWindow w)
        {
            MainWindow = w;
            Observer = ProgramObserver.GetInstance();
        }

        public abstract void BeginInteraction();
    }
}
