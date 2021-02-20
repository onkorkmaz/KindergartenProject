using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Entity
{

    public class DataResultArgs<T>
    {
        private bool _hasError;
        public bool HasError
        {
            get { return _hasError; }
            set { _hasError = value; }
        }
        private T _result;
        public T Result
        {
            get { return _result; }
            set { _result = value; }
        }
        private Exception _myException;
        public Exception MyException
        {
            get { return _myException; }
            set { _myException = value; }
        }
        private string _errorDescription;
        public string ErrorDescription
        {
            get { return _errorDescription; }
            set { _errorDescription = value; }
        }
        private int _errorCode;
        public int ErrorCode
        {
            get { return _errorCode; }
            set { _errorCode = value; }
        }
    }
}