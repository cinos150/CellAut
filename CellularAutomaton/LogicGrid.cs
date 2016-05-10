using System;
using System.Collections.Generic;
using System.Linq;
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
        private int[,] logicGrid;
        private int[,] logicGridCopy;
        enum States { Empty, Alive, Dead };
        private SolidColorBrush empty;
        private SolidColorBrush dead;
        private SolidColorBrush alive;



        public LogicGrid(int width , int height, SolidColorBrush empty, SolidColorBrush dead, SolidColorBrush alive)
        {

            this.empty = empty;
            this.dead = dead;
            this.alive = alive;
            logicGrid = logicGridCopy = new int[width, height];

            Array.Clear(logicGrid, 0, logicGrid.Length);
            Array.Clear(logicGridCopy, 0, logicGridCopy.Length);


        }

        public int[,] LogicGrid1
        {
            get
            {
                return logicGrid;
            }

            set
            {
                logicGrid = value;
            }
        }



     

        public  int[,]  computeGeneration(Canvas mainCanvas,Rectangle [,] visualRectangle, Rect viewport)
        {
            List<PositionRule>  filteredNumberRuleAlive =
                                RuleSet.positionRules.Where(p => p.InputState.Equals((int)States.Alive)).ToList();

            List<PositionRule> filteredNumberRuleDead = 
                                RuleSet.positionRules.Where(p => p.InputState.Equals((int)States.Dead)).ToList();


            List<PositionRule> filteredNumberRuleEmpty = 
                                RuleSet.positionRules.Where(p => p.InputState.Equals((int)States.Empty)).ToList();
            List<PositionRule> currRule;




            logicGridCopy = (int[,])logicGrid.Clone();
            bool isthesame = true;

            for (int xGrid = 0; xGrid < logicGrid.GetLength(0); xGrid++)
            {
                for (int yGrid = 0; yGrid < logicGrid.GetLength(1); yGrid++)
                {

                   
                    

                    switch(logicGrid[xGrid,yGrid])
                    {
                        case (int)States.Empty:
                            currRule = filteredNumberRuleEmpty;
                            break;
                        case (int)States.Dead:
                            currRule = filteredNumberRuleDead;

                            break;
                        case (int)States.Alive:
                            currRule = filteredNumberRuleAlive;
                            break;

                        default:
                            currRule = null;
                            break;
                    }


                    foreach (var currNumberBasedRule in currRule)
                    {
                        isthesame = true;
                        for (int i = -2; i < 2; i++)
                        {
                            for (int j = -2; j < 2; j++)
                            {


                                if (!isInRangewidth(xGrid + i) || !isInRangeheight(yGrid + j))
                                    continue;

                         
                                if (logicGrid[xGrid + i,yGrid + j] != currNumberBasedRule.Neighbourhood[i + 2, j + 2])
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
                            Point p = new Point(xGrid, yGrid);
                            
                            logicGridCopy[xGrid, yGrid] = currNumberBasedRule.OutputState;

                            if(currNumberBasedRule.InputState == (int)States.Empty)
                            {

                                Rectangle item = new Rectangle();
                                item.Width = viewport.Width;
                                item.Height = viewport.Height;

                                switch (currNumberBasedRule.OutputState)
                                {
                                    case (int)States.Alive:
                                        mainCanvas.Children.Add(item);
                                        Canvas.SetLeft(item, xGrid * item.Width);
                                        Canvas.SetTop(item, yGrid * item.Height);

                                        visualRectangle[xGrid, yGrid] = item;
                                        break;

                                    case (int)States.Dead:
                                        item.Fill = dead;
                                        mainCanvas.Children.Add(item);
                                        Canvas.SetLeft(item, xGrid * item.Width);
                                        Canvas.SetTop(item, yGrid * item.Height);

                                        visualRectangle[xGrid, yGrid] = item;
                                        break;

                                    case (int)States.Empty:
                                    
                                        break;
                                    default:
                                        break;


                                }
                                break;
                            }

                            Rectangle choosenOne = visualRectangle[xGrid, yGrid];
                                   // mainCanvas.Children.Remove(item);
                                    switch(currNumberBasedRule.OutputState)
                                    {
                                        case (int)States.Alive:
                                            choosenOne.Fill = alive;
                                        // mainCanvas.Children.Add(item);
                                         //   Canvas.SetLeft(item, xGrid * item.Width);
                                          //  Canvas.SetTop(item, yGrid * item.Height);
                                            break;
                                        case (int)States.Dead:
                                    choosenOne.Fill = dead;
                                      //      mainCanvas.Children.Add(item);
                                        //    Canvas.SetLeft(item, xGrid * item.Width);
                                          //  Canvas.SetTop(item, yGrid * item.Height);
                                            break;

                                        case (int)States.Empty:
                                        mainCanvas.Children.Remove(choosenOne);
                                        break;
                                        default:
                                            break;


                                    }
                               
                                   
                                  
                                    break;
                                



                            
                          
                        }


                    }
                   

                }
            }

            return logicGridCopy;
        }

        private bool isInRangewidth(int x)
        {
            return x > 0 && x<SystemParameters.PrimaryScreenWidth ;
        }

        private bool isInRangeheight(int x)
        {
            return x > 0 && x < SystemParameters.PrimaryScreenHeight;
        }



    }
}
