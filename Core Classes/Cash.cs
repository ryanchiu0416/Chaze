using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Chaze.Parent_Classes;

namespace Chaze.Core_Classes
{
    class Cash: Asset
    {
        public Cash() { }

        public Cash(double value)
        {
            this.Value = value;
        }

    }
}
