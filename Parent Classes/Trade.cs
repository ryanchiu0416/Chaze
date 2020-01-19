using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Chaze.Core_Classes;
using Chaze.Exceptions;
using Chaze.Utility_Classes;

namespace Chaze.Parent_Classes
{
    class Trade
    {
        public Asset Asset { get; private set; }
        public char Action { get; private set; }

        public Trade()
        {

        }

        public Trade(InvestmentAccount account, Asset asset, char action)
        {

            Action = action;  //buy or sell
            Asset = asset;


            if (action == 'b')  // b for buy
            {
                try
                {
                    
                    asset.WithdrawAsset(account); //Withdraw funds to cover the buy
                    account.AddInvestmentAssets(asset); //Asset bought. Add to account.
           
                }
                catch (InsufficientFundsException)
                {

                    throw;
                }

            }
            else  //we are selling
            {
                try
                {

                    Validator.VerifyAssets(account, asset); //are we trying to sell what we don't have?

                    RemoveTradeAsset(account, asset);

                    asset.DepositAsset(account); //but adds funds gained from sale

                    
                }
                catch (InvalidTradeException)
                {

                    throw;
                }
            }
        }

        private static void RemoveTradeAsset(InvestmentAccount account, Asset asset)
        {

            int SoldQuantity;
            int totalOwnedQuantity = 0;

            if (asset.GetType() == typeof(Stock))
            {


                SoldQuantity = ((Stock)asset).quantity;

                foreach (Stock ListStock in account.AssetList.OfType<Stock>())
                {
                    if (ListStock.Symbol == ((Stock)asset).Symbol)
                    {
                        totalOwnedQuantity += ListStock.quantity;
                    }
                }

                if (totalOwnedQuantity < SoldQuantity)
                {

                    throw new InvalidTradeException("Not enough shares to trade with.");

                }
                else
                {
                    foreach (Stock ListStock in account.AssetList.OfType<Stock>())
                    {
                        if (ListStock.Symbol == ((Stock)asset).Symbol)
                        {


                            if (SoldQuantity < ListStock.quantity)
                            {
                                //reduce share, no removal
                                ListStock.changeQuantity(SoldQuantity);

                                SoldQuantity = 0;

                            }
                            else
                            {
                                //remove asset
                                SoldQuantity -= ListStock.quantity;
                                ListStock.QuantityToZero();

                            }
                        }
                    }
                }


            }
            else
            {


                SoldQuantity = ((Forex)asset).quantity;

                foreach (Forex ListForex in account.AssetList.OfType<Forex>())

                    if (ListForex.currencyPair == ((Forex)asset).currencyPair)
                    {
                        totalOwnedQuantity += ListForex.quantity;
                    }

                if (totalOwnedQuantity < SoldQuantity)
                {

                    throw new InvalidTradeException("Not enough quantities to trade with.");

                }
                else
                {
                    foreach (Forex ListForex in account.AssetList.OfType<Forex>())
                    {
                        if (ListForex.currencyPair == ((Forex)asset).currencyPair)
                        {


                            if (SoldQuantity < ListForex.quantity)
                            {
                                //reduce share, no removal
                                ListForex.changeQuantity(SoldQuantity);

                                SoldQuantity = 0;

                            }
                            else
                            {
                                //remove asset
                                SoldQuantity -= ListForex.quantity;
                                ListForex.QuantityToZero();

                            }
                        }
                    }
                }

            }

            int i = 0;
            while (i < account.AssetList.Count)
            {
                if ((account.AssetList[i].GetType() == typeof(Stock) && ((Stock)account.AssetList[i]).quantity == 0)
                    || (account.AssetList[i].GetType() == typeof(Forex) && ((Forex)account.AssetList[i]).quantity == 0))
                {
                    account.RemoveInvestmentAssets(account.AssetList[i]);

                }
                else
                {
                    i++;
                }
            }

        }


        public override string ToString()
        {

            return Asset.ToString() +
                 " Action : " + (Action == 'b' ? "Buy" : "Sell") + "\r\n";
        }
    }
}
