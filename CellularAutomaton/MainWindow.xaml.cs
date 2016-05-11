using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using Microsoft.Win32;
using static System.String;

namespace CellularAutomaton
{
   
    public partial class MainWindow
    {
     

        private readonly LogicGrid _lGrid = null;
      
        private readonly Dictionary<Point,Rectangle> _visualRectangle;
        private readonly DispatcherTimer _dispatcherTimer;
        private bool _performSimulation = false;
        private Queue<byte[,]> oscilationQueue; 



        public MainWindow()
        {
            InitializeComponent();

            VisibleGrid.Background = GlobalSettings.StateColors[(int)GlobalSettings.States.Empty];
            st.ScaleX = 13;
            st.ScaleY = 13;

            oscilationQueue = new Queue<byte[,]>();

            var width = MainCanvas.Width = 1000;//SystemParameters.PrimaryScreenWidth;
            var height = MainCanvas.Height = 500;//SystemParameters.PrimaryScreenHeight;

            _visualRectangle = new Dictionary<Point,Rectangle>();

         

            _lGrid = new  LogicGrid(Convert.ToInt32(width), Convert.ToInt32(height));

             _dispatcherTimer = new DispatcherTimer();
            _dispatcherTimer.Tick += dispatcherTimer_Tick;
            _dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 500);

            DeadButton.Background = GlobalSettings.StateColors[(int)GlobalSettings.States.Dead]; 
            AliveButton.Background = GlobalSettings.StateColors[(int)GlobalSettings.States.Alive];


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


        private Point _startPoint;
        private Point _originalPoint;
        private Boolean flag;

        private void MainWindow1_MouseDown(object sender, MouseButtonEventArgs e)
        {

            var rekt = VisualBrush.Viewport;

            var cellPos = e.GetPosition(MainCanvas);
            int tempX = (int) (cellPos.X/rekt.Width);
            int tempY = (int) (cellPos.Y/rekt.Height);

            if (e.ChangedButton == MouseButton.Left && e.ButtonState == MouseButtonState.Pressed)
            {

                Rectangle ClickedRectangle = null;

                if (tempX >= Width && tempY >= Height)
                    return;

                if (_lGrid.LogicGrid1[tempX, tempY] != (int) GlobalSettings.States.Empty)
                {
                    try
                    {
                        ClickedRectangle =
                            VisualTreeHelper.HitTest(MainCanvas, cellPos).VisualHit as Rectangle;




                        if (Equals(ClickedRectangle.Fill, GlobalSettings.StateColors[(int)GlobalSettings.States.Alive]))
                        {
                            ClickedRectangle.Fill = GlobalSettings.StateColors[(int)GlobalSettings.States.Dead]; 
                            _lGrid.LogicGrid1[tempX, tempY] = (int) GlobalSettings.States.Dead;

                            return;

                        }
                        if (Equals(ClickedRectangle.Fill, GlobalSettings.StateColors[(int)GlobalSettings.States.Dead]))
                        {

                            MainCanvas.Children.Remove(ClickedRectangle);
                            _visualRectangle.Remove(new Point(tempX, tempY));
                            _lGrid.LogicGrid1[tempX, tempY] = (int) GlobalSettings.States.Empty;
                            return;
                        }


                    }
                    catch (NullReferenceException)
                    {


                    }


                }
                else
                {
                    var newCell = new Rectangle
                    {
                        Width = rekt.Width,
                        Height = rekt.Height,
                        Fill = GlobalSettings.StateColors[(int) GlobalSettings.States.Alive],
                        Tag = new Point(tempX, tempY)
                    };



                    Canvas.SetLeft(newCell, tempX*newCell.Width);
                    Canvas.SetTop(newCell, tempY*newCell.Height);


                    _lGrid.LogicGrid1[tempX, tempY] = (int) GlobalSettings.States.Alive;
                    _visualRectangle.Add(new Point(tempX, tempY), newCell);

                    MainCanvas.Children.Add(newCell);
                }


            }





            if (e.ChangedButton == MouseButton.Right && e.ButtonState == MouseButtonState.Pressed)
            {

                _startPoint = e.GetPosition(MainCanvas);
                _originalPoint = new Point(TranslateTransform.X, TranslateTransform.Y);
                if (!flag)
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


            else if (e.ChangedButton == MouseButton.Right && e.ButtonState == MouseButtonState.Released)
            {
                MainCanvas.ReleaseMouseCapture();
                flag = false;
            }

        }


        

        private void DeadButton_OnClick(object sender, RoutedEventArgs e)
        {

            Button b = (Button)sender;
            ColorDialog colorDialog = new ColorDialog {Owner = this};
            var showDialog = colorDialog.ShowDialog();
            if (showDialog != null && (bool)showDialog)
            {
                using (new WaitCursor())
                {
                    FillSpecificCellWithColorChange(b, GlobalSettings.StateColors[(int)GlobalSettings.States.Dead], new SolidColorBrush(colorDialog.SelectedColor));
                }
               
              
            }
        }

        private void AliveButton_OnClick(object sender, RoutedEventArgs e)
        {
            Button b = (Button)sender;
            ColorDialog colorDialog = new ColorDialog {Owner = this};
            var showDialog = colorDialog.ShowDialog();
            if (showDialog != null && (bool)showDialog)
            {
                using (new WaitCursor())
                {
                    FillSpecificCellWithColorChange(b, GlobalSettings.StateColors[(int)GlobalSettings.States.Alive], new SolidColorBrush(colorDialog.SelectedColor));
                }
            }
        }

        private void FillSpecificCellWithColorChange(Button b,SolidColorBrush oldColor ,SolidColorBrush newColor)
        {
            if (!Equals(b.Background, newColor))
            {



                foreach (Rectangle rect in MainCanvas.Children)
                {
                    if (rect.Fill.Equals(oldColor))
                        rect.Fill = newColor;
                }

                b.Background = GlobalSettings.StateColors[(int)GlobalSettings.States.Alive] = newColor;

            }

        }


        private void EmptyButton_OnClick(object sender, RoutedEventArgs e)
        {
            Button b = (Button)sender;
            ColorDialog colorDialog = new ColorDialog {Owner = this};
            var showDialog = colorDialog.ShowDialog();
            if (showDialog != null && (bool)showDialog)
            {
                b.Background = VisibleGrid.Background = GlobalSettings.StateColors[(int)GlobalSettings.States.Empty] = new SolidColorBrush(colorDialog.SelectedColor);

            }
        }

        private void ResetGridResetButtonOnClick(object sender, RoutedEventArgs e)
        {
            TranslateTransform.X = 0;
            TranslateTransform.Y = 0;
            Array.Clear(_lGrid.LogicGrid1, 0, _lGrid.LogicGrid1.Length);
            MainCanvas.Children.Clear();
          oscilationQueue.Clear();

           _visualRectangle.Clear();


        }

        private void MainCanvas_OnMouseMove(object sender, MouseEventArgs e)
        {
            if (!MainCanvas.IsMouseCaptured) return;

            if (!flag) return;
            var moveVector = _startPoint - e.GetPosition(MainCanvas);
                
            TranslateTransform.X = _originalPoint.X - moveVector.X;
            TranslateTransform.Y = _originalPoint.Y - moveVector.Y;
        }



        private void Start_OnClick(object sender, RoutedEventArgs e)
        {
            var b = (Button) sender;
            b.Content = string.Equals((string) b.Content, "Start", StringComparison.OrdinalIgnoreCase)
                ? "Pause"
                : "Start";

            _lGrid.InitRules();

            _performSimulation = !_performSimulation;


            if (_performSimulation)
            {
                AddRuleButton.IsEnabled = false;
                StopButton.IsEnabled = true;
                _dispatcherTimer.Start();
            }
            else
            {
                _dispatcherTimer.Stop();
            }







        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {

            if(_performSimulation == false)
                _dispatcherTimer.Stop();

            if (oscilationQueue.Count < 3)
            {
                oscilationQueue.Enqueue(_lGrid.LogicGrid1);
            }
            else
            {
                oscilationQueue.Dequeue();
            }

            Application.Current.Dispatcher.BeginInvoke(
                    DispatcherPriority.Background,
                    new Action(() =>
                    {



                        for (int i = 0; i < stepIteration.Value; i++)
                        {
                            _lGrid.LogicGrid1 = _lGrid.ComputeGeneration(MainCanvas,
                            _visualRectangle, VisualBrush.Viewport);


                            if (oscilationQueue.Any(oneItemBytes => AreTheSame(_lGrid.LogicGrid1, oneItemBytes)))
                            {
                                _dispatcherTimer.Stop();
                                MessageBox.Show("Oscilation detected", "Confirmation", MessageBoxButton.OK,
                                    MessageBoxImage.Information);
                                StartSimulationButton.Content = "Start";
                                _performSimulation = false;
                                StopButton.IsEnabled = false;
                                AddRuleButton.IsEnabled = true;
                                return;
                            }
                        }
                    }));





        }


        private bool AreTheSame(byte[,] arr1, byte[,] arr2)
        {
            for (int i = 0; i < arr1.GetLength(0); i++)
            {
                for (int j = 0; j < arr1.GetLength(1); j++)
                {
                    if (arr1[i, j] != arr2[i, j])
                        return false;
                }
            }
            return true;
        }


        private void StopResetOnClick(object sender, RoutedEventArgs e)
        {
            AddRuleButton.IsEnabled = true;
            oscilationQueue.Clear();
            if (_performSimulation)
            {
                _dispatcherTimer.Stop();

                MainCanvas.Children.Clear();

                _visualRectangle.Clear();

                TranslateTransform.X = 0;
                TranslateTransform.Y = 0;
                Array.Clear(_lGrid.LogicGrid1, 0, _lGrid.LogicGrid1.Length);
                StopButton.IsEnabled = false;
                AddRuleButton.IsEnabled = true;
                StartSimulationButton.Content = "Start";
            }


        }



        private void SaveGridButtonClick(object sender, RoutedEventArgs e)
        {

            string gridType = "(.PIgrid) | *.PIgrid";


            SaveGrid(gridType,"grid", _lGrid.LogicGrid1);

        }

        public static void Serialize(object t, string path)
        {
            using (Stream stream = File.Open(path, FileMode.Create))
            {
                BinaryFormatter bformatter = new BinaryFormatter();
                bformatter.Serialize(stream, t);
            }
        }
    

        public static object Deserialize(string path)
        {
            using (Stream stream = File.Open(path, FileMode.Open))
            {
                BinaryFormatter bformatter = new BinaryFormatter();
                return bformatter.Deserialize(stream);
            }
        }

        private void SaveGrid(String type,string name, object whatToserialize)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog
            {
                Filter = "Text Files " + type,
                Title = "Save a "+name +" to file"
            };
            saveFileDialog1.ShowDialog();


            if (saveFileDialog1.FileName != "")
            {
                Serialize(whatToserialize, saveFileDialog1.FileName);
                StartSimulationButton.Content = "Start";
                _performSimulation = false;
            }
        }


        private void LoadGridButtonClick(object sender, RoutedEventArgs e)
        {

            LoadGrid(".PIgrid", "Text Files (.PIgrid)|*.PIgrid");
        }

        private void ResetRulesButton_OnClick(object sender, RoutedEventArgs e)
        {
           RuleSet.NumberRule.Clear();
            RuleSet.PositionRules.Clear();
        }

        private void HistoryButtonClick(object sender, RoutedEventArgs e)
        {
            var b = (Button)sender;
            var win2 = new CurrAppliedRules();
            win2.Show();
            b.IsEnabled = false;

            win2.Closed += CurrActive_Closed;
        }

        private void CurrActive_Closed(object sender, EventArgs e)
        {
            CurrentlyActiveButton.IsEnabled = true;
        }
    


      
        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            var b = (Button) sender;
            StartSimulationButton.Content = "Start";
            _performSimulation = false;
            _dispatcherTimer.Stop();
            var win2 = new AddRuleWindow();
                win2.Show();
            b.IsEnabled = false;
            resetMenuItem.IsEnabled = false;

            win2.Closed += Win2_Closed;
            }

        private void Win2_Closed(object sender, EventArgs e)
        {
            AddRuleButton.IsEnabled = true;
            resetMenuItem.IsEnabled = true;
        }


        private void MainWindow_OnKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Left:
                   
                    TranslateTransform.X += 20;
                    break;
                case Key.Right:
                   
                        TranslateTransform.X -= 20;
                    break;
                case Key.Up:
                  
                        TranslateTransform.Y += 20;
                    break;
                case Key.Down:
                   
                        TranslateTransform.Y -= 20;
                    break;
            }
          

        }





        private void SaveRulesPositionButtonClick(object sender, RoutedEventArgs e)
        {

            if (RuleSet.PositionRules.Count == 0 || RuleSet.PositionRules == null)
                return;

            const string ruleType = "(.PIPrules) | *.PIPrules";



            SaveRules(ruleType, "position rules", RuleSet.PositionRules);



        }


        private void SaveRules<T>(string type, string name, List<T> objectToSerialize)
        {

            SaveFileDialog saveFileDialog1 = new SaveFileDialog
            {
                Filter = "Text Files " + type,
                Title = "Save a " + name + " to file"
            };
            saveFileDialog1.ShowDialog();


            if (saveFileDialog1.FileName != "")
            {
                WriteToBinaryFile(saveFileDialog1.FileName, objectToSerialize);
            }

        }


        public static void WriteToBinaryFile<T>(string filePath, T objectToWrite, bool append = false)
        {
            using (Stream stream = File.Open(filePath, append ? FileMode.Append : FileMode.Create))
            {
                var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                binaryFormatter.Serialize(stream, objectToWrite);
            }
        }

        public static T ReadFromBinaryFile<T>(string filePath)
        {
            using (Stream stream = File.Open(filePath, FileMode.Open))
            {
                var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                return (T)binaryFormatter.Deserialize(stream);
            }
        }


        private void LoadRules<T>(string def, string filter, List<T> objectToSerialize)
        {

            OpenFileDialog dlg = new OpenFileDialog
            {
                DefaultExt = def,
                Filter = filter
            };


            if (Directory.Exists(AppDomain.CurrentDomain.BaseDirectory))
            {
                dlg.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;
            }


            bool? result = dlg.ShowDialog();


            if (result == true)
            {

                using (Stream stream = File.Open(dlg.FileName, FileMode.Open))
                {
                    var bformatter = new BinaryFormatter();

                    objectToSerialize.AddRange((List<T>) bformatter.Deserialize(stream));
                }

            }

        }


        private void SaveRuleNumberButtonClick(object sender, RoutedEventArgs e)
        {

            if (RuleSet.NumberRule.Count == 0 || RuleSet.NumberRule == null)
                return;

            string ruleType = "(.PINrules) | *.PINrules";


            SaveRules(ruleType, "number rules", RuleSet.NumberRule);




        }


      



        private void LoadGrid(string def, string filter)
        {

            OpenFileDialog dlg = new OpenFileDialog
            {
                DefaultExt = def,
                Filter = filter
            };


            if (Directory.Exists(AppDomain.CurrentDomain.BaseDirectory))
            {
                dlg.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;
            }


            bool? result = dlg.ShowDialog();


            if (result == true)
            {

                _lGrid.LogicGrid1 = (byte[,])Deserialize(dlg.FileName);

            }



            _visualRectangle.Clear();
            MainCanvas.Children.Clear();

            using (new WaitCursor())
            {

                for (int i = 0; i < _lGrid.LogicGrid1.GetLength(0); i++)
                {
                    for (int j = 0; j < _lGrid.LogicGrid1.GetLength(1); j++)
                    {

                        if (_lGrid.LogicGrid1[i, j].Equals((byte)GlobalSettings.States.Empty))
                            continue;

                        int tempX = (int)(i * VisualBrush.Viewport.Width);
                        int tempY = (int)(j * VisualBrush.Viewport.Width);

                        var choosenRectangle = new Rectangle
                        {
                            Width = VisualBrush.Viewport.Width,
                            Height = VisualBrush.Viewport.Height
                        };

                        switch (_lGrid.LogicGrid1[i, j])
                        {
                            case (byte)GlobalSettings.States.Alive:
                                choosenRectangle.Fill = GlobalSettings.StateColors[(int)GlobalSettings.States.Alive];

                                break;
                            case (byte)GlobalSettings.States.Dead:
                                choosenRectangle.Fill = GlobalSettings.StateColors[(int)GlobalSettings.States.Dead];
                                break;
                        }

                        MainCanvas.Children.Add(choosenRectangle);
                        Canvas.SetLeft(choosenRectangle, tempX);
                        Canvas.SetTop(choosenRectangle, tempY);
                        _visualRectangle.Add(new Point(i, j), choosenRectangle);


                    }
                }
            }
        }

        private void LoadPositionRuleButtonClick(object sender, RoutedEventArgs e)
        {
            LoadRules(".PIPrules", "Text Files (.PIPrules)|*.PIPrules", RuleSet.PositionRules);
        }

        private void LoadNumberRuleButtonClick(object sender, RoutedEventArgs e)
        {
            LoadRules(".PINrules", "Text Files (.PINrules)|*.PINrules", RuleSet.NumberRule);
        }
    }

      



    }






   


