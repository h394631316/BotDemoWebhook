using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BotDemoWebhook.Web
{
    public class BankAccount
    {
        public string Savings { get; set; }
        public string Checking { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string SecurityQuestion1 { get; set; }
        public string SecurityQuestion2 { get; set; }
        public string DailyTransactionLimit { get; set; }
        public string AccountNumber { get; set; }
        public string Username { get; set; }
        public string AccountType { get; set; }
        public string BranchNumber { get; set; }
        public string RoutingNumber { get; set; }
        public string TransitNumber { get; set; }
        public string HoldPolicy { get; set; }
        public string OverdraftPolicy { get; set; }
        public string Password { get; set; }
    }
}