using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CellularAutomaton
{
    /// <summary>
    /// Interaction logic for ColorDialog.xaml
    /// </summary>
    public partial class ColorDialog : Window
    {
       
        public ColorDialog()
        {
            InitializeComponent();
        }
       

       
        public Color SelectedColor
        {
            get
            {
                try
                {
                    return (Color) colorPicker.SelectedColor;
                }
                catch (Exception)
                {
                }

                return new Color();
            }
        }
      

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Close();
            }
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
       
    }
}
