using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CellularAutomaton
{
   public  class NumberRule
    {

        private int count;
        private int StateChoosen;
        private int inputState;
        private int outputState;

        public int Count
        {
            get
            {
                return count;
            }

          private   set
            {
                count = value;
            }
        }

        public int StateChoosen1
        {
            get
            {
                return StateChoosen;
            }

            private set
            {
                StateChoosen = value;
            }
        }

        public int InputState
        {
            get
            {
                return inputState;
            }

            private set
            {
                inputState = value;
            }
        }

        public int OutputState
        {
            get
            {
                return outputState;
            }

            private set
            {
                outputState = value;
            }
        }

        public NumberRule(int count, int stateChoosen, int inputState, int outputState)
        {
            this.Count = count;
            this.StateChoosen = stateChoosen;
            this.InputState = inputState;
            this.OutputState = outputState;
        }
    }
}
