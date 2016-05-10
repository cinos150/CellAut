using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CellularAutomaton
{
    public class PositionRule
    {
      
    
        enum States {Empty,Alive,Dead};
       private int[,] neighbourhood;
     
       private int outputState;
       private int inputState;
        private int emptyCount;
        private int deadCount;
        private int aliveCount;

        public int[,] Neighbourhood
        {
            get
            {
                return neighbourhood;
            }

            set
            {
                neighbourhood = value;
            }
        }

        public int OutputState
        {
            get
            {
                return outputState;
            }

            set
            {
                outputState = value;
            }
        }

        public int InputState
        {
            get
            {
                return inputState;
            }

            set
            {
                inputState = value;
            }
        }


        public int EmptyCount
        {
            get
            {
                return emptyCount;
            }

            set
            {
                emptyCount = value;
            }
        }

        public int DeadCount
        {
            get
            {
                return deadCount;
            }

            set
            {
                deadCount = value;
            }
        }

        public int AliveCount
        {
            get
            {
                return aliveCount;
            }

            set
            {
                aliveCount = value;
            }
        }

        public PositionRule(int [,]_neighbourhood, int _inputState, int _outputState)
        {
         
            this.neighbourhood = _neighbourhood;
            this.inputState = _inputState;
            this.outputState = _outputState;
            countStates();
        }



        

        private void  countStates()
        {
            
            for(int i = 0; i < neighbourhood.GetLength(0); i++)
            {
                for (int j = 0; j < neighbourhood.GetLength(1); j++)
                {

                    if (i == 2 && j == 2)
                        continue;
                        

                    switch(neighbourhood[i,j])
                    {
                        case (int)States.Empty:
                            emptyCount++;
                            break;
                        case (int)States.Dead:
                            deadCount++;
                            break;
                        case (int)States.Alive:
                            aliveCount++;
                            break;
                        default:
                            break;
                       
                    }
                }
            }

            
        }

    }
}
