using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CellularAutomaton
{
    [Serializable]
    public class PositionRule
    {


      //  private readonly Bitmap _visualRepresentationBitmap;

       // public BitmapSource visualBitmapSource { get; set; }

        public byte[,] Neighbourhood { get; set; }

        public byte OutputState { get; set; }

        public byte InputState { get; set; }


        public int EmptyCount { get; set; }

        public int DeadCount { get; set; }

        public int AliveCount { get; set; }

        public PositionRule(byte [,]_neighbourhood, byte _inputState, byte _outputState)
        {
         
            this.Neighbourhood = _neighbourhood;
            this.InputState = _inputState;
            this.OutputState = _outputState;
         //   _visualRepresentationBitmap = new Bitmap(5, 5);
            CountStates();
          //  visualBitmapSource = loadBitmap(_visualRepresentationBitmap);


        }


        //[DllImport("gdi32")]
        //static extern int DeleteObject(IntPtr o);

        //public static BitmapSource loadBitmap(Bitmap source)
        //{
        //    var ip = source.GetHbitmap();
        //    BitmapSource bs = null;
        //    try
        //    {
        //        bs = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
        //            ip,
        //            IntPtr.Zero, 
        //            Int32Rect.Empty,
        //            BitmapSizeOptions.FromEmptyOptions()
        //            );
        //    }
        //    finally
        //    {
        //        DeleteObject(ip);
        //    }

        //    return bs;

        //}


        [field: NonSerialized]
        private void  CountStates()
        {
            
            for(int i = 0; i < Neighbourhood.GetLength(0); i++)
            {
                for (int j = 0; j < Neighbourhood.GetLength(1); j++)
                {
                    var oldColor = GlobalSettings.StateColors[Neighbourhood[i, j]].Color;
                    var drawingcolor = System.Drawing.Color.FromArgb(oldColor.A, oldColor.R, oldColor.G, oldColor.B);
                   

                    if (i == 2 && j == 2)
                    {
                    //  oldColor = GlobalSettings.StateColors[InputState].Color;
                    //  drawingcolor = System.Drawing.Color.FromArgb(oldColor.A, oldColor.R, oldColor.G, oldColor.B);
                    //  _visualRepresentationBitmap.SetPixel(i, j, drawingcolor);
                        continue;
                    }

                   // _visualRepresentationBitmap.SetPixel(i, j, drawingcolor);



                    switch (Neighbourhood[i,j])
                    {
                        case (int)GlobalSettings.States.Empty:
                            EmptyCount++;
                            break;
                        case (int)GlobalSettings.States.Dead:
                            DeadCount++;
                            break;
                        case (int)GlobalSettings.States.Alive:
                            AliveCount++;
                            break;
                       
                       
                    }
                }
            }

            
        }

    }
}
