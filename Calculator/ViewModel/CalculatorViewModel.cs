using Calculator.Common;
using Calculator.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using CalculateLogic.Base;
using System.Text.RegularExpressions;

namespace Calculator.ViewModel
{
    public class CalculatorViewModel : BaseNotify
    {
        #region Constructor

        public CalculatorViewModel()
        {
            CultureInfo nfi = new CultureInfo("en-US", false);
            nfi.NumberFormat.NumberDecimalSeparator = _separator;
            Thread.CurrentThread.CurrentCulture = nfi;
        }

        #endregion

        #region Fields

        private string _separator = ".";

        private DelegateCommand<object> _clickCommand;
        private string _bufferField = "0";
        private string _expressionField = String.Empty;
        private bool _memoryVisibility = false;
        private bool _errorVisibility = false;
        private bool _isDefaultValue = true;
        private bool _comaActive = false;
        private int _openBrackets = 0;
        private bool _isCloseBracket = false;
        private Operators _lastOp = Operators.None;
        private string oldBuffer = String.Empty;
        private bool _isResultClick = false;
        private bool _signClick = false;

        #endregion Fields

        #region Properties

        public string BufferField
        {
            get
            {
                return _bufferField;
            }
            set
            {
                _bufferField = value;
                OnPropertyChanged("BufferField");
            }
        }

        public string ExpressionField
        {
            get
            {
                return _expressionField;
            }
            set
            {
                _expressionField = value;
                OnPropertyChanged("ExpressionField");
            }
        }

        public bool MemoryVisibility
        {
            get
            {
                return _memoryVisibility;
            }
            set
            {
                _memoryVisibility = value;
                OnPropertyChanged("MemoryVisibility");
            }
        }

        public bool ErrorVisibility
        {
            get
            {
                return _errorVisibility;
            }
            set
            {
                _errorVisibility = value;
                OnPropertyChanged("ErrorVisibility");
            }
        }

        #endregion Properties

        #region ICommand

        public ICommand CountCommand
        {
            get
            {
                _clickCommand = new DelegateCommand<object>(CountClick);
                return _clickCommand;
            }
        }

        public ICommand OperatorCommand
        {
            get
            {
                _clickCommand = new DelegateCommand<object>(OperatorClick);
                return _clickCommand;
            }
        }

        public ICommand ChangeSignCommand
        {
            get
            {
                _clickCommand = new DelegateCommand<object>(ChangeSignClick);
                return _clickCommand;
            }
        }

        public ICommand MemoryClearCommand
        {
            get
            {
                _clickCommand = new DelegateCommand<object>(MemoryClearClick);
                return _clickCommand;
            }
        }

        public ICommand MemoryReadCommand
        {
            get
            {
                _clickCommand = new DelegateCommand<object>(MemoryReadClick);
                return _clickCommand;
            }
        }

        public ICommand MemorySaveCommand
        {
            get
            {
                _clickCommand = new DelegateCommand<object>(MemorySaveClick);
                return _clickCommand;
            }
        }

        public ICommand MemoryAddCommand
        {
            get
            {
                _clickCommand = new DelegateCommand<object>(MemoryAddClick);
                return _clickCommand;
            }
        }

        public ICommand MemorySubCommand
        {
            get
            {
                _clickCommand = new DelegateCommand<object>(MemorySubClick);
                return _clickCommand;
            }
        }

        public ICommand ClearBufferCommand
        {
            get
            {
                _clickCommand = new DelegateCommand<object>(ClearBufferClick);
                return _clickCommand;
            }
        }

        public ICommand ClearExpressionCommand
        {
            get
            {
                _clickCommand = new DelegateCommand<object>(ClearExpressionClick);
                return _clickCommand;
            }
        }

        public ICommand BackspaceCommand
        {
            get
            {
                _clickCommand = new DelegateCommand<object>(BackspaceClick);
                return _clickCommand;
            }
        }

        public ICommand ResultCommand
        {
            get
            {
                _clickCommand = new DelegateCommand<object>(ResultClick);
                return _clickCommand;
            }
        }

        #endregion ICommand

        #region Methods

        private void CountClick(object obj)
        {
            var button = obj as Button;
            var content = button.Content.ToString();
            _signClick = false;
            if (ExpressionField.Length > 0)
            {
                if (ExpressionField[ExpressionField.Length - 1].ToString() == ")")
                    return;
            }
            
            if (content == ",")
            {
                if (!_comaActive)
                {
                    if (_isDefaultValue)
                        BufferField = "0";
                    BufferField += _separator;
                    _comaActive = true;
                    _isDefaultValue = false;
                }
            }
            else
            {
                if (_isDefaultValue)
                {
                    BufferField = content;
                    _isDefaultValue = false;
                }
                else
                {
                    if (BufferField == "0")
                    {
                        BufferField = content;
                        return;
                    }
                    // тут надо сделать клево всё!
                    BufferField += content;
                }
            }
        }

        private void ChangeSignClick(object obj)
        {
            // dont forget you can wrap next condition in if(_defaulValue)
            if (BufferField[0] == '-')
            {
                BufferField = BufferField.Remove(0, 1);
            }
            else
            {
                BufferField = BufferField.Insert(0, "-");
            }
        }

        private void MemoryClearClick(object obj)
        {
            Memory memory = new Memory();
            memory.Clear();
            MemoryVisibility = false;
        }

        private void MemoryReadClick(object obj)
        {
            BufferField = new Memory().Read();
        }

        private void MemorySaveClick(object obj)
        {
            Memory memory = new Memory();
            memory.Replace(Convert.ToDouble(BufferField));
            MemoryVisibility = true;
            _isDefaultValue = true;
        }

        private void MemoryAddClick(object obj)
        {
            Memory memory = new Memory();
            memory.Add(Convert.ToDouble(BufferField));
            MemoryVisibility = true;
            _isDefaultValue = true;
        }

        private void MemorySubClick(object obj)
        {
            Memory memory = new Memory();
            memory.Sub(Convert.ToDouble(BufferField));
            MemoryVisibility = true;
            _isDefaultValue = true;
        }

        private void ClearBufferClick(object obj)
        {
            BufferField = "0";
            _isDefaultValue = false;
            _comaActive = false;
            ErrorVisibility = false;
        }

        private void ClearExpressionClick(object obj)
        {
            Clear();
            ErrorVisibility = false;
        }

        private void BackspaceClick(object obj)
        {
            var lenght = BufferField.Length;
            if (lenght > 0)
            {
                if (_isDefaultValue)
                    _isDefaultValue = false;
                var lengthLess = lenght - 1;
                if (BufferField[lengthLess] == _separator[0])
                {
                    _comaActive = false;
                    BufferField = BufferField.Remove(lengthLess, 1);
                    return;
                }
                if (lenght == 1)
                    BufferField = "0";
                else
                    BufferField = BufferField.Remove(lengthLess, 1);
            }
        }

        private void ResultClick(object obj)
        {
            ErrorVisibility = false;
            _comaActive = false;
            // here must be result logic : expression (sign + buffer)
            var expression = String.Empty;
            if (_isCloseBracket)
            {
                expression = ExpressionField;
                oldBuffer = BufferField;
                ExpressionField = String.Empty;
            }
            else if (_isResultClick == false)
            {
                oldBuffer = BufferField;
                expression = ExpressionField + oldBuffer;
                ExpressionField = String.Empty;
            }
            else
            {
                expression = BufferField + OperatorLogic.GetOperator(_lastOp) + oldBuffer;
            }

            string result = String.Empty;
            try
            {
                result = PostfixNotationLogic.Calculate(expression).ToString();
            }
            catch
            {
                Clear();
                ErrorVisibility = true;
                return;
            }
            BufferField = result;
            _isDefaultValue = true;
            _isCloseBracket = false;
            _isDefaultValue = true;
            _isResultClick = true;
            _signClick = false;
        }

        private void OperatorClick(object obj)
        {            
            ErrorVisibility = false;
            _comaActive = false;
            var _operator = obj as string;
            Operators op = ConvertOperator(_operator);
            var opString = OperatorLogic.GetOperator(op);

            if (op == Operators.Close)
            {
                if (_openBrackets > 0)
                {
                    if (ExpressionField.Length > 0)
                    {
                        var newOperator = ExpressionField[ExpressionField.Length - 1].ToString();
                        if (newOperator != ")")
                        {
                            ExpressionField += BufferField + opString;
                            _openBrackets--;
                            _isCloseBracket = true;
                            return;
                        }
                        else
                        {
                            ExpressionField += opString;
                            _openBrackets--;
                            _isCloseBracket = true;
                            return;
                        }
                    }
                    else
                    {
                        ExpressionField += BufferField + opString;
                        _openBrackets--;
                        _isCloseBracket = true;
                        return;
                    }
                }
                else
                    return;
            }
            else if (op == Operators.Open)
            {
                if (ExpressionField.Length > 0)
                {
                    var newOperator = ExpressionField[ExpressionField.Length - 1].ToString();
                    if (IsOperator(newOperator))
                    {
                        ExpressionField += opString;
                        _signClick = true;
                        _openBrackets++;
                        return;
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    ExpressionField += opString;
                    _signClick = true;
                    _openBrackets++;
                    return;
                }
            }

            if (_signClick)
            {
                var index = ExpressionField.Length - 1;
                string lastSymbol = ExpressionField[index].ToString();
                if (IsOperator(lastSymbol) || lastSymbol == ")")
                {
                    ExpressionField = ExpressionField.Remove(index, 1);
                    ExpressionField += opString;
                }
                return;
            }

            if (!_isCloseBracket)
                ExpressionField += BufferField;
            BufferField = "0";
            if (op == Operators.Degree)
            {
                ExpressionField += opString;
                _isDefaultValue = true;
                _signClick = true;
                return;
            }

            var expression = ExpressionField + opString + BufferField;
            _lastOp = op;
            if (_openBrackets > 0) //*************************
            {
                //var splitByBrackets = ExpressionField.Split('(');
                //expression = splitByBrackets[_openBrackets] + opString + BufferField;
                var innerExpression = GetInnerExpression(ExpressionField, _openBrackets);
                expression = innerExpression + opString + BufferField;
            }// ****************************
            string result = String.Empty;
            try
            {
                result = PostfixNotationLogic.Calculate(expression).ToString();
                _signClick = true;
            }
            catch
            {
                Clear();
                ErrorVisibility = true;
                return;
            }
            ExpressionField += opString;
            BufferField = result;
            _isDefaultValue = true;
            _isCloseBracket = false;
            _isResultClick = false;
        }

        private Operators ConvertOperator(string _operator)
        {
            Operators op = Operators.None;
            switch (_operator)
            {
                case "Add":
                    op = Operators.Add;
                    break;
                case "Sub":
                    op = Operators.Sub;
                    break;
                case "Mul":
                    op = Operators.Mul;
                    break;
                case "Div":
                    op = Operators.Div;
                    break;
                case "Open":
                    op = Operators.Open;
                    break;
                case "Close":
                    op = Operators.Close;
                    break;
                case "Degree":
                    op = Operators.Degree;
                    break;
                default:
                    break;
            }
            return op;
        }

        private string GetInnerExpression(string expression, int brackets)
        {
            if (brackets > 0 && expression.Contains('('))
            {
                int index = -99;
                var result = expression;
                for (var i = 0; i < brackets; i++)
                {
                    index = result.IndexOf('(');
                    result = result.Remove(0, index + 1);
                }

                return result;
            }
            else
                return null;
        }

        private bool IsOperator(string input)
        {
            return (Regex.IsMatch(input, @"[*|+|\-|/|^]$")) ? true : false;
        }

        private void Clear()
        {
            ExpressionField = String.Empty;
            BufferField = "0";
            _isDefaultValue = false;
            _comaActive = false;
            _openBrackets = 0;
            _isCloseBracket = false;
            _isResultClick = false;
            _signClick = false;
        }

        #endregion Methods
    }
}
