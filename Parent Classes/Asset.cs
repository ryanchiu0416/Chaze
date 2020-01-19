using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chaze.Interfaces;
using Chaze.Exceptions;

namespace Chaze.Parent_Classes
{
    abstract class Asset: IAssetManagement
    {
        protected double Value;
                                                                                                                       
        public virtual void DepositAsset ( Account accountName )
        {
            accountName.IncreaseBalance(Value);
        }

        public virtual void WithdrawAsset(Account accountName)
        {
            accountName.DecreaseBalance(Value);
        }


        public void TransferAsset(Account fromAccount, Account toAccount)
        {
            if( fromAccount.GetType() == toAccount.GetType() )
            {
                throw new InvalidTransferException("Cannot transfer between same type of accounts.");
            }
            else
            {
                fromAccount.DecreaseBalance(Value);
                toAccount.IncreaseBalance(Value);
            }
        }
    }
}
