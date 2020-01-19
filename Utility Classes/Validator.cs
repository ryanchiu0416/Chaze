using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Chaze.Exceptions;
using Chaze.Parent_Classes;
using Chaze.Core_Classes;

namespace Chaze.Utility_Classes
{
    static class Validator
    {
        public static void VerifyUser(bool success)
        {
            if (success == false)
            {
                throw new NotAuthorizedException("Wrong username or password");
            }
        }


        public static void VerifyBalance( double currentBalance, double withdrawalAmount )
        {
            if (currentBalance < withdrawalAmount)
            {
                throw new InsufficientFundsException("Balance Insufficient");
            }
        }


        public static void VerifyAssets( InvestmentAccount account, Asset asset )
        {
            bool assetFound = false;

            foreach (Asset listedItem in account.AssetList)
            {
                if (asset.GetType() == typeof(Stock) && ((Stock)asset).Symbol == ((Stock)listedItem).Symbol)
                {
                    assetFound = true;
                }
                else if (asset.GetType() == typeof(Forex) && ((Forex)asset).currencyPair == ((Forex)listedItem).currencyPair)
                {
                    assetFound = true;
                }
            }

            if (assetFound == false)
            {
                throw new InvalidTradeException("Invalid Trade request: asset not found.");
            }
            
        }



        public static void VerifyIfAccountExists(List<Account> AccountList, Type AccountType)
        {
            bool AccountExists = false;
            foreach (Account account in AccountList)
            {
                if (account.GetType() == AccountType)
                {
                    AccountExists = true;
                }
            }

            if (AccountExists == true)
            {
                throw new AccountManagementException("Same Type of Account Already Exists.");
            }
        }
        

    }
}
