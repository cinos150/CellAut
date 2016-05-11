using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace CellularAutomaton
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class AddRuleWindow : Window
    {
        private const int yDim = 5;
        private const int xDim = 5;
    

        private readonly Button[,] _buttonList = new Button[xDim,yDim];
        public AddRuleWindow()
        {
            

           
           

            InitializeComponent();




            outputstatecontrol.SelectedValue = inputstateControl.SelectedValue = CountStateControl.SelectedValue = "Empty";
            outputstatecontrol.ItemsSource = inputstateControl.ItemsSource = CountStateControl.ItemsSource =  GlobalSettings.StatesName;
            OutputStateButton.Background = GlobalSettings.StateColors[(int) GlobalSettings.States.Dead];
            OutputStateButton.Tag = (byte) GlobalSettings.States.Dead;

            for (int y = 0; y < yDim; y++)
            {
                for (int x = 0; x < xDim; x++)
                {
                   Button butt =  new Button
                    {
                      Content = "Empty",
                        HorizontalAlignment = HorizontalAlignment.Stretch,
                        VerticalAlignment = VerticalAlignment.Stretch,
                       Background = GlobalSettings.StateColors[(int)GlobalSettings.States.Empty],
                       Tag = (byte)GlobalSettings.States.Empty
                   
                    };

                    butt.AddHandler(ButtonBase.ClickEvent, new RoutedEventHandler(MatrixButtonClick));
                    _buttonList[y, x] = butt;
                   


                    Grid.SetRow(butt, x);
                    Grid.SetColumn(butt, y);
                    PositionBasedButtonGrid.Children.Add(butt);
                }
            }



           


        }


     

        private void MatrixButtonClick(object sender, EventArgs e)
        {
            var b = sender as Button;
            if (b != null)
            {



                b.Background = Equals(b.Background, GlobalSettings.StateColors[(int)GlobalSettings.States.Empty])
                    ? GlobalSettings.StateColors[(int)GlobalSettings.States.Dead]
                    : (Equals(b.Background, GlobalSettings.StateColors[(int)GlobalSettings.States.Dead])) ? GlobalSettings.StateColors[(int)GlobalSettings.States.Alive] : GlobalSettings.StateColors[(int)GlobalSettings.States.Empty];

                b.Content = Equals(b.Background, GlobalSettings.StateColors[(int)GlobalSettings.States.Empty])
                    ? "Empty"
                    : (Equals(b.Background, GlobalSettings.StateColors[(int)GlobalSettings.States.Dead])) ?"Dead" : "Alive";

                b.Tag = (Equals(b.Background, GlobalSettings.StateColors[(int)GlobalSettings.States.Empty]))
                   ? (byte)GlobalSettings.States.Empty
                   : (Equals(b.Background, GlobalSettings.StateColors[(int)GlobalSettings.States.Dead])) ? (byte)GlobalSettings.States.Dead : (byte)GlobalSettings.States.Alive;



            }
        }

        private void positionBasedButton(object sender, RoutedEventArgs e)
        {

        

            byte inputState = 0 ;
            var neigh = new byte[xDim, yDim];
            for (int i = 0; i < xDim; i++)
            {
                for (int j = 0; j < yDim; j++)
                {

                    if(i == xDim/2 && j == yDim/2)
                        inputState = (byte)_buttonList[i, j].Tag;

                    neigh[i, j] = (byte)_buttonList[i, j].Tag;
               
                }
            }


            var pr = new PositionRule(neigh,inputState,(byte)OutputStateButton.Tag);

            var iscontrary = false;

            foreach (PositionRule sec_rule in RuleSet.PositionRules)
            {
                if (iscontrary = RuleSet.isContrary(sec_rule, pr))
                    break;
            }

            foreach (NumberRule secRule in RuleSet.NumberRule.Where(sec_rule => iscontrary = RuleSet.isContrary(sec_rule, pr)))
            {
                break;
            }


            if (!iscontrary)
            {
                RuleSet.PositionRules.Add(pr);
                Console.WriteLine("No Contrary");
                MessageBox.Show("Rule Added", "Confirmation", MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            else
            {
                Console.WriteLine("Is Contrary");
                MessageBox.Show("This is contrary", "Confirmation", MessageBoxButton.OK,
                   MessageBoxImage.Information);
            }
        }

        private void NumberBasedButton(object sender, RoutedEventArgs e)
        {
            byte numInSurr =0;
            if (CountControl.Value != null)
                numInSurr = (byte) CountControl.Value;


          var nRule = new   NumberRule(numInSurr, (byte) CountStateControl.SelectedIndex, (byte) inputstateControl.SelectedIndex, (byte) outputstatecontrol.SelectedIndex);

            bool iscontrary = false;

            foreach (PositionRule secRule in RuleSet.PositionRules)
            {
                if (iscontrary = RuleSet.isContrary(nRule,secRule ))
                    break;
            }

            foreach (NumberRule secRule in RuleSet.NumberRule)
            {
                if (iscontrary = RuleSet.isContrary(secRule, nRule))
                    break;
            }


           
            if (!iscontrary)
            {
                RuleSet.NumberRule.Add(nRule);
                Console.WriteLine("No Contrary");
                MessageBox.Show("Rule Added", "Confirmation", MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            else
            {
                Console.WriteLine("Is Contrary");
                MessageBox.Show("This is contrary", "Confirmation", MessageBoxButton.OK,
                   MessageBoxImage.Information);
            }


        }

        private void loadItem_Click(object sender, RoutedEventArgs e)
        {
            loadItem.Items.Clear();

            for (int i = 0; i < RuleSet.PositionRules.Count; i++)
            {
                Label lb = new Label();
                lb.Content = "Pos Rule nr." + i;
                lb.Tag = i;
                lb.MouseDown += Lb_MouseDown;
                loadItem.Items.Add(lb);
            }


            for (int i = 0; i < RuleSet.NumberRule.Count; i++)
            {
                Label lb = new Label();
                lb.Content = "Num Rule nr." + i +1;
                lb.Tag = i;
                lb.MouseDown += Lb_MouseDown;
                loadItem.Items.Add(lb);
            }


        }

        private void Lb_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var lb = (Label) sender;
            var x = lb.Content as string;

            if (x.Substring(0, x.IndexOf(" ", StringComparison.Ordinal)) == "Pos")
            {
               var chosenRule = RuleSet.PositionRules[(int) lb.Tag];
                int i = 0;
                for(int k = 0 ; k < _buttonList.GetLength(0); k++)
                {
                    for (int l = 0; l < _buttonList.GetLength(1); l++)
                    {
                    _buttonList[k,l].Background = GlobalSettings.StateColors[chosenRule.Neighbourhood[k, l]];
                    _buttonList[k, l].Content = GlobalSettings.StatesName[chosenRule.Neighbourhood[k, l]];
                    }

                }

                OutputStateButton.Content = GlobalSettings.StatesName[chosenRule.OutputState];
                OutputStateButton.Tag = chosenRule.OutputState;
                OutputStateButton.Background = GlobalSettings.StateColors[chosenRule.OutputState];



            }
            else
            {
               var chosenRule = RuleSet.NumberRule[(int) lb.Tag];

                CountControl.Value = (int)chosenRule.Count;
                CountStateControl.SelectedIndex = chosenRule.StateChoosen1;
                inputstateControl.SelectedIndex = chosenRule.InputState;
                outputstatecontrol.SelectedIndex = chosenRule.OutputState;

            }


        }
    }
}
