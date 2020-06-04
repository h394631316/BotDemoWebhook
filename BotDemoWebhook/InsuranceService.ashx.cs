using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using BotDemoWebhook.Web;

namespace BotDemoWebhook.Web
{
    /// <summary>
    /// InsuranceService 的摘要说明
    /// </summary>
    public class InsuranceService : IHttpHandler
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

                    //StructApiMessages messages = new StructApiMessages {
                    //    messages=new List<StructApiMessage> {
                    //        new StructApiMessage{
                    //            text=strResult,
                    //        }
                    //    }
                    //};

                    context.Response.StatusCode = 200;
                    context.Response.Write(JsonConvert.DeserializeObject<dynamic>(strResult));
                    WriteLog("\n output:" + JsonConvert.SerializeObject(strResult));
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
                case "claim":
                    {
                        List<InsuranceAccount> accounts = GetAccounts();
                        
                        string number = data.FormValues.FirstOrDefault(a => a.label.Equals("Claim number", StringComparison.InvariantCultureIgnoreCase))?.value;
                        string name = data.FormValues.FirstOrDefault(a => a.label.Equals("Name", StringComparison.InvariantCultureIgnoreCase))?.value;
                        string birth = data.FormValues.FirstOrDefault(a => a.label.Equals("Date of birth", StringComparison.InvariantCultureIgnoreCase))?.value;

                        var account = accounts.FirstOrDefault(a => a.number == number && a.name == name && a.birth == birth);

                        string stringResult = "";
                        if (!accounts.Exists(a => a.number == number))
                        {
                            stringResult = "I am sorry, your claim number does not exist.";
                        }
                        else
                        {
                            if (!accounts.Exists(a => a.number == number && a.name == name && a.birth == birth))
                            {
                                stringResult = "Sorry, you have entered the wrong name or birthday.";
                            }
                        }

                        if (account != null)
                        {
                            stringResult = $"I can confirm your claim will be paid on the 15th April. Your claim amount is {account.amount}.";
                        }
                       
                        strResult = @"[{
  'type': 'text',
  'content': {
    'message': '" + stringResult + @"'
  }
}]";

                        break;
                    }
                case "covered":
                    {
                        List<InsuranceAccount> accounts = GetAccounts();

                        string number = data.FormValues.FirstOrDefault(a => a.label.Equals("Plan ID", StringComparison.InvariantCultureIgnoreCase))?.value;
                        string organization = data.FormValues.FirstOrDefault(a => a.label.Equals("Name of organization", StringComparison.InvariantCultureIgnoreCase))?.value;
                       
                        var account = accounts.FirstOrDefault(a => a.number == number && a.organisation == organization);
                        
                        string stringResult = "";
                        if (!accounts.Exists(a => a.number == number))
                        {
                            stringResult = "Sorry, your plan ID does not exist.";
                        }
                        else
                        {
                            if (!accounts.Exists(a => a.number == number && a.organisation == organization))
                            {
                                stringResult = "Sorry, you provided the wrong organization name.";
                            }
                        }

                        if (account != null)
                        {
                            stringResult = $"You will be covered 80% up to a total of {account.amountcovered}.";
                        }

                        strResult = @"[{
  'type': 'text',
  'content': {
    'message': '" + stringResult + @"'
  }
}]";

                        break;
                    }
            }
            return strResult;
        }
        private List<InsuranceAccount> GetAccounts()
        {

            List<InsuranceAccount> accounts = new List<InsuranceAccount>();

            string path = HttpContext.Current.Server.MapPath("~/InsuranceAccountStorageInfo.json");
            if (!File.Exists(path))
            {
                using (var file = File.Create(path))
                {//初始化数据
                     accounts = new List<InsuranceAccount> {
                        new InsuranceAccount{
                            number = "2846756",
                            name = "Steve Jones",
                            birth = "15/03/1983",
                            amount = "$656.98",

                            organisation="Comm100",
                            amountcovered="$25,000",
                        },
                        new InsuranceAccount{
                            number = "2846757",
                            name = "Jack Jones",
                            birth = "15/04/1984", 
                            amount = "$756.98",

                            organisation="Comm100",
                            amountcovered="$26,000",
                        },
                        new InsuranceAccount{
                            number = "2846758",
                            name = "Jim Jones",
                            birth = "15/05/1985",
                            amount = "$856.98",

                            organisation="Comm100",
                            amountcovered="$27,000",
                        },
                    };
                    string text = JsonConvert.SerializeObject(accounts);
                    byte[] buff = Encoding.UTF8.GetBytes(text);
                    file.Write(buff, 0, buff.Length);
                }
            }

            var AccountData = File.ReadAllText(path, Encoding.UTF8);
            if (!string.IsNullOrWhiteSpace(AccountData))
            {
                accounts = JsonConvert.DeserializeObject<List<InsuranceAccount>>(AccountData);
            }

            return accounts;
        }
        private void SaveAccounts(List<InsuranceAccount> accounts)
        {
            string json = JsonConvert.SerializeObject(accounts);
            File.WriteAllText(HttpContext.Current.Server.MapPath("~/InsuranceAccountStorageInfo.json"), json, Encoding.UTF8);
        }
        private void WriteLog(string log)
        {
            File.AppendAllText(HttpContext.Current.Server.MapPath("~/logInsurance.txt"), DateTime.Now.ToString() +"\n" + log + "\n", Encoding.UTF8);
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
    public class InsuranceAccount
    {
        public string number { get; set; }
        public string name { get; set; }
        public string birth { get; set; }
        public string amount { get; set; }

        public string organisation { get; set; }
        public string amountcovered { get; set; }
    }
}