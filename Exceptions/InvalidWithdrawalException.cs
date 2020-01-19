using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chaze.Exceptions
{
    class InvalidWithdrawalException: Exception
    {
        public InvalidWithdrawalException(string message)
            : base(message)
        { }
    }
}
