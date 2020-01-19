using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chaze.Parent_Classes;
using FinancialMarket;

namespace Chaze.Core_Classes
{
    class Stock: Asset
    {
        public string Symbol { get; private set; }

        public int quantity { get; private set; }

        public int tranQuantity { get; private set; }

        public Stock()
        { }

        public Stock(double value, string symbol, int quantity)
        {
            this.Value = value;
            Symbol = symbol;
            this.quantity = quantity;
            this.tranQuantity = quantity;
        }

        public override void DepositAsset(Account acct)
        {
            acct.IncreaseBalance(Value * quantity);
        }

        public override void WithdrawAsset(Account acct)
        {
            acct.DecreaseBalance(Value * quantity);
        }

        public override string ToString()
        {
            return " Symbol: " + Symbol.ToUpper() + " | Q: " + tranQuantity + "\r\n" +
                   " Asset Type: " + GetType().Name + "\r\n" +
                   " Asset Value: " + Value + "\r\n";
        }

        public void changeQuantity(int numQuan)
        {
            quantity -= numQuan;
        }

        public void QuantityToZero()
        {
            quantity = 0;
        }
        

       


    }
}
