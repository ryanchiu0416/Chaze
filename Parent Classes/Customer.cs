using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Chaze.Utility_Classes;
using Chaze.Exceptions;
using Chaze.Core_Classes;

namespace Chaze.Parent_Classes
{
    class Customer
    {
        public List<Account> AccountList { get; protected set; }
        private int customerID;
        private string customerName;
        private string customerDL;
        private static int customerCount = 0;

        public Customer(string name, string DL) 
        {
            customerName = name;
            customerDL = DL;
            
            AccountList = new List<Account>();

            customerCount++;
            customerID = customerCount - 1;

            
        }

        public void OpenInvestmentAccount(double balance, double fee, int accountNum, bool isForex)
        {
            try
            {
                Validator.VerifyIfAccountExists(AccountList, typeof(InvestmentAccount));
                InvestmentAccount Investmentacc = new InvestmentAccount(accountNum, balance, fee, isForex);

                AccountList.Add(Investmentacc);

            }
            catch (AccountManagementException accEx)
            {
                throw accEx;
            }
        }


        public void OpenCheckingAccount(double balance, double fee, int accountNum)
        {
            try
            {
                Validator.VerifyIfAccountExists(AccountList, typeof(CheckingAccount));
                CheckingAccount checkingAcc = new CheckingAccount(balance, fee, accountNum);
                AccountList.Add(checkingAcc);
            }
            catch(AccountManagementException accEx)
            {
                throw accEx;
            }
        }


        public int returnCustomerID()
        {
            
            return customerID;
        }

        public string returnCustomerName()
        {
            return customerName;
        }

        public string returnCustomerDL()
        {
            return customerDL;
        }

        public override string ToString()
        {
            return "Customer ID: " + this.returnCustomerID() + "\r\n"
                + "Customer Name: " + customerName + "\r\n";
        }

        public static void Reset()
        {
            customerCount = 0;
        }
    }
}
