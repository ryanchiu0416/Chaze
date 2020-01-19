using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chaze.Exceptions
{
    class CustomerNotFoundException: Exception
    {
        public CustomerNotFoundException(string message)
            : base(message)
        {
        }
    }
}
