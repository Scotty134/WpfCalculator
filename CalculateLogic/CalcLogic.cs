using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculateLogic
{
    public static class CalcLogic<T>
    {
        public static dynamic first { get; set; }
        public static dynamic second { get; set; }
        
        public static double Calculate (ActionSign action)
        {
            switch (action)
            {
                case ActionSign.Add : first += second;
                    break;
                case ActionSign.Sub: first -= second;
                    break;
                case ActionSign.Mul: first *= second;
                    break;
                case ActionSign.Div: first /= second;
                    break;
                case ActionSign.Mod: first = Math.Abs(first);
                    break;
                case ActionSign.Exp: first = Math.Pow(first, second);
                    break;
                case ActionSign.Sin: first = Math.Sin(first);
                    break;
                case ActionSign.Cos: first = Math.Cos(first);
                    break;
                case ActionSign.Tan: first = Math.Tan(first);
                    break;
            }

            return first;
        }
    }

    public enum ActionSign : int { Add, Sub, Mul, Div, Mod, Exp, Sin, Cos, Tan, None};
}
