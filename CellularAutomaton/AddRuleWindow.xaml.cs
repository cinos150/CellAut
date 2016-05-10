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
    public partial class AddRuleWindow : Window
    {
        private const int yDim = 5;
        private const int xDim = 5;
        private SolidColorBrush empty;
        private SolidColorBrush dead ;
        private SolidColorBrush alive;
        private enum States { Empty, Alive, Dead};

        private Button[,] buttonList = new Button[xDim,yDim];
        public AddRuleWindow(SolidColorBrush empty, SolidColorBrush dead, SolidColorBrush alive)
        {
            this.empty = empty;
            this.dead = dead;
            this.alive = alive;

            String[] dropStrings = new string[] {"Empty", "Alive", "Dead"};
           

            InitializeComponent();



            outputstatecontrol.ItemsSource = inputstateControl.ItemsSource = countStateControl.ItemsSource =  dropStrings;
            outputStateButton.Tag = -1;


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
                       Tag = (int)States.Empty
                   
                    };

                    butt.AddHandler(ButtonBase.ClickEvent, new RoutedEventHandler(MatrixButtonClick));
                    buttonList[y, x] = butt;
                    if (y == 2 && x == 2)
                    {
                        butt.Background = empty;
                        butt.Content = "Input State";
                        
                    }


                    Grid.SetRow(butt, x);
                    Grid.SetColumn(butt, y);
                    positionBasedButtonGrid.Children.Add(butt);
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

                b.Tag = (b.Background == empty)
                   ? (int)States.Empty
                   : (b.Background == dead) ? (int)States.Dead : (int)States.Alive;



            }
        }

        private void positionBasedButton(object sender, RoutedEventArgs e)
        {

            if ((String)buttonList[xDim / 2, yDim / 2].Content == "Input State" || (int)outputStateButton.Tag == -1)
                return;

            int inputState = 0 ;
            int[,] neigh = new int[xDim, yDim];
            for (int i = 0; i < xDim; i++)
            {
                for (int j = 0; j < yDim; j++)
                {

                    if(i == xDim/2 && j == yDim/2)
                        inputState = (int)buttonList[i, j].Tag;

                    neigh[i, j] = (int)buttonList[i, j].Tag;
               
                }
            }


            PositionRule PR = new PositionRule(neigh,inputState,(int)outputStateButton.Tag);

            bool iscontrary = false;

            foreach (PositionRule sec_rule in RuleSet.positionRules)
            {
                if (iscontrary = RuleSet.isContrary(sec_rule, PR))
                    break;
            }

            foreach (NumberRule sec_rule in RuleSet.numberRule)
            {
                if (iscontrary = RuleSet.isContrary(sec_rule, PR))
                    break;
            }


            if (!iscontrary)
            {
                RuleSet.positionRules.Add(PR);
                Console.WriteLine("No Contrary");
            }
            else
                Console.WriteLine("is Contrary");

        }

        private void numberBasedButton(object sender, RoutedEventArgs e)
        {

        }
    }
}
