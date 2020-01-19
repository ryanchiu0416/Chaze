using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chaze.Exceptions
{
    class InsufficientFundsException: Exception
    {
        public InsufficientFundsException(string message)
            : base(message)
        { }
    }
}
