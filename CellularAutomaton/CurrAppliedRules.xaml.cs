using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media;
using ListView = System.Windows.Controls.ListView;
using ListViewItem = System.Windows.Forms.ListViewItem;

namespace CellularAutomaton
{
    /// <summary>
    /// Interaction logic for CurrAppliedRules.xaml
    /// </summary>
    public partial class CurrAppliedRules : Window
    {
        public CurrAppliedRules()
        {
            InitializeComponent();

            PreapreListViewNumberBased();
            PreapreListViewPositionBased();

        }



        public void PreapreListViewNumberBased()
        {
            NumberBasedListView.Items.Clear();


            foreach (NumberRule numberRule in RuleSet.NumberRule)
            {
                String full = "If " + numberRule.Count + " " + GlobalSettings.StatesName[numberRule.StateChoosen1] + " cells \n then " +
                              GlobalSettings.StatesName[numberRule.InputState] + " becomes " + GlobalSettings.StatesName[numberRule.OutputState] + ".";
               
                

                NumberBasedListView.Items.Add(full);
            }
        }


        private void numberBasedClick(object sender, RoutedEventArgs e)
        {
            var listView = sender as ListView;
            var item = (String) listView?.SelectedItem;
            if (item != null)
            {
                 NumberBasedListView.Items.Remove(item);
               

            }
        }
        
        private void positionBasedClick(object sender, RoutedEventArgs e)
        {
            var listView = sender as ListView;
            var item = (String)listView?.SelectedItem;
            if (item != null)
            {
                PositionBasedListView.Items.Remove(item);


            }
        }


        public void PreapreListViewPositionBased()
        {
            PositionBasedListView.Items.Clear();
           
            foreach (var positionRule in RuleSet.PositionRules)
            {
               var full =  GlobalSettings.StatesName[positionRule.InputState] + " becomes " + GlobalSettings.StatesName[positionRule.OutputState] + ".";

               

                var pseudoImage = full +"\n";

                for (int i = 0; i < positionRule.Neighbourhood.GetLength(0); i++)
                {
                    for (int j = 0; j < positionRule.Neighbourhood.GetLength(1); j++)
                    {
                        pseudoImage+= GlobalSettings.StatesName[positionRule.Neighbourhood[i, j]] + " ";
                    }
                    pseudoImage += "\n";
                }


              
                PositionBasedListView.Items.Add(pseudoImage);



            }
        }


        private void RefreshNumberBased_Click(object sender, RoutedEventArgs e)
        {
            PreapreListViewNumberBased();
        }

        private void RefreshPositionBased_Click(object sender, RoutedEventArgs e)
        {
            PreapreListViewPositionBased();
        }
    }
}
