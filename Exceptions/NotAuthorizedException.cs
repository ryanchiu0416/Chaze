using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Chaze.Exceptions
{
    class NotAuthorizedException: Exception
    {
        public NotAuthorizedException(string message): base(message)
        {
        }
    }
}
