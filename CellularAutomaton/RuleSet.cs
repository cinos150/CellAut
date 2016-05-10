using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CellularAutomaton 
{

 

    public class RuleSet 
    {
        public static List<PositionRule> positionRules = new List<PositionRule>();
        public static List<NumberRule> numberRule = new  List<NumberRule>();


        enum States { Empty, Alive, Dead };

        public RuleSet()
        {

        }

        public static bool isContrary(NumberRule rule1, PositionRule rule2)
        {
            if(rule1.OutputState == rule2.OutputState)
                return false;

            switch (rule1.StateChoosen1)
            {
                case (int)States.Empty:
                    if (rule1.Count == rule2.EmptyCount && rule2.DeadCount == 0 && rule2.AliveCount == 0)
                        return true;
                    break;

                case (int)States.Dead:
                    if (rule1.Count == rule2.DeadCount && rule2.AliveCount ==0)
                        return true;
                    break;

                case (int)States.Alive:
                    if (rule1.Count == rule2.AliveCount && rule2.DeadCount == 0)
                        return true;
                    break;
                default:
                    break;
                   
            }




            return false;



        }

        public static bool  isContrary(NumberRule rule1, NumberRule rule2)
        {
            if (rule1.OutputState == rule2.OutputState)
                return false;

            if (rule1.Count == rule2.Count)
                return true;

            return false;
        }


        public static bool  isContrary(PositionRule rule1, PositionRule rule2)
        {
            if (rule1.OutputState == rule2.OutputState)
                return false;

           

            return equals(rule1, rule2);
        }



        private static bool equals(PositionRule rule1, PositionRule rule2)
        {

            for (int i = 0 ; i < rule1.Neighbourhood.GetLength(0); i++)
            {
                for (int j = 0; j < rule1.Neighbourhood.GetLength(1); j++)
                {
                    //if (i == 2 && j == 2 && rule1.Neighbourhood[i, j] != rule2.Neighbourhood[i, j])
                    //    return false;

                    if (rule1.Neighbourhood[i, j] != rule2.Neighbourhood[i, j])
                        return false;
                }      
            }

            return true;
        }

    }
}
