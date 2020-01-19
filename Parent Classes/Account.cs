using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Chaze.Utility_Classes;
using Chaze.Exceptions;


namespace Chaze.Parent_Classes
{
    abstract class Account
    {
        
        protected double balanceDouble;
        protected double fee;
        public int accountNumber { get; protected set; }

        public void IncreaseBalance( double amount)
        {
            balanceDouble += amount;
        }

        public void DecreaseBalance( double amount)
        {
            try
            {
                Validator.VerifyBalance(balanceDouble, amount);
                balanceDouble -= amount;
            }
            catch (InsufficientFundsException inEx)
            {
                throw inEx;
            }
        }

        public override string ToString()
        {
            return "Account Type: " + GetType().Name +"\r\n" +
                "Account Number: " + accountNumber + "\r\n"
                + "Balance: " + balanceDouble + "\r\n";
        }
    }
}
