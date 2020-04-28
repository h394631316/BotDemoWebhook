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
    /// LoginService 的摘要说明
    /// </summary>
    public class LoginService : IHttpHandler
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
                case "login":
                    {
                        Random random = new Random(Guid.NewGuid().GetHashCode());
                        var accounts = GetLoginAccounts();
                        
                        string name = data.FormValues.FirstOrDefault(a => a.label.Equals("email", StringComparison.InvariantCultureIgnoreCase))?.value;
                        string password = data.FormValues.FirstOrDefault(a => a.label.Equals("password", StringComparison.InvariantCultureIgnoreCase))?.value;

                        //Name or password is wrong. /Login succeeded.
                        string stringResult = "Your email or password is wrong. Login failed.";
                        if(accounts.Exists(f=>f.Name == name && f.Password == password))
                        {
                            stringResult = "Login succeeded.";
                        }

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
        private List<LoginAccount> GetLoginAccounts()
        {

            List<LoginAccount> accounts = new List<LoginAccount>();

            string path = HttpContext.Current.Server.MapPath("~/LoginAccountStorageInfo.json");
            if (!File.Exists(path))
            {
                using (var file = File.Create(path))
                {//初始化数据
                    List<LoginAccount> loginAccounts = new List<LoginAccount> {
                        new LoginAccount{
                            Name = "tom@comm100.demo",
                            Password = "123456",
                        },
                        new LoginAccount{
                            Name = "jim@comm100.demo",
                            Password = "123456",
                        },
                        new LoginAccount{
                            Name = "jack@comm100.demo",
                            Password = "123456",
                        },
                    };
                    string text = JsonConvert.SerializeObject(loginAccounts);
                    byte[] buff = Encoding.UTF8.GetBytes(text);
                    file.Write(buff, 0, buff.Length);
                }
            }

            var accountData = File.ReadAllText(path, Encoding.UTF8);
            if (!string.IsNullOrWhiteSpace(accountData))
            {
                accounts = JsonConvert.DeserializeObject<List<LoginAccount>>(accountData);
            }

            return accounts;
        }
        public class LoginAccount
        {
            public string Name { get; set; }
            public string Password { get; set; }
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