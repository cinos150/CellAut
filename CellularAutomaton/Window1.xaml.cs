using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CellularAutomaton
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        private const int yDim = 5;
        private const int xDim = 5;
        private SolidColorBrush empty;
        private SolidColorBrush dead ;
        private SolidColorBrush alive;

        public Window1(SolidColorBrush empty, SolidColorBrush dead, SolidColorBrush alive)
        {
            this.empty = empty;
            this.dead = dead;
            this.alive = alive;

            String[] dropStrings = new string[] {"Dead", "Alive", "Empty"};
           

            InitializeComponent();



            comboBox.ItemsSource = dropStrings;
            comboBox_Copy.ItemsSource = dropStrings;
            for (int y = 0; y < yDim; y++)
            {
                for (int x = 0; x < xDim; x++)
                {
                   Button butt =  new Button
                    {
                      Content = "Empty",
                        HorizontalAlignment = HorizontalAlignment.Stretch,
                        VerticalAlignment = VerticalAlignment.Stretch,
                       Background = empty,
                       Tag = new Point(x,y),
                   
                    };

                    butt.AddHandler(ButtonBase.ClickEvent, new RoutedEventHandler(MatrixButtonClick));

                    if (y == 2 && x == 2)
                    {
                        butt.Background = dead;
                        butt.Content = "Input State";
                        
                    }


                    Grid.SetRow(butt, x);
                    Grid.SetColumn(butt, y);
                    Grid.Children.Add(butt);
                }
            }

        }


     

        private void MatrixButtonClick(object sender, EventArgs e)
        {
            if (sender is Button)
            {
                Button b = sender as Button;

                b.Background = (b.Background == empty)
                    ? dead
                    : (b.Background == dead) ? alive : empty;

                b.Content = (b.Background == empty)
                    ? "Empty"
                    : (b.Background == dead) ?"Dead" : "Alive";
            }
        }


    }
}
