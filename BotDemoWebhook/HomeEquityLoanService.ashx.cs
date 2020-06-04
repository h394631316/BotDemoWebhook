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
    /// HomeEquityLoanService 的摘要说明
    /// </summary>
    public class HomeEquityLoanService : IHttpHandler
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
                case "loan":
                    {                        
                        string homeValue = data.FormValues.FirstOrDefault(a => a.label.Equals("home value", StringComparison.InvariantCultureIgnoreCase))?.value;
                        string mortgageBalance = data.FormValues.FirstOrDefault(a => a.label.Equals("mortgage balance", StringComparison.InvariantCultureIgnoreCase))?.value;
                        string primaryOrSecondaryHome = data.FormValues.FirstOrDefault(a => a.label.Equals("primary or secondary home", StringComparison.InvariantCultureIgnoreCase))?.value;
                        string zipCode = data.FormValues.FirstOrDefault(a => a.label.Equals("zip code", StringComparison.InvariantCultureIgnoreCase))?.value;

                        //loan amount primary （home value-mortgage balance）*0.75
                        //secondary （home value-mortgage balance）*0.8

                        //Thanks. Based on what you’ve entered, your maximum loan amount would be $150,000.

                        var homeValueDec = decimal.Parse(homeValue?.Replace("$", "").Replace(",", ""));
                        var mortgageBalanceDec = decimal.Parse(mortgageBalance?.Replace("$", "").Replace(",", ""));

                        var amount = (homeValueDec - mortgageBalanceDec) * 0.75m;
                        if (primaryOrSecondaryHome.Equals("secondary", StringComparison.OrdinalIgnoreCase))
                        {
                            amount = (homeValueDec - mortgageBalanceDec) * 0.8m;
                        }

                        var amountStr = string.Format("${0:N0}", amount);

                        string stringResult = $"Thanks. Based on what you’ve entered, your maximum loan amount would be {amountStr}.";
                        
                        strResult = @"[{
  'type': 'text',
  'content': {
    'message': '"+ stringResult + @"'
  }
}]";

                        break;
                    }
            }
            return strResult;
        }
        private void WriteLog(string log)
        {
            File.AppendAllText(HttpContext.Current.Server.MapPath("~/log.txt"), DateTime.Now.ToString() +"\n" + log + "\n", Encoding.UTF8);
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