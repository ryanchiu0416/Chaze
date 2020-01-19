using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chaze.Parent_Classes;
using Chaze.Enums;

namespace Chaze.Core_Classes
{
    class Forex: Asset
    {
        //keep track of currency pairs
        public int quantity { get; private set; }
        public CurrencyPairs currencyPair { get; protected set; }
        public int tranQuantity { get; private set; }

        public Forex()
        {
        }

        public Forex(double value, CurrencyPairs currencyPair, int quantity)
        {
            this.Value = value;
            this.currencyPair = currencyPair;
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

        public void changeQuantity(int num)
        {
            quantity -= num;
        }

        public void QuantityToZero()
        {
            quantity = 0;
        }

        public override string ToString()
        {
            return " Currency Pair " + currencyPair + " | Q: " + tranQuantity + "\r\n" +
                   " Asset Type: " + GetType().Name + "\r\n" +
                   " Asset Value: " + Value + "\r\n";
        }



    }
}
