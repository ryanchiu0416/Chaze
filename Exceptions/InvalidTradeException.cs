using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chaze.Exceptions
{
    class InvalidTradeException: Exception
    {
        public InvalidTradeException(string message)
            : base(message)
        { }
    }
}
