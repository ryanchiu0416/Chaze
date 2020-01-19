using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chaze.Exceptions
{
    class AccountManagementException : Exception
    {
        public AccountManagementException(string message)
            : base(message)
        {
        }
    }
}
