using System;

namespace Calculator.Helpers
{
    internal class Memory
    {
        #region Constructors

        public Memory()
        { }

        #endregion Constructors

        #region Fields

        private static double _buffer = 0.0d;
        private static bool _isEmpty = true;

        #endregion Fields

        #region Properties

        public double Buffer {
            get
            {
                return _buffer;
            }
            set
            {
                _buffer = value;
            }
        }

        public bool IsEmpty
        {
            get 
            {
                return _isEmpty;
            }
            set
            {
                _isEmpty = value;
            }
        }

        #endregion Properties

        #region Methods

        public string Read()
        {
            return (!IsEmpty)? Buffer.ToString() : "0";
        }

        public void Clear()
        {
            Buffer = 0.0d;
            IsEmpty = true;
        }

        public void Add(double val)
        {
            Buffer += val;
            IsEmpty = false;
        }

        public void Sub(double val)
        {
            Buffer -= val;
            IsEmpty = false;
        }

        public void Replace(double val)
        {
            Buffer = val;
            IsEmpty = false;
        }

        #endregion Methods
    }
}
