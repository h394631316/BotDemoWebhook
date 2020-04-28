using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BotDemoWebhook.Web
{
    public partial class BankStorageInfo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                initData();
            }
        }

        private void initData()
        {
            var AccountData = File.ReadAllText(Server.MapPath("~/BankStorageInfo.json"), Encoding.UTF8);
            List<BankAccount> accounts = new List<BankAccount>();
            if (!string.IsNullOrWhiteSpace(AccountData))
            {
                accounts = JsonConvert.DeserializeObject<List<BankAccount>>(AccountData);
            }

            var account1 = accounts[0];
            var account2 = accounts[1];
            var account3 = accounts[2];

            txtSavings1.Text = account1.Savings;
            txtSavings2.Text = account2.Savings;
            txtSavings3.Text = account3.Savings;

            txtChecking1.Text = account1.Checking;
            txtChecking2.Text = account2.Checking;
            txtChecking3.Text = account3.Checking;

            txtAddress1.Text = account1.Address;
            txtAddress2.Text = account2.Address;
            txtAddress3.Text = account3.Address;

            txtPhone1.Text = account1.Phone;
            txtPhone2.Text = account2.Phone;
            txtPhone3.Text = account3.Phone;

            txtSecurityQuestion11.Text = account1.SecurityQuestion1;
            txtSecurityQuestion12.Text = account2.SecurityQuestion1;
            txtSecurityQuestion13.Text = account3.SecurityQuestion1;

            txtSecurityQuestion21.Text = account1.SecurityQuestion2;
            txtSecurityQuestion22.Text = account2.SecurityQuestion2;
            txtSecurityQuestion23.Text = account3.SecurityQuestion2;

            txtDailyTransactionLimit1.Text = account1.DailyTransactionLimit;
            txtDailyTransactionLimit2.Text = account2.DailyTransactionLimit;
            txtDailyTransactionLimit3.Text = account3.DailyTransactionLimit;

            txtAccountNumber1.Text = account1.AccountNumber;
            txtAccountNumber2.Text = account2.AccountNumber;
            txtAccountNumber3.Text = account3.AccountNumber;

            txtUsername1.Text = account1.Username;
            txtUsername2.Text = account2.Username;
            txtUsername3.Text = account3.Username;

            txtAccountType1.Text = account1.AccountType;
            txtAccountType2.Text = account2.AccountType;
            txtAccountType3.Text = account3.AccountType;

            txtBranchNumber1.Text = account1.BranchNumber;
            txtBranchNumber2.Text = account2.BranchNumber;
            txtBranchNumber3.Text = account3.BranchNumber;

            txtRoutingNumber1.Text = account1.RoutingNumber;
            txtRoutingNumber2.Text = account2.RoutingNumber;
            txtRoutingNumber3.Text = account3.RoutingNumber;

            txtTransitNumber1.Text = account1.TransitNumber;
            txtTransitNumber2.Text = account2.TransitNumber;
            txtTransitNumber3.Text = account3.TransitNumber;

            txtHoldPolicy1.Text = account1.HoldPolicy;
            txtHoldPolicy2.Text = account2.HoldPolicy;
            txtHoldPolicy3.Text = account3.HoldPolicy;

            txtOverdraftPolicy1.Text = account1.OverdraftPolicy;
            txtOverdraftPolicy2.Text = account2.OverdraftPolicy;
            txtOverdraftPolicy3.Text = account3.OverdraftPolicy;

            txtPassword1.Text = account1.Password;
            txtPassword2.Text = account2.Password;
            txtPassword3.Text = account3.Password;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            List<BankAccount> accounts = new List<BankAccount>();
            accounts.Add(new BankAccount
            {
                Savings= txtSavings1.Text,
                Checking=txtChecking1.Text,
                Address = txtAddress1.Text,
                Phone=txtPhone1.Text,
                SecurityQuestion1=txtSecurityQuestion11.Text,
                SecurityQuestion2=txtSecurityQuestion21.Text,
                DailyTransactionLimit=txtDailyTransactionLimit1.Text,
                AccountNumber=txtAccountNumber1.Text,
                Username=txtUsername1.Text,
                AccountType=txtAccountType1.Text,
                BranchNumber=txtBranchNumber1.Text,
                RoutingNumber=txtRoutingNumber1.Text,
                TransitNumber=txtTransitNumber1.Text,
                HoldPolicy=txtHoldPolicy1.Text,
                OverdraftPolicy=txtOverdraftPolicy1.Text,
                Password=txtPassword1.Text,
            });
            accounts.Add(new BankAccount
            {
                Savings = txtSavings2.Text,
                Checking = txtChecking2.Text,
                Address = txtAddress2.Text,
                Phone = txtPhone2.Text,
                SecurityQuestion1 = txtSecurityQuestion12.Text,
                SecurityQuestion2 = txtSecurityQuestion22.Text,
                DailyTransactionLimit = txtDailyTransactionLimit2.Text,
                AccountNumber = txtAccountNumber2.Text,
                Username = txtUsername2.Text,
                AccountType = txtAccountType2.Text,
                BranchNumber = txtBranchNumber2.Text,
                RoutingNumber = txtRoutingNumber2.Text,
                TransitNumber = txtTransitNumber2.Text,
                HoldPolicy = txtHoldPolicy2.Text,
                OverdraftPolicy = txtOverdraftPolicy2.Text,
                Password = txtPassword2.Text,
            });
            accounts.Add(new BankAccount
            {
                Savings = txtSavings3.Text,
                Checking = txtChecking3.Text,
                Address = txtAddress3.Text,
                Phone = txtPhone3.Text,
                SecurityQuestion1 = txtSecurityQuestion13.Text,
                SecurityQuestion2 = txtSecurityQuestion23.Text,
                DailyTransactionLimit = txtDailyTransactionLimit3.Text,
                AccountNumber = txtAccountNumber3.Text,
                Username = txtUsername3.Text,
                AccountType = txtAccountType3.Text,
                BranchNumber = txtBranchNumber3.Text,
                RoutingNumber = txtRoutingNumber3.Text,
                TransitNumber = txtTransitNumber3.Text,
                HoldPolicy = txtHoldPolicy3.Text,
                OverdraftPolicy = txtOverdraftPolicy3.Text,
                Password = txtPassword3.Text,
            });
            string json = JsonConvert.SerializeObject(accounts);
            File.WriteAllText(Server.MapPath("~/BankStorageInfo.json"), json, Encoding.UTF8);
        }
    }
}