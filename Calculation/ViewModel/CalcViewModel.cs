using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

using CalculateLogic;
using Calculation.Command;
using System.Windows.Controls;
using System.Globalization;
using System.Threading;
namespace Calculation.ViewModel
{
    public class CalcViewModel : BaseNotify
    {
        #region Constructor

        public CalcViewModel()
        {
            FirstOp = "0";
            SecOp = "0";
            Sign = String.Empty;
            DataField = MakeResult(FirstOp);
            CultureInfo nfi = new CultureInfo("en-US", false);
            nfi.NumberFormat.NumberDecimalSeparator = ".";
            Thread.CurrentThread.CurrentCulture = nfi;
        }

        #endregion

        #region Fields

        private DelegateCommand<object> _clickCommand;
        private string _first = String.Empty;
        private string _second = String.Empty;
        private string _sign = String.Empty;
        private string _data = String.Empty;
        private bool isResetSign = false;
        private short _state = 0;

        #endregion Fields

        #region Properties

        private string FirstOp
        {
            get { return _first; }
            set
            {
                _first = value;
            }
        }
        private string SecOp
        {
            get { return _second; }
            set
            {
                _second = value;
            }
        }
        private string Sign
        {
            get { return _sign; }
            set
            {
                _sign = value;
            }
        }

        public string DataField
        {
            get { return _data; }
            set
            {
                _data = value;
                OnPropertyChanged("DataField");
            }
        }

        #endregion Properties

        #region ICommand

        public ICommand CountCommand
        {
            get
            {
                _clickCommand = new DelegateCommand<object>(CountEnter);
                return _clickCommand;
            }
        }

        public ICommand SignCommand
        {
            get
            {
                _clickCommand = new DelegateCommand<object>(SignClick);
                return _clickCommand;
            }
        }

        public ICommand ClearCommand
        {
            get
            {
                _clickCommand = new DelegateCommand<object>(ClearClick);
                return _clickCommand;
            }
        }

        public ICommand ChangeCommand
        {
            get
            {
                _clickCommand = new DelegateCommand<object>(ChangeSign);
                return _clickCommand;
            }
        }

        #endregion ICommand

        #region Methods

        private void CountEnter(object obj)
        {
            try
            {
                var item = obj as Button;
                string value = item.Content.ToString();
                switch (_state)
                {
                    case 0:
                        short fval = EnterValue(value, _first);
                        if (fval == 1)
                        {
                            FirstOp = value;
                        }
                        if (fval == 2)
                        {
                            FirstOp += value;
                        }
                        if (fval == 3)
                        {
                            FirstOp += value;
                        }
                        DataField = MakeResult(FirstOp);
                        break;
                    case 1:
                        short sval = EnterValue(value, _second);
                        if (sval == 1)
                        {
                            SecOp = value;
                        }
                        if (sval == 2)
                        {
                            SecOp += value;
                        }
                        if (sval == 3)
                        {
                            SecOp += value;
                        }
                        isResetSign = false;
                        DataField = MakeResult(FirstOp, Sign, SecOp);
                        break;
                }
            }
            catch
            {
                DataField = "Error";
                _state = 99;
            }
        }

        private void SignClick(object obj)
        {
            var item = obj as Button;
            if (_state == 0 && item.Content.ToString() == "=")
                return;

            if (_state == 0 && item.Content.ToString() != "=")
            {
                Sign = item.Content.ToString();
                DataField = MakeResult(FirstOp, Sign);
                _state = 1;
                return;
            }
            if (_state == 1 && !isResetSign)
            {
                try
                {
                    CalcLogic<double>.first = Convert.ToDouble(FirstOp);
                    CalcLogic<double>.second = Convert.ToDouble(SecOp);
                    switch (Sign)
                    {
                        case "+":
                            CalcLogic<double>.Calculate(ActionSign.Add);
                            break;
                        case "-":
                            CalcLogic<double>.Calculate(ActionSign.Sub);
                            break;
                        case "*":
                            CalcLogic<double>.Calculate(ActionSign.Mul);
                            break;
                        case "/":
                            CalcLogic<double>.Calculate(ActionSign.Div);
                            break;
                    }
                    FirstOp = CalcLogic<double>.first.ToString();
                    SecOp = "0";
                    if (item.Content.ToString() != "=")
                        Sign = item.Content.ToString();
                    isResetSign = true;
                    DataField = MakeResult(FirstOp, Sign);
                    return;
                }
                catch
                {
                    DataField = "Error";
                    _state = 99;
                }
            }
            if (_state == 1 && isResetSign)
            {
                Sign = item.Content.ToString();
                DataField = MakeResult(FirstOp, Sign);
                return;
            }

        }

        private void ClearClick(object obj)
        {
            DataField = String.Empty;
            FirstOp = "0";
            SecOp = "0";
            Sign = String.Empty;
            _state = 0;
            DataField = MakeResult(FirstOp);
        }

        private void ChangeSign(object obj)
        {
            switch (_state)
            {
                case 0:
                    char val = IsNeedSwipe(_first);
                    if (val != '0')
                    {
                        StringBuilder sb = new StringBuilder(_first);
                        sb[0] = val;
                        FirstOp = sb.ToString();
                        DataField = MakeResult(FirstOp);
                    }
                    else
                    {
                        FirstOp = _first.Insert(0, "-");
                        DataField = MakeResult(FirstOp);
                    }
                    break;
                case 1:
                    char val1 = IsNeedSwipe(_second);
                    if (val1 != '0')
                    {
                        StringBuilder sb = new StringBuilder(_second);
                        sb[0] = val1;
                        SecOp = sb.ToString();
                        DataField = MakeResult(FirstOp, Sign, SecOp);
                    }
                    else
                    {
                        SecOp = _second.Insert(0, "-");
                        DataField = MakeResult(FirstOp, Sign, SecOp);
                    }
                    break;
            }
        }

        private char IsNeedSwipe(string elem)
        {
            char c = elem[0];
            switch (c)
            {
                case '+':
                    return '-';
                case '-':
                    return '+';
                default: return '0';
            }
        }

        private short EnterValue(string value, string elem)
        {
            if (elem == "0" && value == "0")
            {
                return 0;
            }
            if (elem == "0" && value != "0" && value != ".")
            {
                return 1;
            }
            if (elem != "0" && value != ".")
            {
                return 2;
            }
            if (value == "." && elem.IndexOf(".") == -1)
            {
                return 3;
            }
            return 99;
        }

        private string MakeResult(string FirstOp)
        {
            return MakeResult(FirstOp, null, null);
        }

        private string MakeResult(string FirstOp, string Sign)
        {
            return MakeResult(FirstOp, Sign, null);
        }

        private string MakeResult(string FirstOp, string Sign, string SecondOp)
        {
            string ret = String.Empty;
            if (FirstOp != null)
                ret = FirstOp;
            if (Sign != null)
                ret += Sign;
            if (SecondOp != null)
                ret += SecondOp;
            return ret;
        }

        #endregion Methods
    }
}
