using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using BotDemoWebhook.Web;

namespace WebApplication5
{
    /// <summary>
    /// bankSotrageInfo 的摘要说明
    /// </summary>
    public class bankSotrageInfo : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            HttpRequest request = context.Request;
            try
            {
                Stream stream = request.InputStream;
                string json = string.Empty;
                if (stream.Length != 0)
                {
                    StreamReader streamReader = new StreamReader(stream);
                    json = streamReader.ReadToEnd();
                    WriteLog("input:"+json);
                    WebhookSendData data = JsonConvert.DeserializeObject<WebhookSendData>(json);
                    string action = context.Request.QueryString["action"].ToLower();
                    string strResult = Handle(context, action, data);

                    StructApiMessages messages = new StructApiMessages {
                        messages=new List<StructApiMessage> {
                            new StructApiMessage{
                                text=strResult,
                            }
                        }
                    };

                    context.Response.StatusCode = 200;
                    context.Response.Write(JsonConvert.SerializeObject(messages));
                    WriteLog("output:" + JsonConvert.SerializeObject(messages));
                    return;
                }
                context.Response.StatusCode = 400;
                context.Response.Write("request data invaild");
            }
            catch(Exception ex)
            {
                context.Response.StatusCode = 500;
                context.Response.Write(ex.Message);
                WriteLog("exception:" + ex.Message);
            }
        }

        private string Handle(HttpContext context, string action, WebhookSendData data)
        {
            string strResult = string.Empty;
            switch (action)
            {
                case "transfer":
                    {
                        string username = data.Visitor.ssoid.ToString();
                        List<BankAccount> accounts = GetBankAccounts();

                        string fromAccountType = data.FormValues.FirstOrDefault(a => a.label.Equals("from account", StringComparison.InvariantCultureIgnoreCase))?.value;//转出账户类型
                        //string toAccountType = data.FormValues.FirstOrDefault(a => a.label.Equals("to account type", StringComparison.InvariantCultureIgnoreCase))?.value;//转入账户类型
                        string amount = data.FormValues.FirstOrDefault(a => a.label.Equals("amount", StringComparison.InvariantCultureIgnoreCase))?.value;//金额
                        //string payee = data.FormValues.FirstOrDefault(a => a.label.Equals("payee", StringComparison.InvariantCultureIgnoreCase))?.value;//收款人

                        BankAccount fromBankAccount = accounts.FirstOrDefault(a => a.Username.Equals(username, StringComparison.InvariantCultureIgnoreCase));
                        //BankAccount toBankAccount = accounts.FirstOrDefault(a => a.Username.Equals(payee, StringComparison.InvariantCultureIgnoreCase));

                        if (fromBankAccount != null /*&& toBankAccount != null*/)
                        {
                            if (fromAccountType.Equals("Savings", StringComparison.InvariantCultureIgnoreCase))
                            {
                                fromBankAccount.Savings = (decimal.Parse(fromBankAccount.Savings) - decimal.Parse(amount)).ToString("0.00");
                            }
                            else
                            {
                                fromBankAccount.Checking = (decimal.Parse(fromBankAccount.Checking) - decimal.Parse(amount)).ToString("0.00");
                            }

                            //if (toAccountType.Equals("Savings", StringComparison.InvariantCultureIgnoreCase))
                            //{
                            //    toBankAccount.Savings = (decimal.Parse(toBankAccount.Savings) + decimal.Parse(amount)).ToString();
                            //}
                            //else
                            //{
                            //    toBankAccount.Checking = (decimal.Parse(toBankAccount.Checking) + decimal.Parse(amount)).ToString();
                            //}

                            SaveBankAccounts(accounts);
                        }

                        //strResult = "Placeholder text";
                        strResult = string.Format("${0} dollars transferred out your {1} account. Your new checking balance is: ${2}, your new savings balance is ${3}",
                  amount, fromAccountType, fromBankAccount.Checking, fromBankAccount.Savings);

                        break;
                    }
                case "transferbetweensavingsandchecking":
                    {
                        string username = data.Visitor.ssoid.ToString();
                        List<BankAccount> accounts = GetBankAccounts();

                        string fromAccountType = data.FormValues.FirstOrDefault(a => a.label.Equals("from account", StringComparison.InvariantCultureIgnoreCase))?.value;//转出账户类型
                        string toAccountType = data.FormValues.FirstOrDefault(a => a.label.Equals("to account", StringComparison.InvariantCultureIgnoreCase))?.value;//转入账户类型
                        string amount = data.FormValues.FirstOrDefault(a => a.label.Equals("amount", StringComparison.InvariantCultureIgnoreCase))?.value;//金额
                        
                        BankAccount fromBankAccount = accounts.FirstOrDefault(a => a.Username.Equals(username, StringComparison.InvariantCultureIgnoreCase));
                        BankAccount toBankAccount = accounts.FirstOrDefault(a => a.Username.Equals(username, StringComparison.InvariantCultureIgnoreCase));

                        if (fromBankAccount != null && toBankAccount != null)
                        {
                            if (fromAccountType.Equals("Savings", StringComparison.InvariantCultureIgnoreCase))
                            {
                                fromBankAccount.Savings = (decimal.Parse(fromBankAccount.Savings) - decimal.Parse(amount)).ToString("0.00");
                            }
                            else
                            {
                                fromBankAccount.Checking = (decimal.Parse(fromBankAccount.Checking) - decimal.Parse(amount)).ToString("0.00");
                            }

                            if (toAccountType.Equals("Savings", StringComparison.InvariantCultureIgnoreCase))
                            {
                                toBankAccount.Savings = (decimal.Parse(toBankAccount.Savings) + decimal.Parse(amount)).ToString("0.00");
                            }
                            else
                            {
                                toBankAccount.Checking = (decimal.Parse(toBankAccount.Checking) + decimal.Parse(amount)).ToString("0.00");
                            }

                            SaveBankAccounts(accounts);
                        }

                        strResult = string.Format("${0} dollars transferred to your {1} account. Your new checking balance is: ${2}, your new savings balance is ${3}",
                            amount, toAccountType, toBankAccount.Checking, toBankAccount.Savings);

                        break;
                    }
                case "confirmaddress":
                    {
                        string username = data.Visitor.ssoid.ToString();
                        List<BankAccount> accounts = GetBankAccounts();
                        
                        BankAccount fromBankAccount = accounts.FirstOrDefault(a => a.Username.Equals(username, StringComparison.InvariantCultureIgnoreCase));
                        
                        if (fromBankAccount != null)
                        {
                        }

                        strResult = string.Format("The address we have on file for you is {0}. Would you like to change it?", fromBankAccount?.Address);

                        break;
                    }
                case "confirmphone":
                    {
                        string username = data.Visitor.ssoid.ToString();
                        List<BankAccount> accounts = GetBankAccounts();

                        BankAccount fromBankAccount = accounts.FirstOrDefault(a => a.Username.Equals(username, StringComparison.InvariantCultureIgnoreCase));

                        if (fromBankAccount != null)
                        {
                        }

                        strResult = string.Format("The phone we have on file for you is {0}. Would you like to change it?", fromBankAccount?.Phone);

                        break;
                    }
                case "savingsbalance":
                    {
                        string username = data.Visitor.ssoid.ToString();
                        List<BankAccount> accounts = GetBankAccounts();

                        string fromAccountType = data.FormValues.FirstOrDefault(a => a.label.Equals("account", StringComparison.InvariantCultureIgnoreCase))?.value;//账户类型

                        BankAccount fromBankAccount = accounts.FirstOrDefault(a => a.Username.Equals(username, StringComparison.InvariantCultureIgnoreCase));
                        string balance = fromBankAccount?.Savings;
                        if (fromBankAccount != null)
                        {
                            if (fromAccountType.Equals("Savings", StringComparison.InvariantCultureIgnoreCase))
                            {
                                balance = fromBankAccount?.Savings;
                            }
                            else
                            {
                                balance = fromBankAccount?.Checking;
                            }
                        }

                        strResult = string.Format("Your balance is ${0}",decimal.Parse( balance).ToString("0.00"));

                        break;
                    }
                case "accountnumber":
                    {
                        string username = data.Visitor.ssoid.ToString();
                        List<BankAccount> accounts = GetBankAccounts();

                        BankAccount fromBankAccount = accounts.FirstOrDefault(a => a.Username.Equals(username, StringComparison.InvariantCultureIgnoreCase));

                        if (fromBankAccount != null)
                        {
                        }

                        strResult = string.Format("Your account number is {0}", fromBankAccount?.AccountNumber);

                        break;
                    }
                case "accountusername":
                    {
                        string username = data.Visitor.ssoid.ToString();
                        List<BankAccount> accounts = GetBankAccounts();

                        BankAccount fromBankAccount = accounts.FirstOrDefault(a => a.Username.Equals(username, StringComparison.InvariantCultureIgnoreCase));

                        if (fromBankAccount != null)
                        {
                        }

                        strResult = string.Format("Your account username is {0}", fromBankAccount?.Username);

                        break;
                    }
                case "branchnumber":
                    {
                        string username = data.Visitor.ssoid.ToString();
                        List<BankAccount> accounts = GetBankAccounts();

                        BankAccount fromBankAccount = accounts.FirstOrDefault(a => a.Username.Equals(username, StringComparison.InvariantCultureIgnoreCase));

                        if (fromBankAccount != null)
                        {
                        }

                        strResult = string.Format("your branch number is {0}", fromBankAccount?.BranchNumber);

                        break;
                    }
                case "transactionlimits":
                    {
                        string username = data.Visitor.ssoid.ToString();
                        List<BankAccount> accounts = GetBankAccounts();

                        BankAccount fromBankAccount = accounts.FirstOrDefault(a => a.Username.Equals(username, StringComparison.InvariantCultureIgnoreCase));

                        if (fromBankAccount != null)
                        {
                        }

                        strResult = string.Format("Your daily transaction limit is {0}", fromBankAccount?.DailyTransactionLimit);

                        break;
                    }
                case "routingnumber":
                    {
                        string username = data.Visitor.ssoid.ToString();
                        List<BankAccount> accounts = GetBankAccounts();

                        BankAccount fromBankAccount = accounts.FirstOrDefault(a => a.Username.Equals(username, StringComparison.InvariantCultureIgnoreCase));

                        if (fromBankAccount != null)
                        {
                        }

                        strResult = string.Format("Your Routing Number is {0}", fromBankAccount?.RoutingNumber);

                        break;
                    }
                case "transitnumber":
                    {
                        string username = data.Visitor.ssoid.ToString();
                        List<BankAccount> accounts = GetBankAccounts();

                        BankAccount fromBankAccount = accounts.FirstOrDefault(a => a.Username.Equals(username, StringComparison.InvariantCultureIgnoreCase));

                        if (fromBankAccount != null)
                        {
                        }

                        strResult = string.Format("Your transit number is {0}", fromBankAccount?.TransitNumber);

                        break;
                    }
                case "holdpolicy":
                    {
                        string username = data.Visitor.ssoid.ToString();
                        List<BankAccount> accounts = GetBankAccounts();

                        BankAccount fromBankAccount = accounts.FirstOrDefault(a => a.Username.Equals(username, StringComparison.InvariantCultureIgnoreCase));

                        if (fromBankAccount != null)
                        {
                        }

                        strResult = string.Format("The hold policy on your deposits is {0}", fromBankAccount?.HoldPolicy);

                        break;
                    }
                case "overdraftpolicy":
                    {
                        string username = data.Visitor.ssoid.ToString();
                        List<BankAccount> accounts = GetBankAccounts();

                        BankAccount fromBankAccount = accounts.FirstOrDefault(a => a.Username.Equals(username, StringComparison.InvariantCultureIgnoreCase));

                        if (fromBankAccount != null)
                        {
                        }

                        strResult = string.Format("The overdraft policy on your account is {0}", fromBankAccount?.OverdraftPolicy);

                        break;
                    }
                case "changeaddress":
                    {
                        string username = data.Visitor.ssoid.ToString();
                        List<BankAccount> accounts = GetBankAccounts();

                        BankAccount fromBankAccount = accounts.FirstOrDefault(a => a.Username.Equals(username, StringComparison.InvariantCultureIgnoreCase));
                        string address = data.FormValues.FirstOrDefault(a => a.label.Equals("sys-street-address", StringComparison.InvariantCultureIgnoreCase))?.value;//新地址

                        if (fromBankAccount != null)
                        {
                            fromBankAccount.Address = address;
                            SaveBankAccounts(accounts);
                        }

                        strResult = string.Format("your new address on file is {0}", address);

                        break;
                    }
                case "changephone":
                    {
                        string username = data.Visitor.ssoid.ToString();
                        List<BankAccount> accounts = GetBankAccounts();

                        BankAccount fromBankAccount = accounts.FirstOrDefault(a => a.Username.Equals(username, StringComparison.InvariantCultureIgnoreCase));
                        string phone = data.FormValues.FirstOrDefault(a => a.label.Equals("Your new phone", StringComparison.InvariantCultureIgnoreCase))?.value;//新电话

                        if (fromBankAccount != null)
                        {
                            fromBankAccount.Phone = phone;
                            SaveBankAccounts(accounts);
                        }

                        strResult = string.Format("your new phone number on file is {0}", phone);

                        break;
                    }
                case "changepassword":
                    {
                        string username = data.Visitor.ssoid.ToString();
                        List<BankAccount> accounts = GetBankAccounts();

                        BankAccount fromBankAccount = accounts.FirstOrDefault(a => a.Username.Equals(username, StringComparison.InvariantCultureIgnoreCase));
                        string accountNumber = data.FormValues.FirstOrDefault(a => a.label.Equals("Account number", StringComparison.InvariantCultureIgnoreCase))?.value;
                        string lastName = data.FormValues.FirstOrDefault(a => a.label.Equals("Last name", StringComparison.InvariantCultureIgnoreCase))?.value;
                        string zipCode = data.FormValues.FirstOrDefault(a => a.label.Equals("Zip code", StringComparison.InvariantCultureIgnoreCase))?.value;
                        string emailAddress = data.FormValues.FirstOrDefault(a => a.label.Equals("Email address", StringComparison.InvariantCultureIgnoreCase))?.value;
                        string securityQuestion1 = data.FormValues.FirstOrDefault(a => a.label.Equals("Security question #1", StringComparison.InvariantCultureIgnoreCase))?.value;
                        string securityQuestion2 = data.FormValues.FirstOrDefault(a => a.label.Equals("Security question #2", StringComparison.InvariantCultureIgnoreCase))?.value;
                        string password = data.FormValues.FirstOrDefault(a => a.label.Equals("New password", StringComparison.InvariantCultureIgnoreCase))?.value;

                        if (fromBankAccount != null)
                        {
                            if (fromBankAccount.SecurityQuestion2 != securityQuestion2)
                            {
                                strResult = "Your security question #2 is wrong, please try again.";
                            }
                            if (fromBankAccount.SecurityQuestion1 != securityQuestion1)
                            {
                                strResult = "Your security question #1 is wrong, please try again.";
                            }                           
                            if (fromBankAccount.AccountNumber != accountNumber)
                            {
                                strResult = "Your account number is wrong, please try again.";
                            }

                            if (string.IsNullOrWhiteSpace(strResult))
                            {
                                fromBankAccount.Password = password;
                                SaveBankAccounts(accounts);
                                strResult = string.Format("You password has been changed");
                            }                      
                        }                        

                        break;
                    }
                case "atmnearest":
                    {
                        string username = data.Visitor.ssoid.ToString();

                        List<dynamic> atms = new List<dynamic> {
                            new {
                                latitude=47.23965880000002,
                                longitude=-122.40253050000001,
                                response="The nearest FCB ATM is located at: 2024 E 29th St, Tacoma, WA 98404",
                            },
                            new {
                                latitude=48.4718561,
                                longitude=-122.41023940000002,
                                response="The nearest ATM location to you is: 15400 Airport Dr, Burlington, WA 98233",
                            },
                            new {
                                latitude=47.666490399999994,
                                longitude=-117.40244949999999,
                                response="The nearest ATM location to you is: 502 E Boone Ave, Spokane, WA 99258",
                            },
                        };

                        double latitude = data.Visitor.latitude;
                        double longitude = data.Visitor.longitude;

                        double first = GetDistance(latitude, longitude, atms[0].latitude, atms[0].longitude);
                        double second = GetDistance(latitude, longitude, atms[1].latitude, atms[1].longitude);
                        double third = GetDistance(latitude, longitude, atms[2].latitude, atms[2].longitude);

                        int minIndex = 0;
                        double min = first;
                        if (first > second)
                        {
                            min = second;
                            minIndex = 1;
                        }
                        if (min > third)
                        {
                            min = third;
                            minIndex = 2;
                        }
                        string response = atms[minIndex].response;
                        strResult = string.Format(response);

                        break;
                    }
                    case "branchnearest":
                    {
                        string username = data.Visitor.ssoid.ToString();

                        List<dynamic> atms = new List<dynamic> {
                            new {
                                latitude=47.654332700000005,
                                longitude=-117.4293462,
                                response="The nearest branch to you is: 1111 W 3rd Ave, Spokane, WA 99201",
                            },
                            new {
                                latitude=47.5969724,
                                longitude=-122.32901979999997,
                                response="The nearest FCB branch to you is: 800 Occidental Ave S, Seattle, WA 98134",
                            },
                            new {
                                latitude=48.76304990000005,
                                longitude=-122.463887,
                                response="The nearest branch to you is: 2410 James St, Bellingham, WA 98225",
                            },
                        };

                        double latitude = data.Visitor.latitude;
                        double longitude = data.Visitor.longitude;

                        double first = GetDistance(latitude, longitude, atms[0].latitude, atms[0].longitude);
                        double second = GetDistance(latitude, longitude, atms[1].latitude, atms[1].longitude);
                        double third = GetDistance(latitude, longitude, atms[2].latitude, atms[2].longitude);

                        int minIndex = 0;
                        double min = first;
                        if (first > second)
                        {
                            min = second;
                            minIndex = 1;
                        }
                        if (min > third)
                        {
                            min = third;
                            minIndex = 2;
                        }
                        string response = atms[minIndex].response;
                        strResult = string.Format(response);

                        break;
                    }
            }
            return strResult;
        }
        private List<BankAccount> GetBankAccounts()
        {
            var AccountData = File.ReadAllText(HttpContext.Current.Server.MapPath("~/BankStorageInfo.json"), Encoding.UTF8);
            List<BankAccount> accounts = new List<BankAccount>();
            if (!string.IsNullOrWhiteSpace(AccountData))
            {
                accounts = JsonConvert.DeserializeObject<List<BankAccount>>(AccountData);
            }
            return accounts;
        }
        private void SaveBankAccounts(List<BankAccount> accounts)
        {
            string json = JsonConvert.SerializeObject(accounts);
            File.WriteAllText(HttpContext.Current.Server.MapPath("~/BankStorageInfo.json"), json, Encoding.UTF8);
        }
        private void WriteLog(string log)
        {
            File.AppendAllText(HttpContext.Current.Server.MapPath("~/log.txt"), log, Encoding.UTF8);
        }
        //地球半径，单位米
        private const double EARTH_RADIUS = 6378137;
        /// <summary>
        /// 计算两点位置的距离，返回两点的距离，单位 米
        /// 该公式为GOOGLE提供，误差小于0.2米
        /// </summary>
        /// <param name="lat1">第一点纬度</param>
        /// <param name="lng1">第一点经度</param>
        /// <param name="lat2">第二点纬度</param>
        /// <param name="lng2">第二点经度</param>
        /// <returns></returns>
        public static double GetDistance(double lat1, double lng1, double lat2, double lng2)
        {
            double radLat1 = Rad(lat1);
            double radLng1 = Rad(lng1);
            double radLat2 = Rad(lat2);
            double radLng2 = Rad(lng2);
            double a = radLat1 - radLat2;
            double b = radLng1 - radLng2;
            double result = 2 * Math.Asin(Math.Sqrt(Math.Pow(Math.Sin(a / 2), 2) + Math.Cos(radLat1) * Math.Cos(radLat2) * Math.Pow(Math.Sin(b / 2), 2))) * EARTH_RADIUS;
            return result;
        }

        /// <summary>
        /// 经纬度转化成弧度
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        private static double Rad(double d)
        {
            return (double)d * Math.PI / 180d;
        }
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}