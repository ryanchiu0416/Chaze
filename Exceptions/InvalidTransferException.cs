using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chaze.Exceptions
{
    class InvalidTransferException: Exception
    {
        public InvalidTransferException(string message)
            : base(message)
        { }
    }
}
