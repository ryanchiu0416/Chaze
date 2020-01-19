using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chaze.Parent_Classes;

namespace Chaze.Core_Classes
{
    class CheckingAccount: Account
    {
        public CheckingAccount(double balance, double fee, int accountNum)
        {
            this.balanceDouble = balance;
            this.fee = fee;
            this.accountNumber = accountNum;
        }
    }
}
