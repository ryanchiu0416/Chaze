using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Chaze.Parent_Classes;
using Chaze.Core_Classes;
using Chaze.Enums;
using Chaze.Utility_Classes;
using Chaze.Exceptions;

namespace Chaze.Core_Classes
{
    class InvestmentAccount: Account
    {
        private bool isForex;
        public List<Asset> AssetList {get; protected set; } 
        public List<Trade> TradeList = new List<Trade>();

     
        public InvestmentAccount(int accountID, double balance, double fee, bool isForex)
        {
            this.accountNumber = accountID;
            this.balanceDouble = balance;
            this.fee = fee;
            this.isForex = isForex;
            AssetList = new List<Asset>();
        }

        public void TradeStock( string symbol, double price, int quantity, char action )
        {
            

            try
            {
                Stock newstock = new Stock(price, symbol, quantity);


                Trade stockTrade = new Trade(this, newstock, action);
                TradeList.Add(stockTrade);
            }
            catch (InsufficientFundsException ex)
            {
                throw ex;
            }
            catch (InvalidTradeException tradeEx)
            {
                throw tradeEx;
            }
        }

        public void TradeForex( CurrencyPairs currencyPair, double price, int quantity, char action )
        {
            try
            {
                if (isForex == false)
                {
                    throw new InvalidTradeException("This account is not eligible for FOREX.");
                }
 
                try
                {
                    Forex newForex = new Forex(price, currencyPair, quantity);
                    Trade forexTrade = new Trade(this, newForex, action);
                    TradeList.Add(forexTrade);
                }
                catch (InsufficientFundsException insufficientFundEx)
                {
                    throw insufficientFundEx;
                }
            }
            catch (InvalidTradeException forexEx)
            {
                throw forexEx;
            }
        }

        public void AddInvestmentAssets( Asset asset )
        {
            AssetList.Add(asset);
        }

        public void RemoveInvestmentAssets( Asset asset)
        {
            AssetList.Remove(asset);
        }

        public override string ToString()
        {
            string baseString = (base.ToString() + " =========List of trades=========" + "\r\n");

            foreach (Trade trade in TradeList)
            {
                baseString += trade.ToString() + "\r\n";
            }

            return baseString;
        }
    }
}
