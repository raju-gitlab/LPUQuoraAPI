using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LPUQa.UTILITIES.CustomeExceptions
{
    [Serializable]
    public class CustomeException : Exception
    {
        private string _errorCode;
        public string ErrorCode
        {
            get
            {
                return _errorCode;
            }
            set
            {
                _errorCode = value;
            }
        }

        private string _errorMessege;
        public string ErrorMessege
        {
            get
            {
                return _errorMessege;
            }
            set
            {
                _errorMessege = value;
            }
        }

        private string _responseType;
        public string ResponseType
        {
            get
            {
                return _responseType;
            }
            set
            {
                _responseType = value;
            }
        }
        private string _errorDescription;
        public string ErrorDescription
        {
            get
            {
                return _errorDescription;
            }
            set
            {
                _errorDescription = value;
            }
        }

        public CustomeException() { }
        public CustomeException(string Messege) : base(Messege)
        {
            _errorMessege = Messege;
        }
        public CustomeException(string errorCode, string message, string description)
        {
            _errorCode = errorCode;
            _errorMessege = message;
            _errorDescription = description;
        }
        public CustomeException(string errorCode, string message)
        {
            _errorCode = errorCode;
            _errorMessege = message;
        }
        public CustomeException(string code, Exception ex)
        {
            _errorCode = code;
            _errorDescription = ex.Message + ex.StackTrace;
            _errorMessege = ex.Message;
        }
        public CustomeException(string code, string messge, Exception ex)
        {
            _errorCode = code;
            _errorMessege = messge;
            _errorDescription = ex.Message + ex.StackTrace;
        }
    }
}
