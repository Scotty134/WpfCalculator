using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator.Helpers
{
    public static class OperatorLogic
    {
        public static string GetOperator(Operators op)
        {
            var resultOperator = String.Empty;

            switch(op)
            {
                case Operators.Add :
                    resultOperator = "+";
                    break;
                case Operators.Close :
                    resultOperator = ")";
                    break;
                case Operators.Degree :
                    resultOperator = "^";
                    break;
                case Operators.Div :
                    resultOperator = "/";
                    break;
                case Operators.Mul :
                    resultOperator = "*";
                    break;
                case Operators.Open :
                    resultOperator = "(";
                    break;
                case Operators.Sub : 
                    resultOperator = "-";
                    break;
            }

            return resultOperator;
        }
    }

    public enum Operators : int
    {
        None, Add, Sub, Mul, Div, Open, Close, Degree
    }
}


