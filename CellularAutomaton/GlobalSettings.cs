using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace CellularAutomaton
{
    class GlobalSettings
    {
        private static readonly SolidColorBrush EmptyColor=  Brushes.White;
        private static readonly SolidColorBrush DeadColor = Brushes.Blue;
        private static readonly SolidColorBrush AliveColor = Brushes.Green;

        public static SolidColorBrush[] StateColors = {EmptyColor, AliveColor, DeadColor };
       


        public static string[] StatesName = new[] { "Empty", "Alive", "Dead" };
        public enum States { Empty, Alive, Dead };


         

    }
}
