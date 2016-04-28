using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Xceed.Wpf.Toolkit;

namespace CellularAutomaton
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Point p;

        private SolidColorBrush empty = Brushes.White;
        private SolidColorBrush dead = Brushes.Yellow;
        private SolidColorBrush alive = Brushes.Green;


      


        public MainWindow()
        {
            InitializeComponent();

            VisibleGrid.Background = empty;
            st.ScaleX = 13;
            st.ScaleY = 13;
            MainCanvas.Width= SystemParameters.PrimaryScreenWidth;


            MainCanvas.Height = SystemParameters.PrimaryScreenHeight;
          


        }

       
        const double ScaleRate = 1.1;
        private void Canvas_MouseWheel(object sender, MouseWheelEventArgs e)
        {
          

            if (e.Delta > 0)
            {
                st.ScaleX *= ScaleRate;
                st.ScaleY *= ScaleRate;



            }
            else if(e.Delta < 0 &&  st.ScaleX > 1)
            {



                st.ScaleX /= ScaleRate;
                st.ScaleY /= ScaleRate;
            }
        }


        private Point startPoint;
        private Point originalPoint;
        private Boolean flag = false; 
        private void MainWindow1_MouseDown(object sender, MouseButtonEventArgs e)
        {

            try
            {
                Point cellPos = e.GetPosition(MainCanvas);
             

                Rectangle ClickedRectangle =
                    VisualTreeHelper.HitTest(MainCanvas, cellPos).VisualHit as Rectangle;


                if (ClickedRectangle.Fill == alive)
                {
                    ClickedRectangle.Fill =dead;
                  
                    return;

                }
                else if (ClickedRectangle.Fill == dead)
                {
                    MainCanvas.Children.Remove(ClickedRectangle);
                    return;
                }

            }
            catch (NullReferenceException)
            {
                //ignored for now
            }

            var rekt = VisualBrush.Viewport;

         if (e.ChangedButton == MouseButton.Middle && e.ButtonState == MouseButtonState.Pressed)
         {
           
             startPoint = e.GetPosition(MainCanvas);
                originalPoint = new Point(TranslateTransform.X, TranslateTransform.Y);
             if (flag == false)
             {
                    MainCanvas.CaptureMouse();
                    flag = true;
             }
             else
             {
                    MainCanvas.ReleaseMouseCapture();
                    flag = false;

             }
         }


           else if (e.ChangedButton == MouseButton.Middle && e.ButtonState == MouseButtonState.Released)
            {
                MainCanvas.ReleaseMouseCapture();
                flag = false;
            }
            else
            {
                var newCell = new Rectangle
                {
                    Width = rekt.Width,
                    Height = rekt.Height,
                    Fill = alive

                };


                var x = e.GetPosition(MainCanvas);
                int tempX = (int) (x.X/rekt.Width);
                int tempY = (int) (x.Y/rekt.Height);


                Canvas.SetLeft(newCell, tempX*newCell.Width);
                Canvas.SetTop(newCell, tempY*newCell.Height);

                MainCanvas.Children.Add(newCell);
            }
        }


        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            Window1 win2 = new Window1(empty, dead, alive);
            win2.Show();
           
        }

        private void DeadButton_OnClick(object sender, RoutedEventArgs e)
        {

            Button b = (Button)sender;
            ColorDialog colorDialog = new ColorDialog();
            colorDialog.Owner = this;
            if ((bool)colorDialog.ShowDialog())
            {
                b.Background =  dead = new SolidColorBrush(colorDialog.SelectedColor);
                
            }
        }

        private void AliveButton_OnClick(object sender, RoutedEventArgs e)
        {
            Button b = (Button)sender;
            ColorDialog colorDialog = new ColorDialog {Owner = this};
            if ((bool)colorDialog.ShowDialog())
            {
                b.Background = alive = new SolidColorBrush(colorDialog.SelectedColor);

            }
        }


        private void EmptyButton_OnClick(object sender, RoutedEventArgs e)
        {
            Button b = (Button)sender;
            ColorDialog colorDialog = new ColorDialog();
            colorDialog.Owner = this;
            if ((bool)colorDialog.ShowDialog())
            {
                b.Background = VisibleGrid.Background =  empty = new SolidColorBrush(colorDialog.SelectedColor);

            }
        }

        private void ResetButton_OnClick(object sender, RoutedEventArgs e)
        {
            TranslateTransform.X = 0;
            TranslateTransform.Y = 0;
            MainCanvas.Children.Clear();

        }

        private void MainCanvas_OnMouseMove(object sender, MouseEventArgs e)
        {
            if (!MainCanvas.IsMouseCaptured) return;

            if (!flag) return;
            var moveVector = startPoint - e.GetPosition(MainCanvas);
                
            TranslateTransform.X = originalPoint.X - moveVector.X;
            TranslateTransform.Y = originalPoint.Y - moveVector.Y;
        }

        private void Start_OnClick(object sender, RoutedEventArgs e)
        {
            Button b = (Button)sender;
            b.Content = (String.Equals((string) b.Content, "Start", StringComparison.OrdinalIgnoreCase)) ? "Stop" : "Start";
        }

        private void Pause_OnClick(object sender, RoutedEventArgs e)
        {
            Button b = (Button)sender;
            b.Content = (string.Equals((string)b.Content, "Pause", StringComparison.OrdinalIgnoreCase)) ? "Resume" : "Pause";
        }
    }


      

    }

   


