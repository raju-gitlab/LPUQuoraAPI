using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LPUQa.UTILITIES.CustomeExceptions
{
    public class UserExceptions : CustomeException
    {
        public UserExceptions() {}
        public UserExceptions(string ExceptionName, string ErrorCode, string ExceptionMessege, string LocatedIn):base(ExceptionName,ErrorCode,ExceptionMessege)
            { }
    }
}
