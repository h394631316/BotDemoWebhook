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
    /// HappyPizzaBookService 的摘要说明
    /// </summary>
    public class HappyPizzaBookService : IHttpHandler
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
                case "book":
                    {
                        Random random = new Random(Guid.NewGuid().GetHashCode());
                        List<HappyPizzaBook> accounts = GetBankAccounts();
                        
                        string flavor = data.FormValues.FirstOrDefault(a => a.label.Equals("flavor", StringComparison.InvariantCultureIgnoreCase))?.value;
                        string size = data.FormValues.FirstOrDefault(a => a.label.Equals("size", StringComparison.InvariantCultureIgnoreCase))?.value;
                        string type = data.FormValues.FirstOrDefault(a => a.label.Equals("type", StringComparison.InvariantCultureIgnoreCase))?.value;
                        string payment = data.FormValues.FirstOrDefault(a => a.label.Equals("payment", StringComparison.InvariantCultureIgnoreCase))?.value;
                        //string orderNo =  Guid.NewGuid().ToString("N").Substring(0, 6);
                        string orderNo = random.Next(100000, 999999).ToString();

                        accounts.Add(new HappyPizzaBook
                        {
                            orderNo = orderNo,
                            flavor = flavor,
                            size = size,
                            type = type,
                            payment = payment,
                        });

                        SaveBankAccounts(accounts);

                        //var scheme = context.Request.Url.Scheme;
                        //var host = context.Request.Url.Host;
                        //var port = context.Request.Url.Port; //{scheme}://{host}:{port}

                        var hostUrl = context.Request.Url.ToString().Substring(0,context.Request.Url.ToString().IndexOf("HappyPizzaBookService"));
                        string url = $"{hostUrl}DonorDemo/index3.html?orderNo={orderNo}";

                        strResult = @"[{
  'type': 'text',
  'content': {
    'message': 'Brilliant! Just click this link to enter in your payment details and place your order.',
    'links': [
      {
        'type': 'hyperlink',
        'startPosition': 16,
        'endPosition': 21,
        'ifPushPage': true,
        'url': '"+ url + @"',
        'openIn': 'sideWindow'
      }
    ]
  }
}]";

                        break;
                    }
                case "query":
                    {
                        List<HappyPizzaBook> accounts = GetBankAccounts();

                        string orderNo = data.FormValues.FirstOrDefault(a => a.label.Equals("orderNo", StringComparison.InvariantCultureIgnoreCase))?.value;

                        HappyPizzaBook happyPizzaBook = accounts.FirstOrDefault(a => a.orderNo == orderNo);

                        if (happyPizzaBook != null)
                        {
                            strResult = @"[{
  'type': 'text',
  'content': {
    'message': 'I can see your order is out for delivery now and your delivery person will be in touch shortly if there are any problems.'
  }
}]";
                        }
                        else
                        {
                            strResult = @"[{
  'type': 'text',
  'content': {
    'message': 'Sorry, the order does not exist.'
  }
}]";
                        }

                        break;
                    }
                case "savepayment":
                    {
                        List<HappyPizzaBook> accounts = GetBankAccounts();

                        string orderNo = data.FormValues.FirstOrDefault(a => a.label.Equals("orderNo", StringComparison.InvariantCultureIgnoreCase))?.value;
                        string payment = data.FormValues.FirstOrDefault(a => a.label.Equals("payment", StringComparison.InvariantCultureIgnoreCase))?.value;

                        HappyPizzaBook happyPizzaBook = accounts.FirstOrDefault(a => a.orderNo == orderNo);
                        happyPizzaBook.payment = payment;
                        
                        SaveBankAccounts(accounts);

                        break;
                    }
            }
            return strResult;
        }
        private List<HappyPizzaBook> GetBankAccounts()
        {

            List<HappyPizzaBook> accounts = new List<HappyPizzaBook>();

            string path = HttpContext.Current.Server.MapPath("~/HappyPizzaStorageInfo.json");
            if (!File.Exists(path))
            {
                using(var file = File.Create(path))
                {

                }
            }

            var AccountData = File.ReadAllText(path, Encoding.UTF8);
            if (!string.IsNullOrWhiteSpace(AccountData))
            {
                accounts = JsonConvert.DeserializeObject<List<HappyPizzaBook>>(AccountData);
            }

            return accounts;
        }
        private void SaveBankAccounts(List<HappyPizzaBook> accounts)
        {
            string json = JsonConvert.SerializeObject(accounts);
            File.WriteAllText(HttpContext.Current.Server.MapPath("~/HappyPizzaStorageInfo.json"), json, Encoding.UTF8);
        }
        private void WriteLog(string log)
        {
            File.AppendAllText(HttpContext.Current.Server.MapPath("~/log.txt"), DateTime.Now.ToString() +"\n" + log + "\n", Encoding.UTF8);
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
    public class HappyPizzaBook
    {
        public string orderNo { get; set; }
        public string flavor { get; set; }
        public string size { get; set; }
        public string type { get; set; }
        public string payment { get; set; }
    }
}