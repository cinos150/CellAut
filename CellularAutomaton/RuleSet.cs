using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CellularAutomaton 
{

 

    public class RuleSet 
    {
        public static List<PositionRule> PositionRules = new List<PositionRule>();
        public static List<NumberRule> NumberRule = new  List<NumberRule>();


    

        public static bool isContrary(NumberRule rule1, PositionRule rule2)
        {
            if(rule1.OutputState == rule2.OutputState)
                return false;

            switch (rule1.StateChoosen1)
            {
                case (int)GlobalSettings.States.Empty:
                    if (rule1.Count == rule2.EmptyCount && rule2.DeadCount == 0 && rule2.AliveCount == 0)
                        return true;
                    break;

                case (int)GlobalSettings.States.Dead:
                    if (rule1.Count == rule2.DeadCount && rule2.AliveCount ==0)
                        return true;
                    break;

                case (int)GlobalSettings.States.Alive:
                    if (rule1.Count == rule2.AliveCount && rule2.DeadCount == 0)
                        return true;
                    break;
             
                   
            }




            return false;



        }

        public static bool  isContrary(NumberRule rule1, NumberRule rule2)
        {
            if (rule1.OutputState == rule2.OutputState)
                return false;



            if (rule1.Count == rule2.Count && rule1.StateChoosen1 == rule2.StateChoosen1 && rule1.OutputState != rule2.OutputState)
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
                   
                    if (rule1.Neighbourhood[i, j] != rule2.Neighbourhood[i, j])
                        return false;
                }      
            }



            return true;
        }

    }
}
