using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CellularAutomaton
{
    [Serializable]
    public  class NumberRule
    {
       public byte Count { get; private   set; }

       public byte StateChoosen1 { get; private set; }

       public byte InputState { get; private set; }

       public byte OutputState { get; private set; }

       public NumberRule(byte count, byte stateChoosen, byte inputState, byte outputState)
        {
            Count = count;
            StateChoosen1 = stateChoosen;
            InputState = inputState;
            OutputState = outputState;
        }




    }
}
