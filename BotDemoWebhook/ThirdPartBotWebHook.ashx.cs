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
    /// ThirdPartBotWebHook 的摘要说明
    /// </summary>
    public class ThirdPartBotWebHook : IHttpHandler
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
                    ChatJoinedEventRequest data = JsonConvert.DeserializeObject<ChatJoinedEventRequest>(json);

                    string response = "";
                    switch (data.eventType)
                    {
                        case EnumThirdEventType.chatJoined:
                            {
                                response= File.ReadAllText(HttpContext.Current.Server.MapPath("~/chatJoined.json"), Encoding.UTF8);
                                break;
                            }
                        case EnumThirdEventType.questionAsked:
                        case EnumThirdEventType.intentClicked:
                        case EnumThirdEventType.locationShared:
                        case EnumThirdEventType.formSubmitted:
                            {
                                response = File.ReadAllText(HttpContext.Current.Server.MapPath("~/questionAsked.json"), Encoding.UTF8);
                                break;
                            }
                    }

                    if (!string.IsNullOrEmpty(response))
                    {
                        context.Response.StatusCode = 200;
                        context.Response.Write(response);
                        WriteLog("output:" + response);
                        return;
                    }                 
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

        private void WriteLog(string log)
        {
            File.AppendAllText(HttpContext.Current.Server.MapPath("~/log.txt"), log, Encoding.UTF8);
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