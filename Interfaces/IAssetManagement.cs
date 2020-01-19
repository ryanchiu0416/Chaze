using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chaze.Parent_Classes;
namespace Chaze.Interfaces
{
    interface IAssetManagement
    {
        //apply on assets 

         void DepositAsset(Account account);

         void WithdrawAsset (Account account );

         void TransferAsset(Account fromAccount, Account toAccount);
    }
}
