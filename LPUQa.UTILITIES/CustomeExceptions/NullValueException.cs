using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LPUQa.UTILITIES.CustomeExceptions
{
    public class NullValueException : CustomeException
    {
        public NullValueException(){}
        public NullValueException(string Messege){ }
        public NullValueException(Exception ex){ }
    }
}
