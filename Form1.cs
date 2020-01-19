/*
 * 03-19-2019
 * Final Exam - This banking app serves many purposes. Allow user to create accounts, buy assets, trade, depost, withdraw, etc. 
 * Ryan Chiu
 * 
 */


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Chaze.Utility_Classes;
using Chaze.Exceptions;
using Chaze.Parent_Classes;
using Chaze.Core_Classes;
using Chaze.Enums;
using FinancialMarket;

namespace Chaze
{
    public partial class Chaze : Form
    {
        private List<Customer> CustomerList = new List<Customer>();
        private Customer newCustomer;
        private CurrencyPairs currentSelectedCurrencyPair = CurrencyPairs.None;
        string stockSymbol;

        public Chaze()
        {
            InitializeComponent();
        }

        private void loginButton_Click(object sender, EventArgs e)
        {
            string usr = loginBox.Text;
            string pw = passwordBox.Text;
         
            Login(usr, pw);
        }

        private void exitButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void bankerButton_Click(object sender, EventArgs e)
        {
            AuthenticateBanker();
        }


        //My methods
        private void AuthenticateBanker()
        {
          if (Employee.IsBanker())
          {
            bankerGroup.Show();
          }
        }

        private void Login(string usr, string pw)
        {
            try
            {
                Validator.VerifyUser(Employee.CheckIn(usr, pw));

                //Hide auth
                authBox.Hide();

                //Show others
                customerBox.Show();
                customerInfoBox.Show();
                bankTellerBox.Show();
            }
            catch (Exception ex)
            {
                if (ex.GetType() == typeof(NotAuthorizedException))
                {
                    MessageBox.Show(ex.Message);
                }
            }
         
        }

        private void newCustomer_CheckedChanged(object sender, EventArgs e)
        {

            customerIDBox.Enabled = false;
            verifyButton.Enabled = false;
            addCustomerButton.Enabled = true;
        }

        private void existingCustomer_CheckedChanged(object sender, EventArgs e)
        {
            customerIDBox.Enabled = true;
            verifyButton.Enabled = true;
            addCustomerButton.Enabled = false;
        }

        private void verifyButton_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (Customer cus in CustomerList)
                {
                    if (cus.returnCustomerID() == int.Parse(customerIDBox.Text))
                    {
                        newCustomer = cus;
                        customerNameBox.Text = cus.returnCustomerName();
                        customerDLBox.Text = cus.returnCustomerDL();
                        

                        EnableBankerControls();
                        MessageBox.Show("Verified!"); return;
                    }
                }

                throw new CustomerNotFoundException("Customer Not Found.");
            }
            catch (CustomerNotFoundException cusEx)
            {
                MessageBox.Show(cusEx.Message);
            }
            catch
            {
                MessageBox.Show("All fields are required.");
            }
            
        }

        private void summaryButton_Click(object sender, EventArgs e)
        {
            if (newCustomer == null)
            {
                MessageBox.Show("No Customer is selected.");
            }
            else
            {
                string AccountInfos = "===========Lists of Accounts============" + "\r\n";
 
                foreach(Account acc in newCustomer.AccountList)
                {
                   AccountInfos += acc.ToString() + "\r\n";
                }
                resultBox.AppendText("\r\n" + "==========Customer Information============" + "\r\n" + newCustomer.ToString()
                    + AccountInfos );
            }

                //Show output
                OutputGroupBox.Show();
        }

        private void tradeButton_Click(object sender, EventArgs e)
        {
            //Show trade controls
            tradeGroupBox.Show();
            
        }

        private void buyAssetButton_Click(object sender, EventArgs e)
        {
            try
            {
                double price = double.Parse(stockPriceBox.Text);
                int quantity = int.Parse(assetQuantityBox.Text);
                bool invAccFound = false;

                if (quantity > 0)
                {
                    foreach (Account account in newCustomer.AccountList)
                    {
                        if (account.GetType() == typeof(InvestmentAccount))
                        {
                            invAccFound = true;
                        }
                    }


                    if (invAccFound)
                    {
                        foreach (InvestmentAccount invAcc in newCustomer.AccountList.OfType<InvestmentAccount>())
                        {

                            if (currentSelectedCurrencyPair == CurrencyPairs.None)
                            {
                                //stock trade
                                invAcc.TradeStock(stockSymbol, price, quantity, 'b');
                            }
                            else
                            {
                                //forex trade
                                invAcc.TradeForex(currentSelectedCurrencyPair, price, quantity, 'b');
                            }
                        }

                        HideTradeBox();
                    }
                    else
                    {
                        throw new AccountManagementException("Investment Account does not exist.");
                    }

                }
                else
                {
                    MessageBox.Show("Quantity must be positive.");
                }
           
            }
            catch(InvalidTradeException forexEx)
            {
                MessageBox.Show(forexEx.Message);
            }
            catch(InsufficientFundsException insufficientFundEx)
            {
                MessageBox.Show(insufficientFundEx.Message);
            }
            catch (AccountManagementException AccEx)
            {
                MessageBox.Show(AccEx.Message);
            }
            catch
            {
                MessageBox.Show("All fields are required \r\n OR \r\n Quantity should be in interger.");
            }
        }

        private void sellAssetButton_Click(object sender, EventArgs e)
        {
            try
            {
                double price = double.Parse(stockPriceBox.Text);
                int quantity = int.Parse(assetQuantityBox.Text);
                bool invAccFound = false;

                if (quantity > 0)
                {
                    foreach (Account account in newCustomer.AccountList)
                    {
                        if (account.GetType() == typeof(InvestmentAccount))
                        {
                            invAccFound = true;
                        }
                    }

                    if (invAccFound)
                    {
                        foreach (InvestmentAccount invAcc in newCustomer.AccountList.OfType<InvestmentAccount>())
                        {

                            if (currentSelectedCurrencyPair == CurrencyPairs.None)
                            {
                                //stock trade  
                                invAcc.TradeStock(stockSymbol, price, quantity, 's');

                            }
                            else
                            {
                                //forex trade  
                                invAcc.TradeForex(currentSelectedCurrencyPair, price, quantity, 's');
                            }

                        }

                        HideTradeBox();
                    }
                    else
                    {
                        throw new AccountManagementException("Investment Account does not exist.");
                    }

                }
                else
                {
                    MessageBox.Show("Quantity must be positive.");
                }

            }
            catch (InvalidTradeException tradeEx)
            {
                MessageBox.Show(tradeEx.Message);
            }
            catch (AccountManagementException accEx)
            {
                MessageBox.Show(accEx.Message);
            }
            catch 
            {
                MessageBox.Show("All fields are required \r\n OR \r\n Quantity should be in interger.");
            }


        }

        public void EnableBankerControls()
        {
            //Show controls
            verifyButton.Enabled = false;
            withButton.Enabled = true;
            depotButton.Enabled = true;
            tradeButton.Enabled = true;
            transferButton.Enabled = true;
            account2Combo.Enabled = true;
            accountCombo.Enabled = true;
            summaryButton.Enabled = true;
            amountPriceBox.Enabled = true;
            accountRefreshButton.Enabled = true;
        }

        public void HideTradeBox()
        {
            //Show trade controls
            tradeGroupBox.Hide();
        }


        public void HideBankerBox()
        {
            //Show trade controls
            bankerGroup.Hide();
        }


        private void finishBunkerButton_Click(object sender, EventArgs e)
        {
            HideBankerBox();
        }

        private void cancelBankerButton_Click(object sender, EventArgs e)
        {
            HideBankerBox();
        }

        private void AddInvestment_Click(object sender, EventArgs e)
        {

            try
            {
                int invAccountNum;
                double invOpenBalance, invFee;
                bool isForex;

                invAccountNum = int.Parse(invAccountNumBox.Text);
                invOpenBalance = double.Parse(invBalanceBox.Text);
                invFee = double.Parse(invFeeBox.Text);
                isForex = forexCheck.Checked;

                newCustomer.OpenInvestmentAccount(invOpenBalance, invFee, invAccountNum, isForex);

                MessageBox.Show("Investment Account " + invAccountNum.ToString() + " Created.");
            }
            catch (AccountManagementException accEx)
            {
                MessageBox.Show(accEx.Message);
            }
            catch
            {
                MessageBox.Show("All fields are required.");
            }
            
        }

        private void accountRefreshButton_Click(object sender, EventArgs e)
        {
            accountCombo.DataSource = null;
            account2Combo.DataSource = null;
            accountCombo.DataSource = newCustomer.AccountList;
            account2Combo.DataSource = newCustomer.AccountList.ToList();
            accountCombo.DisplayMember = "accountNumber";
            account2Combo.DisplayMember = "accountNumber";
            accountCombo.ValueMember = null;
            account2Combo.ValueMember = null;
        }

        private void addCustomerButton_Click(object sender, EventArgs e)
        {
            string customerName;
            string customerDL;
            

            customerName = customerNameBox.Text;
            customerDL = customerDLBox.Text;
            

            newCustomer = new Customer(customerName, customerDL);

            CustomerList.Add(newCustomer);
           

            customerIDBox.Text = newCustomer.returnCustomerID().ToString();
           
            MessageBox.Show("New customer has been added!");

            EnableBankerControls(); 
        }

        private void depotButton_Click(object sender, EventArgs e)
        {
            try
            {
                double amount = double.Parse(amountPriceBox.Text);
                if (amount > 0)
                {
                    Account account = (Account)accountCombo.SelectedItem;

                    if (account == null)
                    {
                        throw new AccountManagementException("Customer does not have any accounts \r\n OR \r\n No account has been selected.");
                    }
                    else
                    {
                        Cash cashAmount = new Cash(amount);
                        cashAmount.DepositAsset(account);
                        amountPriceBox.Clear();
                    }
                }
                else
                {
                    MessageBox.Show("amount must be positive");
                }
            }
            catch (AccountManagementException accMgmtEx)
            {
                MessageBox.Show(accMgmtEx.Message);
            }
            catch
            {
                MessageBox.Show("All fields are required.");
            }
        }

        private void forexPriceButton_Click(object sender, EventArgs e)
        {
            
          
        }

        private void customerBox_Enter(object sender, EventArgs e)
        {

        }

        private void Chaze_Load(object sender, EventArgs e)
        {
           //Load
        }

        private void stockSearchButton_Click(object sender, EventArgs e)
        {
            
            OutputGroupBox.Show();
          
            
            stockSymbol = stockSearchBox.Text;

            OutputGroupBox.Show();

            currentSelectedCurrencyPair = CurrencyPairs.None;

            resultBox.Text = stockSymbol.ToString() + "   " + DateTime.Now.ToString();

            StockOutput(stockSymbol);
        }

        

        private void btcButton_Click(object sender, EventArgs e)
        {

            currentSelectedCurrencyPair = CurrencyPairs.BTC;
            OutputGroupBox.Show();
            stockSearchBox.Clear();

            resultBox.Text = currentSelectedCurrencyPair.ToString() + "   " + DateTime.Now.ToString();

            btcOutput();
        }

        private void eurusdButton_Click(object sender, EventArgs e)
        {

            currentSelectedCurrencyPair = CurrencyPairs.EUR2USD;
            OutputGroupBox.Show();
            stockSearchBox.Clear();
            resultBox.Text = currentSelectedCurrencyPair.ToString() + "   " + DateTime.Now.ToString();

            ForexOutput();
        }

        private void usdjpyButton_Click(object sender, EventArgs e)
        {
            currentSelectedCurrencyPair = CurrencyPairs.USD2JPY;
            OutputGroupBox.Show();
            stockSearchBox.Clear();
            resultBox.Text = currentSelectedCurrencyPair.ToString() + "   " + DateTime.Now.ToString();
            ForexOutput();
        }

        private void gbpusdButton_Click(object sender, EventArgs e)
        {
            currentSelectedCurrencyPair = CurrencyPairs.GBP2USD;
            OutputGroupBox.Show();
            stockSearchBox.Clear();
            resultBox.Text = currentSelectedCurrencyPair.ToString() + "   " + DateTime.Now.ToString();
            ForexOutput();
        }

        private void usdchfButton_Click(object sender, EventArgs e)
        {
            currentSelectedCurrencyPair = CurrencyPairs.USD2CHF;
            OutputGroupBox.Show();
            stockSearchBox.Clear();
            resultBox.Text = currentSelectedCurrencyPair.ToString() + "   " + DateTime.Now.ToString();
            ForexOutput();
        }

        private void cnyusdButton_Click(object sender, EventArgs e)
        {
            currentSelectedCurrencyPair = CurrencyPairs.CNY2USD;
            OutputGroupBox.Show();
            stockSearchBox.Clear();
            resultBox.Text = currentSelectedCurrencyPair.ToString() + "   " + DateTime.Now.ToString();
            ForexOutput();
        }

        private void withButton_Click(object sender, EventArgs e)
        {
            try
            {
                double withdrawalAmount = double.Parse(amountPriceBox.Text);
                if (withdrawalAmount > 0)
                {
                    Account account = (Account)accountCombo.SelectedItem;

                    if(account == null)
                    {
                        throw new AccountManagementException("Customer does not have any accounts \r\n OR \r\n No account has been selected.");
                    }
                    else
                    {
                        Cash cashAmount = new Cash(withdrawalAmount);
                        cashAmount.WithdrawAsset(account);
                        amountPriceBox.Clear();
                    }
                }
                else
                {
                    MessageBox.Show("Withdrawal amount must be positive.");
                }
            }
            catch(AccountManagementException accEx)
            {
                MessageBox.Show(accEx.Message);
            }
            catch(InsufficientFundsException inEx)
            {
                MessageBox.Show(inEx.Message);
            }
            catch
            {
                MessageBox.Show("All fields are required.");
            }
        }




        private void StockOutput(string Symbol)
        {
            StockMarket stockmarket = new StockMarket("MWSOY45W94DB1V3J");
            string finalOutputString;

            try
            {
                List<Equity> equityInfos = stockmarket.GetEquityInfo(Symbol);

                Equity latestData = equityInfos[0];


                string outputString = string.Format("The current open price of {0} is {1:C2}, highest it has been" +
                    "is {2:C2}, lower it has been is {3:C2}, current volume is {4}, stock info is pulled at {5}, " +
                    "current time is {6:d} at {6:t}", latestData.Symbol, latestData.Open, latestData.High, latestData.Low,
                    latestData.Volume, latestData.Timestamp, DateTime.Now);
                finalOutputString = outputString;

                foreach (Equity equity in equityInfos)
                {
                    string historicalString = String.Format("" +
                        "Open: {1:C2} \r\n " +
                        "Highest: {2:C2} \r\n " +
                        "Lowest: {3:C2} \r\n " +
                        "Volume: {4} \r\n " +
                        "Stock info is pulled at:  {5}, \r\n  " +
                        "Current time: {6:d} at {6:t}", equity.Symbol, equity.Open,
                        equity.High, equity.Low, equity.Volume, equity.Timestamp, DateTime.Now);

                    finalOutputString += "\r\n" + "\r\n" + historicalString;
                }

                resultBox.AppendText(finalOutputString);
                stockPriceBox.Text = latestData.Open.ToString();
            }
            catch (APICallLimitReachedException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (Exception Eex)
            {
                MessageBox.Show(Eex.Message);
            }
        }


        public void ForexOutput()
        {
            StockMarket stockmarket = new StockMarket("MWSOY45W94DB1V3J");
            string finalOutputString;
            string [] currencyInput;
            
            string FromCurrency;
            string ToCurrency;

            currencyInput = currentSelectedCurrencyPair.ToString().Split('2');
            FromCurrency = currencyInput[0];
            ToCurrency = currencyInput[1];
            
            try
            {
                List<FOREX> GetForexInfo = stockmarket.GetForexInfo(FromCurrency, ToCurrency);

                FOREX latestData = GetForexInfo[0];


                string outputString = string.Format("The current open price of {0} is {1:C2}, highest it has been" +
                    "is {2:C2}, lower it has been is {3:C2}, close is {4}, FOREX info is pulled at {5}, " +
                    "current time is {6:d} at {6:t}", latestData.Pair, latestData.Open, latestData.High, latestData.Low,
                    latestData.Close, latestData.Timestamp, DateTime.Now);
                finalOutputString = outputString;

                foreach (FOREX forex in GetForexInfo)
                {
                    string historicalString = String.Format("" +
                        "Open: {1:C2} \r\n " +
                        "Highest: {2:C2} \r\n " +
                        "Lowest: {3:C2} \r\n " +
                        "Close: {4} \r\n " +
                        "FOREX info is pulled at:  {5}, \r\n  " +
                        "Current time: {6:d} at {6:t}", forex.Pair, forex.Open,
                        forex.High, forex.Low, forex.Close, forex.Timestamp, DateTime.Now);

                    finalOutputString += "\r\n" + "\r\n" + historicalString;
                }

                resultBox.AppendText(finalOutputString);
                stockPriceBox.Text = latestData.Open.ToString();
            }
            catch (APICallLimitReachedException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (Exception Eex)
            {
                MessageBox.Show(Eex.Message);
            }
        }


        public void btcOutput()
        {
            StockMarket stockmarket = new StockMarket("MWSOY45W94DB1V3J");
            string finalOutputString;
            


            try
            {
                List<BitCoin> GetBTCInfo = stockmarket.GetBitCoinInfo();

                BitCoin latestData = GetBTCInfo[0];


                string outputString = string.Format("The current open price of {0} in China is {1:C2}, highest it has been" +
                    "is {2:C2}, lower it has been is {3:C2}, close is {4}. The current open price of {5} in US is {6:C2}, highest it has been" +
                    "is {7:C2}, lower it has been is {8:C2}, close is {9}. Market Volume is {10}, and Market Cap is {11} BTC info is pulled at {12}, " +
                    "current time is {13:d} at {13:t}", latestData.GetType().Name, latestData.OpenCNY, latestData.HighCNY, latestData.LowCNY,
                    latestData.CloseCNY, latestData.GetType().Name, latestData.OpenUSD, latestData.HighUSD, latestData.LowUSD,
                    latestData.CloseUSD, latestData.Volume, latestData.MarketCap, latestData.Timestamp, DateTime.Now);
                finalOutputString = outputString;

                foreach (BitCoin bit in GetBTCInfo)
                {
                    string historicalString = String.Format("" +
                        "CNY Open: {1:C2} \r\n " +
                        "CNY Highest: {2:C2} \r\n " +
                        "CNY Lowest: {3:C2} \r\n " +
                        "CNY Close: {4} \r\n " +
                        "USD Open: {5:C2} \r\n " +
                        "USD Highest: {6:C2} \r\n " +
                        "USD Lowest: {7:C2} \r\n " +
                        "USD Close: {8} \r\n " +
                        "Market Volume: {9} \r\n " +
                        "Market Cap: {10} \r\n " +
                        "BTC info is pulled at:  {11}, \r\n  " +
                        "Current time: {12:d} at {12:t}", bit.GetType().Name, bit.OpenCNY,
                        bit.HighCNY, bit.LowCNY, bit.CloseCNY, latestData.OpenUSD, latestData.HighUSD, latestData.LowUSD,
                    latestData.CloseUSD, latestData.Volume, latestData.MarketCap, bit.Timestamp, DateTime.Now);

                    finalOutputString += "\r\n" + "\r\n" + historicalString;
                }

                resultBox.AppendText(finalOutputString);
                stockPriceBox.Text = latestData.OpenUSD.ToString();
            }
            catch (APICallLimitReachedException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (Exception Eex)
            {
                MessageBox.Show(Eex.Message);
            }
        }


        private void addCheckingAcctButton_Click(object sender, EventArgs e)
        {
            try
            {
                int accNum = int.Parse(checkAcctNumBox.Text);
                double balance = double.Parse(checkBalanceBox.Text);
                double fee = double.Parse(checkFeeBox.Text);

                newCustomer.OpenCheckingAccount(balance, fee, accNum);

                MessageBox.Show("Checking Account " + accNum.ToString() + " Created.");
            }
            catch (AccountManagementException accEx)
            {
                MessageBox.Show(accEx.Message);
            }
            catch
            {
                MessageBox.Show("All fields are required");
            }
        }

        private void transferButton_Click(object sender, EventArgs e)
        {
            try
            {
                double amount = double.Parse(amountPriceBox.Text);
                if (amount > 0)
                {
                    Account fromAcc = (Account)accountCombo.SelectedItem;
                    Account toAcc = (Account)account2Combo.SelectedItem;

                    if (fromAcc == null || toAcc == null)
                    {
                        throw new AccountManagementException("Customer does not have any accounts \r\n OR \r\n No account has been selected.");
                    }
                    else
                    {
                        Cash cashAmount = new Cash(amount);
                        cashAmount.TransferAsset(fromAcc, toAcc);
                        amountPriceBox.Clear();
                    }
                }
                else
                {
                    MessageBox.Show("Amount must be positive.");
                }
            }
            catch (AccountManagementException accEx)
            {
                MessageBox.Show(accEx.Message);
            }
            catch (InvalidTransferException trEx)
            {
                MessageBox.Show(trEx.Message);
            }
            catch (InsufficientFundsException fundEx)
            {
                MessageBox.Show(fundEx.Message);
            }
            catch
            {
                MessageBox.Show("All fields are required.");
            }

        }

        private void logoutButton_Click(object sender, EventArgs e)
        {
            CustomerList.Clear();
            currentSelectedCurrencyPair = CurrencyPairs.None;
            newCustomer = null;

            
            bankerGroup.Hide();
            tradeGroupBox.Hide();
            OutputGroupBox.Hide();

            Customer.Reset();
            resultBox.Clear();

            accountCombo.DataSource = null;
            account2Combo.DataSource = null;
            amountPriceBox.Clear();

            Application.Exit();
        }



    }
}

