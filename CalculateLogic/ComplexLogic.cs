using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculateLogic
{
    public struct ComplexLogic
    {
        //(A+jB) Комплексные числа
        public double a { get; private set; }
        public double b { get; private set; }


        public ComplexLogic(Double firstOp, Double secondOp)
            : this()
        {
            this.a = firstOp;
            this.b = secondOp;
        }

        public static explicit operator ComplexLogic(String InLine)
        {
            Double A, B;
            string sign = "+j";

            if (InLine.IndexOf("-j") != -1)
            {
                sign = "-j";
            }
            string[] BlocData = InLine.Replace("(", "").Replace(")", "").Replace(sign, " ").Split();

            if (!double.TryParse(BlocData[0], out A))
            {
                throw new System.FormatException(BlocData[0].ToString() + " not Double format region. Element A");
            }

            if (!double.TryParse(BlocData[1], out B))
            {
                throw new System.FormatException(BlocData[1].ToString() + " not Double format region. Element B");
            }

            if (sign == "-j")
            {
                B *= -1;
            }

            return new ComplexLogic(A, B);
        }

        public static ComplexLogic operator +(ComplexLogic firstOp, ComplexLogic secondOp)
        {
            //(a1+b1j)+(a2+b2j)=(a1+a2)+(b1+b2)j
            Double A, B;
            A = firstOp.a + secondOp.a;
            B = firstOp.b + secondOp.b;
            return new ComplexLogic(A, B);
        }

        public static ComplexLogic operator -(ComplexLogic firstOp, ComplexLogic secondOp)
        {
            //(a1+b1j)-(a2+b2j)=(a1-a2)+(b1-b2)j
            Double A, B;
            A = firstOp.a - secondOp.a;
            B = firstOp.b - secondOp.b;
            return new ComplexLogic(A, B);
        }

        public static ComplexLogic operator /(ComplexLogic firstOp, ComplexLogic secondOp)
        {
            //(a1+b1j)/(a2+b2j)=(a1*a2+b1*b2)/(a2^2+b2^2)+((b1*a2-a1*b2)/(a2^2+b2^2))j
            Double A1, A2, B1;
            A1 = firstOp.a * secondOp.a + firstOp.b * secondOp.b;
            A2 = Math.Pow(secondOp.a, 2) + Math.Pow(secondOp.b, 2);
            B1 = firstOp.b * secondOp.a + firstOp.a * secondOp.b;
            if (A2 == 0)
            {
                throw new System.DivideByZeroException("Complex DivideByZeroException");
            }
            else
            {
                A1 /= A2;
                B1 /= A2;
            }
            return new ComplexLogic(A1, B1);

        }

        public static ComplexLogic operator *(ComplexLogic firstOp, ComplexLogic secondOp)
        {
            //(a1+b1j)-(a2+b2j)=(a1*a2-b1*b2)+(b1*a2+a1*b2)j
            Double A, B;
            A = firstOp.a * secondOp.a + firstOp.b * secondOp.b;
            B = firstOp.b * secondOp.a + firstOp.a * secondOp.b;
            return new ComplexLogic(A, B);
        }

        public override string ToString()
        {
            return a.ToString() + (b < 0 ? "-" : "+") + "j" + Math.Abs(b).ToString();
        }
    }
}
