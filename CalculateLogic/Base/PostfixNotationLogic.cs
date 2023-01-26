using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace CalculateLogic.Base
{
    public static class PostfixNotationLogic
    {
        static private byte GetPriority(string s)
        {
            switch (s)
            {
                case "(": return 0;
                case ")": return 1;
                case "+": return 2;
                case "-": return 3;
                case "*": return 4;
                case "/": return 4;
                case "^": return 5;
                default: return 6;
            }
        }
        
        static public double Calculate(string input)
        {
            CultureInfo nfi = new CultureInfo("en-US", false);
            nfi.NumberFormat.NumberDecimalSeparator = ".";
            Thread.CurrentThread.CurrentCulture = nfi;

            var output = GetExpression(input);
            double result = Counting(output);
            return result;
        }

        static private List<string> GetExpression(string input)      
        {
            string[] sb = ToArrayRegular(input);
            //string[] sb = ToArraySpace(input);
            List<string> sblist = new List<string>(sb);

            for (var x = 0; x < sb.Length; x++)
            {
                if (sb[x] == "" || sb[x] == String.Empty || sb[x] == " ")
                    sblist.Remove(sb[x]);
            }

            for (var x = 0; x < sblist.Count; x++)
            {
                if (IsNumber(sblist[x]) && x > 0)
                {
                    int y = x - 1;
                    if (!IsOperator(sblist[y]))
                    {
                        sblist[x] = sblist[x].Remove(0, 1);
                        sblist.Insert(x, "-");
                    }
                }
            }

            List<string> output = new List<string>();
            Stack<string> operStack = new Stack<string>();

            for (int i = 0; i < sblist.Count; i++)
            {
                if (IsNumber(sblist[i]))
                {
                    output.Add(sblist[i]);
                    continue;
                }

                if (IsOperator(sblist[i]))
                {
                    if (sblist[i] == "(")
                        operStack.Push(sblist[i]);
                    else if (sblist[i] == ")")
                    {
                        string s = operStack.Pop();

                        while (s != "(")
                        {
                            output.Add(s);
                            s = operStack.Pop();
                        }
                    }
                    else
                    {
                        if (operStack.Count > 0)
                            if (GetPriority(sblist[i]) <= GetPriority(operStack.Peek()))
                                output.Add(operStack.Pop());

                        operStack.Push(sblist[i]);

                    }
                }
            }

            while (operStack.Count > 0)
                output.Add(operStack.Pop());

            return output;
        }

        static private double Counting(List<string> sblist)
        {
            double result = 0;
            Stack<double> temp = new Stack<double>();

            for (int i = 0; i < sblist.Count; i++)
            {
                if (IsNumber(sblist[i]))
                {
                    temp.Push(double.Parse(sblist[i]));
                    continue;
                }
                else if (IsOperator(sblist[i]))
                {
                    double a = temp.Pop();
                    double b = temp.Pop();

                    switch (sblist[i])
                    {
                        case "+": result = b + a; break;
                        case "-": result = b - a; break;
                        case "*": result = b * a; break;
                        case "/": result = b / a; break;
                        case "^": result = double.Parse(Math.Pow(double.Parse(b.ToString()), double.Parse(a.ToString())).ToString()); break;
                    }
                    temp.Push(result);
                }
            }
            return temp.Peek();
        }

        static private bool IsOperator(string input)
        {
            return (Regex.IsMatch(input, @"[*|+|\-|/|(|)|^]$")) ? true: false;
        }

        static private bool IsNumber(string input)
        {
            return (Regex.IsMatch(input, @"[-]?\d+([.,]{0,1}\d+)?")) ? true : false;
        }

        static private string[] ToArrayRegular(string input)
        {
            return Regex.Split(input.Trim(' '), @"([-]?\d+[.,]{0,1}\d+)|([-]?\d+)|([*|+|-|/|(|)|^])");
        }

        static private string[] ToArraySpace(string input)
        {
            return input.Split(' ');
        }

    }
}
