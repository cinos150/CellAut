using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace CellularAutomaton
{
    class LogicGrid
    {
        private byte[,] _logicGridCopy;

       
        private readonly int _width;
        private readonly int _height;



        private  List<PositionRule> _filteredPositionRuleAlive;
        private  List<PositionRule> _filteredPositionRuleDead;
        private  List<PositionRule> _filteredPositionRuleEmpty;


        private List<NumberRule> _filteredNumberRuleAlive;
        private List<NumberRule> _filteredNumberRuleDead;
        private List<NumberRule> _filteredNumberRuleEmpty;
      


        public LogicGrid(int width , int height)
        {

            _width = width;
            _height = height;
       
            LogicGrid1 = _logicGridCopy = new byte[width, height];

            Array.Clear(LogicGrid1, 0, LogicGrid1.Length);
            Array.Clear(_logicGridCopy, 0, _logicGridCopy.Length);
        }

    
        public void InitRules()
        {
            _filteredPositionRuleAlive =
                           RuleSet.PositionRules.Where(p => p.InputState.Equals((int)GlobalSettings.States.Alive)).ToList();

            _filteredPositionRuleDead =
                               RuleSet.PositionRules.Where(p => p.InputState.Equals((int)GlobalSettings.States.Dead)).ToList();


            _filteredPositionRuleEmpty =
                               RuleSet.PositionRules.Where(p => p.InputState.Equals((int)GlobalSettings.States.Empty)).ToList();


            _filteredNumberRuleAlive =
                         RuleSet.NumberRule.Where(p => p.InputState.Equals((int)GlobalSettings.States.Alive)).ToList();

            _filteredNumberRuleDead =
                               RuleSet.NumberRule.Where(p => p.InputState.Equals((int)GlobalSettings.States.Dead)).ToList();


            _filteredNumberRuleEmpty =
                               RuleSet.NumberRule.Where(p => p.InputState.Equals((int)GlobalSettings.States.Empty)).ToList();



        }

        public byte[,] LogicGrid1 { get; set; }


        public  byte[,]  ComputeGeneration(Canvas mainCanvas,Dictionary<Point, Rectangle> visualRectangle, Rect viewport)
        {
         

            _logicGridCopy = (byte[,])LogicGrid1.Clone();

            for (int yGrid = 0; yGrid < LogicGrid1.GetLength(1); yGrid++)
            {
                for (int xGrid = 0; xGrid < LogicGrid1.GetLength(0); xGrid++)
                {
                    List<PositionRule> currRulePositionBased;
                    List<NumberRule> currRuleNumberBased;
                    switch (LogicGrid1[xGrid, yGrid])
                    {
                        case (int) GlobalSettings.States.Empty:
                            currRulePositionBased = _filteredPositionRuleEmpty;
                            currRuleNumberBased = _filteredNumberRuleEmpty;
                            break;
                        case (int) GlobalSettings.States.Dead:
                            currRulePositionBased = _filteredPositionRuleDead;
                            currRuleNumberBased = _filteredNumberRuleDead;
                            break;
                        case (int) GlobalSettings.States.Alive:
                            currRulePositionBased = _filteredPositionRuleAlive;
                            currRuleNumberBased = _filteredNumberRuleAlive;
                            break;

                        default:
                            currRulePositionBased = null;
                            currRuleNumberBased = null;
                            break;
                    }
                

                    if (currRulePositionBased != null && currRuleNumberBased != null)
                        using (var rulePositionBased = currRulePositionBased.GetEnumerator())
                        using (var ruleNumberBased = currRuleNumberBased.GetEnumerator())
                        {
                            while(rulePositionBased.MoveNext() || ruleNumberBased.MoveNext())
                            {

                                var currRuleNumber = ruleNumberBased.Current;
                                var currRulePosition = rulePositionBased.Current;

                                int emptyCount = 0;
                                int deadCount = 0;
                                int aliveCount = 0;

                                #region numberBased


                                if (currRuleNumber != null)
                                {
                                    if (ComputeNumberBased(out aliveCount, out deadCount, out emptyCount,
                                        xGrid, yGrid, currRuleNumber.StateChoosen1, currRuleNumber.Count,
                                        currRuleNumber.OutputState, currRuleNumber.InputState,
                                        mainCanvas, viewport, visualRectangle))
                                        break;


                                }


                                #endregion


                                if (currRuleNumber != null && currRulePosition!= null)
                                {
                                    if (currRulePosition.EmptyCount == emptyCount &&
                                        currRulePosition.AliveCount == aliveCount &&
                                        currRulePosition.DeadCount == deadCount)
                                        break;
                                }


                                #region positionBased




                                if (currRulePosition != null)
                                {
                                    if (computePositionBased(xGrid, yGrid, currRulePosition.Neighbourhood,
                                        currRulePosition.OutputState, currRulePosition.InputState, mainCanvas, viewport,
                                        visualRectangle))
                                        break;
                                }

                                #endregion






                            }
                        }
                }
            }

            return _logicGridCopy;
        }



        private bool computePositionBased(int xGrid, int yGrid,byte[,] Neighbourhood, byte OutputState, byte InputState, Canvas mainCanvas, Rect viewport, Dictionary<Point, Rectangle> visualRectangle)
        {

            var isthesame = true;

            for (int i = -2; i <= 2; i++)
            {
                for (int j = -2; j <= 2; j++)
                {
                    if (!IsInRangewidth(xGrid + i) || !IsInRangeheight(yGrid + j))
                    {
                        if ((int)GlobalSettings.States.Empty != Neighbourhood[i + 2, j + 2])
                        {
                            isthesame = false;
                            break;
                        }
                        continue;
                    }


                    if (LogicGrid1[xGrid + i, yGrid + j] !=
                        Neighbourhood[i + 2, j + 2])
                    {
                        isthesame = false;
                        break;
                    }
                }
                if (!isthesame)
                    break;
            }
            if (isthesame)
            {
                _logicGridCopy[xGrid, yGrid] = OutputState;
                DealWithAppliedRuledPositionBased(xGrid, yGrid, InputState, OutputState, viewport, mainCanvas,
                    visualRectangle);


                return true;
            }
            return false;
        }

        private bool ComputeNumberBased(out int  aliveCount, out int  deadCount, out int  emptyCount, int xGrid, int yGrid, int StateChoosen1, int Count, byte OutputState, byte InputState, Canvas mainCanvas, Rect viewport, Dictionary<Point,Rectangle> visualRectangle)
        {

            emptyCount = 0;
            deadCount =0;
            aliveCount = 0;

            for (int i = -2; i <= 2; i++)
            {
                for (int j = -2; j <= 2; j++)
                {

                    if(i ==0 && j == 0)
                        continue;
                    

                    if (!IsInRangewidth(xGrid + i) || !IsInRangeheight(yGrid + j))
                    {
                        emptyCount++;
                        continue;
                    }

                    switch (LogicGrid1[xGrid + i, yGrid + j])
                    {
                        case (byte)GlobalSettings.States.Empty:
                            emptyCount++;
                            break;

                        case (byte)GlobalSettings.States.Dead:
                            deadCount++;
                            break;
                        case (byte)GlobalSettings.States.Alive:
                            aliveCount++;
                            break;
                    }



                }
            }

            bool isMatch = false;

            switch (StateChoosen1)
            {
                case (byte)GlobalSettings.States.Empty:
                    if (Count == emptyCount)
                        isMatch = true;
                    break;
                case (byte)GlobalSettings.States.Alive:
                    if (Count == aliveCount)
                        isMatch = true;
                    break;
                case (byte)GlobalSettings.States.Dead:
                    if (Count == deadCount)
                        isMatch = true;
                    break;
            }


            if (isMatch)
            {
                _logicGridCopy[xGrid, yGrid] = OutputState;
                DealWithAppliedRuledPositionBased(xGrid, yGrid, InputState, OutputState, viewport, mainCanvas,
                    visualRectangle);

                return true;
            }

            return false;
        }


        private void DealWithAppliedRuledPositionBased(int xGrid, int yGrid, int InputState,int OutputState, Rect viewport, Canvas mainCanvas, Dictionary<Point,Rectangle> visualRectangle)
        {
           


            if (InputState == (int)GlobalSettings.States.Empty)
            {

                if (OutputState == (int)GlobalSettings.States.Empty)
                    return;


                var item = new Rectangle
                {
                    Width = viewport.Width,
                    Height = viewport.Height
                };

                switch (OutputState)
                {
                    case (int)GlobalSettings.States.Alive:
                        item.Fill = GlobalSettings.StateColors[(int)GlobalSettings.States.Alive];
                        break;
                    case (int)GlobalSettings.States.Dead:
                        item.Fill = GlobalSettings.StateColors[(int)GlobalSettings.States.Dead];
                        break;
                }
                mainCanvas.Children.Add(item);
                Canvas.SetLeft(item, xGrid * item.Width);
                Canvas.SetTop(item, yGrid * item.Height);
                visualRectangle.Add(new Point(xGrid, yGrid), item);

                return;
            }



            Rectangle choosenOne = visualRectangle[new Point(xGrid, yGrid)];

            switch (OutputState)
            {
                case (int)GlobalSettings.States.Alive:
                    choosenOne.Fill = GlobalSettings.StateColors[(int)GlobalSettings.States.Alive];

                    break;
                case (int)GlobalSettings.States.Dead:
                    choosenOne.Fill = GlobalSettings.StateColors[(int)GlobalSettings.States.Dead];

                    break;

                case (int)GlobalSettings.States.Empty:
                    mainCanvas.Children.Remove(choosenOne);
                    visualRectangle.Remove(new Point(xGrid, yGrid));
                    break;
            }
        }

        private bool IsInRangewidth(int x)
        {
            return x >= 0 && x<_width ;
        }

        private bool IsInRangeheight(int x)
        {
            return x >= 0 && x < _height;
        }



    }
}
